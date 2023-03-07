using System.Diagnostics.CodeAnalysis;
using Autofac;
using FatCat.Toolkit.Console;

namespace FatCat.Worker;

[ExcludeFromCodeCoverage(Justification = "Infrastructure cannot test")]
public class WorkerModule : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		ConsoleLog.WriteGreen("Loading Worker Module");

		builder
			.RegisterType<WorkerRunner>()
			.As<IWorkerRunner>()
			.SingleInstance();

		builder
			.RegisterType<TimerWorkerItemFactory>()
			.As<ITimerWorkerItemFactory>()
			.SingleInstance();

		builder.RegisterType<TimerWorkerItem>()
				.As<ITimerWorkerItem>();
	}
}