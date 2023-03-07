using System.Diagnostics.CodeAnalysis;
using FatCat.Worker.Wrappers;

namespace FatCat.Worker;

public interface ITimerWorkerItem : IDisposable
{
	void Start(IWorkerItem workerItem);

	void Stop();
}

[ExcludeFromCodeCoverage(Justification = "This is a wrapper for the timer thread thus it is not testable")]
public class TimerWorkerItem : ITimerWorkerItem
{
	private readonly ITimerWrapperFactory timerWrapperFactory;

	private int runs;
	private ITimerWrapper timer;
	private IWorkerItem workerItem;

	private int NumberOfTimesToRun => workerItem.NumberOfTimesToRun();

	private bool RunForever => NumberOfTimesToRun == -1;

	private bool UnderTimesToRun => runs < NumberOfTimesToRun;

	public TimerWorkerItem(ITimerWrapperFactory timerWrapperFactory) => this.timerWrapperFactory = timerWrapperFactory;

	public void Dispose() => timer?.Dispose();

	public void Start(IWorkerItem workerItem)
	{
		this.workerItem = workerItem;

		if (timer != null) return;

		timer = timerWrapperFactory.CreateTimerWrapper();

		if (workerItem.NumberOfTimesToRun() <= 0) timer.AutoReset = !this.workerItem.WaitOnWorkBeforeDelay();

		if (RunAtSpecificTime())
		{
			timer.AutoReset = false;

			timer.Interval = this.workerItem.GetSpecificTimeToRun() - DateTime.Now;
		}
		else timer.Interval = this.workerItem.Interval;

		timer.OnTimerElapsed = TimerElapsed;

		timer.Start();
	}

	public void Stop() => timer?.Dispose();

	private bool RunAtSpecificTime() => workerItem.GetSpecificTimeToRun() != DateTime.MinValue;

	private void TimerElapsed()
	{
		runs++;

		workerItem.DoWork().Wait();

		if (RunAtSpecificTime()) return;

		if (timer.AutoReset) return;

		if (RunForever || UnderTimesToRun) timer.Start();
	}
}