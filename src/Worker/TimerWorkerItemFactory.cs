using System.Diagnostics.CodeAnalysis;
using FatCat.Toolkit.Injection;

namespace FatCat.Worker;

public interface ITimerWorkerItemFactory
{
	ITimerWorkerItem CreateTimerWorkerItem();
}

[ExcludeFromCodeCoverage(Justification = "")]
internal class TimerWorkerItemFactory : ITimerWorkerItemFactory
{
	private readonly ISystemScope systemScope;

	public TimerWorkerItemFactory(ISystemScope systemScope) => this.systemScope = systemScope;

	public ITimerWorkerItem CreateTimerWorkerItem() => systemScope.Resolve<ITimerWorkerItem>();
}