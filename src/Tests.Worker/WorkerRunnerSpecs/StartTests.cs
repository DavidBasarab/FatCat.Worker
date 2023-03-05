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

		A.CallTo(() => timeWrapperFactory.CreateTimerWrapper())
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

		A.CallTo(() => timerWrapper.Start(worker.DoWork, interval, true))
		.MustHaveHappened(workerTypes.Count, Times.Exactly);
	}
}