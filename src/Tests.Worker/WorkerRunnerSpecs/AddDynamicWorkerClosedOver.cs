using FakeItEasy;
using FatCat.Worker;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Worker.WorkerRunnerSpecs;

public class AddDynamicWorkerClosedOver : WorkerRunnerTests
{
	private readonly IDynamicWorker instanceWorker;

	public AddDynamicWorkerClosedOver()
	{
		instanceWorker = A.Fake<IDynamicWorker>();

		A.CallTo(() => instanceWorker.Interval)
		.Returns(interval);

		A.CallTo(() => instanceWorker.WaitOnWorkBeforeDelay())
		.Returns(true);

		A.CallTo(() => systemScope.Resolve(A<Type>._))
		.Returns(instanceWorker);
	}

	[Fact]
	public void CreateTimerWrapper()
	{
		workerRunner.AddDynamicWorker<TestDynWorker>();

		A.CallTo(() => timeWrapperFactory.CreateTimerWrapper())
		.MustHaveHappened();
	}

	[Fact]
	public void ResolveWorkerType()
	{
		workerRunner.AddDynamicWorker<TestDynWorker>();

		A.CallTo(() => systemScope.Resolve(typeof(TestDynWorker)))
		.MustHaveHappened();
	}

	[Fact]
	public void StartTimer()
	{
		workerRunner.AddDynamicWorker<TestDynWorker>();

		A.CallTo(() => timerWrapper.Start(instanceWorker.DoWork, interval, true))
		.MustHaveHappened();
	}

	[Fact]
	public void TimerIsAddedToCollection()
	{
		workerRunner.AddDynamicWorker<TestDynWorker>();

		workerRunner.Timers
					.Should()
					.Contain(timerWrapper);
	}

	private class TestDynWorker : IDynamicWorker
	{
		public TimeSpan Interval { get; }

		public bool OneTimeRun { get; }

		public Task DoWork() => throw new NotImplementedException();
	}
}