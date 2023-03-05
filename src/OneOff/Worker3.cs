using FatCat.Fakes;
using FatCat.Toolkit.Console;
using FatCat.Worker;
using Humanizer;

namespace OneOff;

public class Worker3 : IWorker
{
	private readonly TimeSpan delayTime = Faker.RandomInt(1, 4).Seconds();

	public TimeSpan Interval => 1.Seconds();

	public bool WaitOnWorkBeforeDelay() => false;

	public async Task DoWork()
	{
		ConsoleLog.WriteWhite($"Worker 3 going to wait for {delayTime} seconds");

		await Task.Delay(delayTime);

		ConsoleLog.WriteWhite($"Worker 3 has waited for {delayTime} seconds. Done");
	}
}