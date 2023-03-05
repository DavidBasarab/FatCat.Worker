namespace FatCat.Worker;

public interface IWorkerItem
{
	TimeSpan Interval { get; }

	Task DoWork();

	bool WaitOnWorkBeforeDelay() => true;
}

public interface IWorker : IWorkerItem { }

public interface IDynamicWorker : IWorkerItem
{
	bool OneTimeRun { get; }
}