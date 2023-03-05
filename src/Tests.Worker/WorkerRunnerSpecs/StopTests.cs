using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Worker.WorkerRunnerSpecs;

public class StopTests : WorkerRunnerTests
{
	public StopTests() => SetUpFakeTimers();

	[Fact]
	public void AfterStopWillClearTimers()
	{
		workerRunner.Stop();

		workerRunner.Timers
					.Should()
					.BeEmpty();
	}
	
	[Fact]
	public void DisposeAllTimers()
	{
		workerRunner.Stop();
		
		foreach (var timer in timers)
		{
			A.CallTo(() => timer.Dispose())
			.MustHaveHappened();
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