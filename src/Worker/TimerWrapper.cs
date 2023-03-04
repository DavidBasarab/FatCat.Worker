using System.Diagnostics.CodeAnalysis;
using System.Timers;
using Timer = System.Timers.Timer;

namespace FatCat.Worker;

internal interface ITimerWrapper : IDisposable
{
	void Start(Action timerCallback, TimeSpan interval);

	void Stop();
}

[ExcludeFromCodeCoverage(Justification = "This is a wrapper for the timer thread thus it is not testable")]
internal class TimerWrapper : ITimerWrapper
{
	private Timer timer;
	private Action timerCallback;

	public void Dispose()
	{
		Stop();

		timer?.Dispose();
	}

	public void Start(Action timerCallback, TimeSpan interval)
	{
		this.timerCallback = timerCallback;

		if (timer != null) return;

		timer = new Timer(interval)
				{
					AutoReset = true,
				};

		timer.Elapsed += TimerElapsed;

		timer.Start();
	}

	public void Stop() => timer?.Stop();

	private void TimerElapsed(object sender, ElapsedEventArgs e) => timerCallback();
}