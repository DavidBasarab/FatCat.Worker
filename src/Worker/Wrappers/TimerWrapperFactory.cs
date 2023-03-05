namespace FatCat.Worker.Wrappers;

public interface ITimerWrapperFactory
{
	ITimerWrapper CreateTimerWrapper();
}

public class TimerWrapperFactory : ITimerWrapperFactory
{
	public ITimerWrapper CreateTimerWrapper() => new TimerWrapper();
}