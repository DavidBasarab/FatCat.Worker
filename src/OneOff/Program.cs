using FatCat.Toolkit.Console;
using FatCat.Toolkit.Events;
using FatCat.Worker;
using Humanizer;

namespace OneOff;

public static class Program
{
	public static void Main(params string[] args)
	{
		var consoleUtilities = new ConsoleUtilities(new ManualWaitEvent());

		ConsoleLog.WriteMagenta("This is working");

		var timerWrapper = new TimerWrapper();

		timerWrapper.Start(() => ConsoleLog.WriteMagenta("This is working"), 1.Seconds());

		consoleUtilities.WaitForExit();
	}
}