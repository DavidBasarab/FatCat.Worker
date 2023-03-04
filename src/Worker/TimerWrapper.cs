using FatCat.Toolkit.Console;

namespace FatCat.Worker;

internal interface ITimerWrapper
{
	void Testing();
}

internal class TimerWrapper : ITimerWrapper
{
	public void Testing()
	{
		ConsoleLog.WriteMagenta("This is just testing internals visible too");
	}
}