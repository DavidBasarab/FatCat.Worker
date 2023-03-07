﻿namespace FatCat.Worker;

public interface IWorkerItem
{
	TimeSpan Interval { get; }

	Task DoWork();

	bool WaitOnWorkBeforeDelay() => true;
}

public interface IWorker : IWorkerItem { }

public interface IDynamicWorker : IWorkerItem { }

public interface IRunLimitedNumberWorker : IWorker
{
	int NumberOfTimesToRun { get; }
}

public interface IRunAtSpecificTimeWorker : IWorker
{
	DateTime TimeToRun { get; }
}