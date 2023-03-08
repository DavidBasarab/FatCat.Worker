namespace FatCat.Worker;

public interface IWorker
{
	TimeSpan Interval { get; }

	Task DoWork();

	bool WaitOnWorkBeforeDelay() => true;
}

public interface IDynamicWorker : IWorker { }

public interface IDynamicRunAtSpecificTimeWorker : IRunAtSpecificTimeWorker { }

public interface IRunLimitedNumberWorker : IWorker
{
	int NumberOfTimesToRun { get; }
}

public interface IRunAtSpecificTimeWorker : IWorker
{
	DateTime TimeToRun { get; }
}