using System.Diagnostics.CodeAnalysis;
using FatCat.Toolkit.Injection;

namespace FatCat.Worker;

public interface ITimerWorkerItemFactory
{
	ITimerWorker CreateTimerWorkerItem();
}

[ExcludeFromCodeCoverage(Justification = "")]
internal class TimerWorkerItemFactory : ITimerWorkerItemFactory
{
	private readonly ISystemScope systemScope;

	public TimerWorkerItemFactory(ISystemScope systemScope) => this.systemScope = systemScope;

	public ITimerWorker CreateTimerWorkerItem() => systemScope.Resolve<ITimerWorker>();
}