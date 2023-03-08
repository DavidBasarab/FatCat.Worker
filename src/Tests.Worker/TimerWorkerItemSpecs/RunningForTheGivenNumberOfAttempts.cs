using FakeItEasy;
using FatCat.Worker;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Worker.TimerWorkerItemSpecs;

public class RunningForTheGivenNumberOfAttempts : TimerWorkerItemTests
{
	private readonly IRunLimitedNumberWorker numberWorkerItem;

	public RunningForTheGivenNumberOfAttempts()
	{
		numberOfTimesToRun = 4;
		waitForDelay = false;

		numberWorkerItem = A.Fake<IRunLimitedNumberWorker>();

		A.CallTo(() => numberWorkerItem.NumberOfTimesToRun)
		.Returns(numberOfTimesToRun);
	}

	[Fact]
	public void IfNumberOfTimesToRunIsGreaterThan0SetAutoRestToFalse()
	{
		timerWorker.Start(numberWorkerItem);

		timerWrapper.AutoReset
					.Should()
					.BeFalse();
	}

	[Fact]
	public void OnlyStartNumberOfTimesToRun()
	{
		timerWorker.Start(numberWorkerItem);

		for (var i = 0; i < numberOfTimesToRun + 2; i++) timerWrapper.OnTimerElapsed();

		A.CallTo(() => timerWrapper.Start())
		.MustHaveHappened(numberOfTimesToRun, Times.Exactly);
	}
}