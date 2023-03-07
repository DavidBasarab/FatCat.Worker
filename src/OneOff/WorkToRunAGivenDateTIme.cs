using FatCat.Toolkit.Console;
using FatCat.Worker;
using Humanizer;

namespace OneOff;

public class WorkToRunAGivenDateTIme : IWorker
{
	private readonly DateTime timeToRun;

	public TimeSpan Interval => 1.Seconds();

	public WorkToRunAGivenDateTIme() => timeToRun = DateTime.Now.AddSeconds(5);

	public Task DoWork()
	{
		ConsoleLog.WriteGreen($"I am running at the given time of {timeToRun}");

		return Task.CompletedTask;
	}

	public DateTime GetSpecificTimeToRun() => timeToRun;
}