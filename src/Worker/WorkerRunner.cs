using System.Collections.Concurrent;
using FatCat.Toolkit;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Injection;

namespace FatCat.Worker;

public interface IWorkerRunner : IDisposable
{
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

	public void Dispose()
	{
		// Stop();
		//
		// foreach (var timer in Timers) timer.Dispose();
		//
		// Timers.Clear();
	}

	public void Start()
	{
		if (started) return;

		var currentAssemblies = reflectionTools.GetDomainAssemblies();

		var foundWorkerTypes = reflectionTools.FindTypesImplementing<IWorker>(currentAssemblies);

		foreach (var workerType in foundWorkerTypes) StartWorker(workerType);

		started = true;
	}

	public void Stop()
	{
		foreach (var timer in Timers) timer.Stop();

		Timers.Clear();
	}

	private void StartWorker(Type workerType)
	{
		ConsoleLog.WriteDarkYellow($"   Worker Type <{workerType.FullName}>");

		var worker = systemScope.Resolve(workerType) as IWorker;

		var timer = TimerWrapperFactory.CreateTimerWrapper();

		timer.Start(worker.DoWork, worker.Interval, worker.WaitOnWorkBeforeDelay());

		Timers.Add(timer);
	}
}