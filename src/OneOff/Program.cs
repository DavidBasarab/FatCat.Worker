using Autofac;
using FatCat.Toolkit.Console;
using FatCat.Toolkit.Injection;
using FatCat.Worker;
using Humanizer;

namespace OneOff;

public static class Program
{
	public static async Task Main(params string[] args)
	{
		SystemScope.Initialize(new ContainerBuilder(), ScopeOptions.SetLifetimeScope);

		var consoleUtilities = SystemScope.Container.Resolve<IConsoleUtilities>();

		var workerRunner = SystemScope.Container.Resolve<IWorkerRunner>();

		workerRunner.Start();

		ConsoleLog.WriteYellow("After worker runner start");

		await Task.Delay(15.Seconds());

		ConsoleLog.WriteYellow("After delay");

		workerRunner.Stop();

		ConsoleLog.Write("After stop");

		await Task.Delay(10.Seconds());

		ConsoleLog.WriteDarkBlue("Exiting . . . . .");
	}
}