using System.Diagnostics.CodeAnalysis;
using System.Timers;
using Timer = System.Timers.Timer;

namespace FatCat.Worker.Wrappers;

public interface ITimerWrapper : IDisposable
{
	bool AutoReset { get; set; }

	TimeSpan Interval { get; set; }

	Action OnTimerElapsed { get; set; }

	void Start();
}

[ExcludeFromCodeCoverage(Justification = "Simple wrapper for the timer")]
public class TimerWrapper : ITimerWrapper
{
	private Timer timer;

	public bool AutoReset { get; set; }

	public TimeSpan Interval { get; set; }

	public Action OnTimerElapsed { get; set; }

	public void Dispose() => CleanUpTimer();

	public void Start()
	{
		if (timer is not null) CleanUpTimer();

		timer = new Timer(Interval) { AutoReset = AutoReset };

		timer.Elapsed += TimerElapsed;

		timer.Start();
	}

	private void CleanUpTimer()
	{
		timer.Stop();
		timer.Close();
		timer.Dispose();
	}

	private void TimerElapsed(object sender, ElapsedEventArgs e) => OnTimerElapsed();
}