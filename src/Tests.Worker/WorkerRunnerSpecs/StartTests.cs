using FakeItEasy;
using FatCat.Worker;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Worker.WorkerRunnerSpecs;

public class StartTests : WorkerRunnerTests
{
	[Fact]
	public void CreateATimerWrapperForeachWorker()
	{
		workerRunner.Start();

		A.CallTo(() => timeWorkerItemFactory.CreateTimerWorkerItem())
		.MustHaveHappened(workerTypes.Count, Times.Exactly);
	}

	[Fact]
	public void CreateAWorkerForeachFoundType()
	{
		workerRunner.Start();

		foreach (var workerType in workerTypes)
		{
			A.CallTo(() => systemScope.Resolve(workerType))
			.MustHaveHappened();
		}
	}

	[Theory]
	[InlineData(typeof(IDynamicWorker))]
	[InlineData(typeof(IRunAtSpecificTimeWorker))]
	[InlineData(typeof(IRunLimitedNumberWorker))]
	public void DoNotUserWorkersInterfaces(Type typeNotToUse)
	{
		workerTypes = new List<Type> { typeNotToUse };

		workerRunner.Start();

		A.CallTo(() => systemScope.Resolve(typeNotToUse))
		.MustNotHaveHappened();
	}

	[Fact]
	public void FindTypesImplementingIWorker()
	{
		workerRunner.Start();

		A.CallTo(() => reflectionTools.FindTypesImplementing<IWorker>(assemblies))
		.MustHaveHappened();
	}

	[Fact]
	public void GetDomainAssemblies()
	{
		workerRunner.Start();

		A.CallTo(() => reflectionTools.GetDomainAssemblies())
		.MustHaveHappened();
	}

	[Fact]
	public void IfStartedDoNotStartAgain()
	{
		workerRunner.Start();
		workerRunner.Start();

		A.CallTo(() => reflectionTools.GetDomainAssemblies())
		.MustHaveHappenedOnceExactly();
	}

	[Fact]
	public void SaveTimersOnRunner()
	{
		workerRunner.Start();

		workerRunner.Timers
					.Should()
					.HaveCount(workerTypes.Count);
	}

	[Fact]
	public void StartTheTimerWrapper()
	{
		workerRunner.Start();

		A.CallTo(() => timerWorker.Start(worker))
		.MustHaveHappened(workerTypes.Count, Times.Exactly);
	}

	private class DynamicTestWorker : IDynamicWorker
	{
		public TimeSpan Interval { get; }

		public Task DoWork() => throw new NotImplementedException();
	}

	private class RunAtSpecificTimeTestWorker : IRunAtSpecificTimeWorker
	{
		public TimeSpan Interval { get; }

		public DateTime TimeToRun { get; }

		public Task DoWork() => throw new NotImplementedException();
	}

	private class RunLimitedNumberTestWorker : IRunLimitedNumberWorker
	{
		public TimeSpan Interval { get; }

		public int NumberOfTimesToRun { get; }

		public Task DoWork() => throw new NotImplementedException();
	}
}