using Autofac;

namespace FatCat.Worker;

public class WorkerModule : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder
			.RegisterType<WorkerRunner>()
			.As<IWorkerRunner>();
	}
}