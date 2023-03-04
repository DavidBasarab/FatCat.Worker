using FatCat.Toolkit;
using FatCat.Toolkit.Console;

namespace FatCat.Worker;

public interface IWorkerRunner : IDisposable
{
	void Start();

	void Stop();
}

public class WorkerRunner : IWorkerRunner
{
	public static WorkerRunner Create() => new(new ReflectionTools());

	private readonly IReflectionTools reflectionTools;

	public WorkerRunner(IReflectionTools reflectionTools) => this.reflectionTools = reflectionTools;

	public void Dispose() { }

	public void Start()
	{
		var currentAssemblies = reflectionTools.GetDomainAssemblies();

		foreach (var assembly in currentAssemblies) ConsoleLog.WriteCyan($"{assembly.FullName}");

		var foundWorkerTypes = reflectionTools.FindTypesImplementing<IWorker>(currentAssemblies);

		foreach (var workerType in foundWorkerTypes) ConsoleLog.WriteDarkYellow($"   Worker Type <{workerType.FullName}>");
	}

	public void Stop() { }
}