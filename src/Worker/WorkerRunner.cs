using System.Collections.Concurrent;
using FatCat.Toolkit;
using FatCat.Toolkit.Console;
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
	private ITimerWrapperFactory timerWrapperFactory;

	internal ConcurrentBag<ITimerWrapper> Timers { get; } = new();

	internal ITimerWrapperFactory TimerWrapperFactory
	{
		get => timerWrapperFactory ??= systemScope.Resolve<ITimerWrapperFactory>();
		set => timerWrapperFactory = value;
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
			if (workerType == typeof(IDynamicWorker)) continue;

			StartWorker(workerType);
		}

		started = true;
	}

	public void Stop() => Dispose();

	private void DisposeAllTimers()
	{
		foreach (var timer in Timers) timer.Dispose();
	}

	private void StartTimeForWorkerInstance(IWorkerItem worker)
	{
		var timer = TimerWrapperFactory.CreateTimerWrapper();

		timer.Start(worker.DoWork, worker.Interval, worker.WaitOnWorkBeforeDelay());

		Timers.Add(timer);
	}

	private void StartWorker(Type workerType)
	{
		ConsoleLog.WriteDarkYellow($"   Worker Type <{workerType.FullName}>");

		var worker = systemScope.Resolve(workerType) as IWorkerItem;

		StartTimeForWorkerInstance(worker);
	}

	private void StopAllTimers()
	{
		foreach (var timer in Timers) timer.Stop();
	}
}