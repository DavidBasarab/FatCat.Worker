﻿namespace FatCat.Worker;

public interface IWorker
{
	TimeSpan Interval { get; }

	Task DoWork();

	bool WaitOnWorkBeforeDelay() => true;
}

public interface IDynamicWorker : IWorker { }

public interface IDynamicRunAtSpecificTimeWorker : IDynamicWorker, IRunAtSpecificTimeWorker { }

public interface IDynamicLimitedNumberWorker : IDynamicWorker, IRunLimitedNumberWorker { }

public interface IRunLimitedNumberWorker : IWorker
{
	int NumberOfTimesToRun { get; }
}

public interface IRunAtSpecificTimeWorker : IWorker
{
	DateTime TimeToRun { get; }
}