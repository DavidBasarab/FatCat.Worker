using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Worker.TimerWorkerItemSpecs;

public class RunningForTheGivenNumberOfAttempts : TimerWorkerItemTests
{
	public RunningForTheGivenNumberOfAttempts()
	{
		numberOfTimesToRun = 4;
		waitForDelay = false;
	}

	[Fact]
	public void IfNumberOfTimesToRunIsGreaterThan0SetAutoRestToFalse()
	{
		timerWorkerItem.Start(workerItem);

		timerWrapper.AutoReset
					.Should()
					.BeFalse();
	}

	[Fact]
	public void OnlyStartNumberOfTimesToRun()
	{
		timerWorkerItem.Start(workerItem);

		for (var i = 0; i < numberOfTimesToRun + 2; i++) timerWrapper.OnTimerElapsed();

		A.CallTo(() => timerWrapper.Start())
		.MustHaveHappened(numberOfTimesToRun + 1, Times.Exactly);
	}
}