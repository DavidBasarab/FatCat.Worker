namespace FatCat.Worker;

public interface IWorker
{
	TimeSpan Interval { get; }

	Task DoWork();

	bool WaitOnWorkBeforeDelay() => true;
}