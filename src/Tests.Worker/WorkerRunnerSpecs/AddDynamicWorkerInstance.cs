using FakeItEasy;
using FatCat.Worker;
using FluentAssertions;
using Humanizer;
using Xunit;

namespace Tests.FatCat.Worker.WorkerRunnerSpecs;

public class AddDynamicWorkerInstance : WorkerRunnerTests
{
	private readonly TestDynWorker workerInstance;

	public AddDynamicWorkerInstance() => workerInstance = new TestDynWorker();

	[Fact]
	public void ATimerToTimers()
	{
		workerRunner.AddDynamicWorker(workerInstance);

		workerRunner.Timers
					.Should()
					.Contain(timerWorker);
	}

	[Fact]
	public void CreateTimerWrapper()
	{
		workerRunner.AddDynamicWorker(workerInstance);

		A.CallTo(() => timeWorkerFactory.CreateTimerWorker())
		.MustHaveHappened();
	}

	[Fact]
	public void StartTheTimer()
	{
		workerRunner.AddDynamicWorker(workerInstance);

		A.CallTo(() => timerWorker.Start(workerInstance))
		.MustHaveHappened();
	}

	private class TestDynWorker : IDynamicWorker
	{
		public TimeSpan Interval => 4.Seconds();

		public Task DoWork() => throw new NotImplementedException();

		public bool WaitOnWorkBeforeDelay() => true;
	}
}