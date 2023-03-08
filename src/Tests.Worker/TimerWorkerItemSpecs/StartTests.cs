using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Worker.TimerWorkerItemSpecs;

public class StartTests : TimerWorkerTests
{
	[Fact]
	public void CreateTimerWrapper()
	{
		timerWorker.Start(worker);

		A.CallTo(() => timerWrapperFactory.CreateTimerWrapper())
		.MustHaveHappened();
	}

	[Fact]
	public void OnlyStartTheTimeOnce()
	{
		timerWorker.Start(worker);
		timerWorker.Start(worker);

		A.CallTo(() => timerWrapper.Start())
		.MustHaveHappenedOnceExactly();
	}

	[Fact]
	public void SetAutoResetOnTimer()
	{
		timerWorker.Start(worker);

		timerWrapper.AutoReset
					.Should()
					.Be(!waitForDelay);
	}

	[Fact]
	public void SetIntervalOnTimer()
	{
		timerWorker.Start(worker);

		timerWrapper.Interval
					.Should()
					.Be(interval);
	}

	[Fact]
	public void SetTimerElapsedFunction()
	{
		timerWorker.Start(worker);

		timerWrapper.OnTimerElapsed
					.Should()
					.NotBeNull();
	}

	[Fact]
	public void StartTimer()
	{
		timerWorker.Start(worker);

		A.CallTo(() => timerWrapper.Start())
		.MustHaveHappened();
	}
}