using System.Diagnostics.CodeAnalysis;
using FatCat.Toolkit.Injection;

namespace FatCat.Worker;

public interface ITimerWorkerFactory
{
	ITimerWorker CreateTimerWorker();
}

[ExcludeFromCodeCoverage(Justification = "")]
internal class TimerWorkerFactory : ITimerWorkerFactory
{
	private readonly ISystemScope systemScope;

	public TimerWorkerFactory(ISystemScope systemScope) => this.systemScope = systemScope;

	public ITimerWorker CreateTimerWorker() => systemScope.Resolve<ITimerWorker>();
}