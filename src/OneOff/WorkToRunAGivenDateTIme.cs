using FatCat.Toolkit.Console;
using FatCat.Worker;
using Humanizer;

namespace OneOff;

public class WorkToRunAGivenDateTime : IRunAtSpecificTimeWorker
{
	public TimeSpan Interval => 1.Seconds();

	public DateTime TimeToRun { get; }

	public WorkToRunAGivenDateTime() => TimeToRun = DateTime.Now.AddSeconds(5);

	public Task DoWork()
	{
		ConsoleLog.WriteGreen($"I am running at the given time of {TimeToRun}");

		return Task.CompletedTask;
	}

	public DateTime GetSpecificTimeToRun() => TimeToRun;
}