using System.Diagnostics.CodeAnalysis;
using System.Timers;
using Timer = System.Timers.Timer;

namespace FatCat.Worker;

public interface ITimerWrapper : IDisposable
{
	void Start(IWorkerItem workerItem);

	void Stop();
}

[ExcludeFromCodeCoverage(Justification = "This is a wrapper for the timer thread thus it is not testable")]
internal class TimerWrapper : ITimerWrapper
{
	private Timer timer;
	private IWorkerItem workerItem;

	public void Dispose()
	{
		Stop();

		timer?.Dispose();
	}

	public void Start(IWorkerItem workerItem)
	{
		this.workerItem = workerItem;

		if (timer != null) return;

		timer = new Timer(this.workerItem.Interval) { AutoReset = !this.workerItem.WaitOnWorkBeforeDelay() };

		timer.Elapsed += TimerElapsed;

		timer.Start();
	}

	public void Stop() => timer?.Stop();

	private void TimerElapsed(object sender, ElapsedEventArgs e)
	{
		workerItem.DoWork().Wait();

		if (!timer.AutoReset) timer.Start();
	}
}