using System.Collections.Concurrent;
using FatCat.Toolkit;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Extensions;
using FatCat.Toolkit.Injection;

namespace FatCat.Worker;

public interface IWorkerRunner : IDisposable
{
	void AddDynamicWorker<T>() where T : IDynamicWorker;

	void AddDynamicWorker(IDynamicWorker instance);

	void Start();

	void Stop();
}

public class WorkerRunner : IWorkerRunner
{
	private readonly IReflectionTools reflectionTools;
	private readonly ISystemScope systemScope;

	private bool started;
	private ITimerWorkerFactory timerWorkerFactory;

	internal ConcurrentBag<ITimerWorker> Timers { get; } = new();

	internal ITimerWorkerFactory TimerWorkerFactory
	{
		get => timerWorkerFactory ??= systemScope.Resolve<ITimerWorkerFactory>();
		set => timerWorkerFactory = value;
	}

	public WorkerRunner(IReflectionTools reflectionTools,
						ISystemScope systemScope)
	{
		this.reflectionTools = reflectionTools;
		this.systemScope = systemScope;
	}

	public void AddDynamicWorker<T>() where T : IDynamicWorker => StartWorker(typeof(T));

	public void AddDynamicWorker(IDynamicWorker instance) => StartTimeForWorkerInstance(instance);

	public void Dispose()
	{
		StopAllTimers();
		DisposeAllTimers();

		Timers.Clear();
	}

	public void Start()
	{
		if (started) return;

		var currentAssemblies = reflectionTools.GetDomainAssemblies();

		var foundWorkerTypes = reflectionTools.FindTypesImplementing<IWorker>(currentAssemblies);

		foreach (var workerType in foundWorkerTypes)
		{
			if (SkipType(workerType)) continue;

			StartWorker(workerType);
		}

		started = true;
	}

	public void Stop() => Dispose();

	private void DisposeAllTimers()
	{
		foreach (var timer in Timers) timer.Dispose();
	}

	private static bool SkipType(Type workerType)
	{
		if (workerType.Implements(typeof(IDynamicWorker)) || workerType.Implements(typeof(IRunAtSpecificTimeWorker)) || workerType.Implements(typeof(IRunLimitedNumberWorker))) return true;

		return workerType == typeof(IDynamicWorker) || workerType == typeof(IRunAtSpecificTimeWorker) || workerType == typeof(IRunLimitedNumberWorker);
	}

	private void StartTimeForWorkerInstance(IWorker worker)
	{
		var timer = TimerWorkerFactory.CreateTimerWorker();

		timer.Start(worker);

		Timers.Add(timer);
	}

	private void StartWorker(Type workerType)
	{
		ConsoleLog.WriteDarkYellow($"   Worker Type <{workerType.FullName}>");

		var worker = systemScope.Resolve(workerType) as IWorker;

		StartTimeForWorkerInstance(worker);
	}

	private void StopAllTimers()
	{
		foreach (var timer in Timers) timer.Stop();
	}
}