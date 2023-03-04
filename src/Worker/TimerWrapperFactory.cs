using System.Diagnostics.CodeAnalysis;

namespace FatCat.Worker;

internal interface ITimerWrapperFactory
{
	ITimerWrapper CreateTimerWrapper();
}

[ExcludeFromCodeCoverage(Justification = "")]
internal class TimerWrapperFactory : ITimerWrapperFactory
{
	public ITimerWrapper CreateTimerWrapper() => new TimerWrapper();
}