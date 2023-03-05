using FakeItEasy;
using FatCat.Worker;
using Xunit;

namespace Tests.FatCat.Worker.WorkerRunnerSpecs;

public class StopTests : WorkerRunnerTests
{
	private readonly List<ITimerWrapper> timers;

	public StopTests()
	{
		timers = new List<ITimerWrapper>();

		for (var i = 0; i < 3; i++)
		{
			var timer = A.Fake<ITimerWrapper>();

			timers.Add(timer);

			workerRunner.Timers.Add(timer);
		}
	}

	[Fact]
	public void WillStopEachTimer()
	{
		workerRunner.Stop();

		foreach (var timer in timers)
		{
			A.CallTo(() => timer.Stop())
			.MustHaveHappened();
		}
	}
}