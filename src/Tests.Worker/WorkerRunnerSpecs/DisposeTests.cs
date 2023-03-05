using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Worker.WorkerRunnerSpecs;

public class DisposeTests : WorkerRunnerTests
{
	public DisposeTests() => SetUpFakeTimers();

	[Fact]
	public void ClearTimersAfterDispose()
	{
		workerRunner.Dispose();

		workerRunner.Timers
					.Should()
					.BeEmpty();
	}

	[Fact]
	public void WillDisposeOfAllTheTimers()
	{
		workerRunner.Dispose();

		foreach (var timer in timers)
		{
			A.CallTo(() => timer.Dispose())
			.MustHaveHappened();
		}
	}

	[Fact]
	public void WillStopAllTimers()
	{
		workerRunner.Dispose();

		foreach (var timer in timers)
		{
			A.CallTo(() => timer.Stop())
			.MustHaveHappened();
		}
	}
}