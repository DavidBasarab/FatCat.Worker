using FatCat.Toolkit.Console;
using FatCat.Worker;
using Humanizer;

namespace OneOff;

public class TestWorker1 : IWorker
{
	public TimeSpan Interval => 1.Seconds();

	public Task DoWork()
	{
		ConsoleLog.WriteMagenta("Doing test worker 1");

		return Task.CompletedTask;
	}
}