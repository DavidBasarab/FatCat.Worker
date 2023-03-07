using System.Diagnostics.CodeAnalysis;
using FatCat.Toolkit.Console;
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
	private ITimerWrapper timer;
	private IWorkerItem workerItem;

	public TimerWorkerItem(ITimerWrapperFactory timerWrapperFactory) => this.timerWrapperFactory = timerWrapperFactory;

	public void Dispose() => timer?.Dispose();

	public void Start(IWorkerItem workerItem)
	{
		this.workerItem = workerItem;

		if (timer != null) return;

		timer = timerWrapperFactory.CreateTimerWrapper();

		timer.AutoReset = !this.workerItem.WaitOnWorkBeforeDelay();
		timer.Interval = this.workerItem.Interval;

		timer.OnTimerElapsed = TimerElapsed;

		timer.Start();
	}

	public void Stop() => timer?.Dispose();

	public void TimerElapsed()
	{
		// TODO : Unit Test

		// workerItem.DoWork().Wait();
		//
		// if (!timer.AutoReset) timer.Start();

		ConsoleLog.WriteGreen("TEMP");
		ConsoleLog.WriteGreen("TEMP");
	}
}