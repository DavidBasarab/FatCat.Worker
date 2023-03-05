using Autofac;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Injection;
using FatCat.Worker;

namespace OneOff;

public static class Program
{
	public static void Main(params string[] args)
	{
		SystemScope.Initialize(new ContainerBuilder(), ScopeOptions.SetLifetimeScope);

		var consoleUtilities = SystemScope.Container.Resolve<IConsoleUtilities>();

		var workerRunner = SystemScope.Container.Resolve<IWorkerRunner>();

		workerRunner.Start();

		consoleUtilities.WaitForExit();
	}
}