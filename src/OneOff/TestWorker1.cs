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

public class TestWorker2 : IWorker
{
	public TimeSpan Interval => 2.Seconds();

	public Task DoWork()
	{
		ConsoleLog.WriteDarkCyan("This is the test worker 2");

		return Task.CompletedTask;
	}
}