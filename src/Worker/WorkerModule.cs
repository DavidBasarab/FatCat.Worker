using System.Diagnostics.CodeAnalysis;
using Autofac;

namespace FatCat.Worker;

[ExcludeFromCodeCoverage(Justification = "Infrastructure cannot test")]
public class WorkerModule : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder
			.RegisterType<WorkerRunner>()
			.As<IWorkerRunner>()
			.SingleInstance();

		builder
			.RegisterType<TimerWrapperFactory>()
			.As<ITimerWrapperFactory>()
			.SingleInstance();
	}
}