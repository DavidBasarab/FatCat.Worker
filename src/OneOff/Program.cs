﻿using FatCat.Toolkit.Console;
using FatCat.Toolkit.Events;
using FatCat.Worker;
using Humanizer;

namespace OneOff;

public static class Program
{
	public static void Main(params string[] args)
	{
		var consoleUtilities = new ConsoleUtilities(new ManualWaitEvent());

		var workerRunner = WorkerRunner.Create();
		
		workerRunner.Start();

		consoleUtilities.WaitForExit();
	}
}