using FatCat.Toolkit.Console;
using FatCat.Worker;
using Humanizer;

namespace OneOff;

public class TestDynamicWorker1 : IDynamicWorker
{
	public TimeSpan Interval => 1.Seconds();

	public bool OneTimeRun => true;

	public Task DoWork()
	{
		ConsoleLog.WriteMagenta("This is a dynamic worker only should run when I ask");
		
		return Task.CompletedTask;
	}
}