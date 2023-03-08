using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Worker.TimerWorkerItemSpecs;

public class StartTests : TimerWorkerItemTests
{
	[Fact]
	public void CreateTimerWrapper()
	{
		timerWorker.Start(workerItem);

		A.CallTo(() => timerWrapperFactory.CreateTimerWrapper())
		.MustHaveHappened();
	}

	[Fact]
	public void OnlyStartTheTimeOnce()
	{
		timerWorker.Start(workerItem);
		timerWorker.Start(workerItem);

		A.CallTo(() => timerWrapper.Start())
		.MustHaveHappenedOnceExactly();
	}

	[Fact]
	public void SetAutoResetOnTimer()
	{
		timerWorker.Start(workerItem);

		timerWrapper.AutoReset
					.Should()
					.Be(!waitForDelay);
	}

	[Fact]
	public void SetIntervalOnTimer()
	{
		timerWorker.Start(workerItem);

		timerWrapper.Interval
					.Should()
					.Be(interval);
	}

	[Fact]
	public void SetTimerElapsedFunction()
	{
		timerWorker.Start(workerItem);

		timerWrapper.OnTimerElapsed
					.Should()
					.NotBeNull();
	}

	[Fact]
	public void StartTimer()
	{
		timerWorker.Start(workerItem);

		A.CallTo(() => timerWrapper.Start())
		.MustHaveHappened();
	}
}