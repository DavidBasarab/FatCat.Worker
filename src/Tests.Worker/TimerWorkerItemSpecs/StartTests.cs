using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Worker.TimerWorkerItemSpecs;

public class StartTests : TimerWorkerItemTests
{
	[Fact]
	public void CreateTimerWrapper()
	{
		timerWorkerItem.Start(workerItem);

		A.CallTo(() => timerWrapperFactory.CreateTimerWrapper())
		.MustHaveHappened();
	}

	[Fact]
	public void OnlyStartTheTimeOnce()
	{
		timerWorkerItem.Start(workerItem);
		timerWorkerItem.Start(workerItem);

		A.CallTo(() => timerWrapper.Start())
		.MustHaveHappenedOnceExactly();
	}

	[Fact]
	public void SetAutoResetOnTimer()
	{
		timerWorkerItem.Start(workerItem);

		timerWrapper.AutoReset
					.Should()
					.Be(!waitForDelay);
	}

	[Fact]
	public void SetIntervalOnTimer()
	{
		timerWorkerItem.Start(workerItem);

		timerWrapper.Interval
					.Should()
					.Be(interval);
	}

	[Fact]
	public void SetTimerElapsedFunction()
	{
		timerWorkerItem.Start(workerItem);

		timerWrapper.OnTimerElapsed
					.Should()
					.NotBeNull();
	}

	[Fact]
	public void StartTimer()
	{
		timerWorkerItem.Start(workerItem);

		A.CallTo(() => timerWrapper.Start())
		.MustHaveHappened();
	}
}