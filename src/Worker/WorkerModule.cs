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
			.RegisterType<TimerWorkerFactory>()
			.As<ITimerWorkerFactory>()
			.SingleInstance();

		builder.RegisterType<TimerWorker>()
				.As<ITimerWorker>();
	}
}