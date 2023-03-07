namespace FatCat.Worker;

public interface IWorkerItem
{
	TimeSpan Interval { get; }

	Task DoWork();

	int NumberOfTimesToRun() => -1;

	bool WaitOnWorkBeforeDelay() => true;
}

public interface IWorker : IWorkerItem { }

public interface IDynamicWorker : IWorkerItem { }