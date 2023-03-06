using FakeItEasy;
using FatCat.Worker;
using FatCat.Worker.Wrappers;
using FluentAssertions;
using Humanizer;
using Xunit;

namespace Tests.FatCat.Worker.TimerWorkerItemSpecs;

public class StartTests
{
	private readonly TimerWorkerItem timerWorkerItem;
	private TimeSpan interval;
	private int numberOfTimesToRun;
	private ITimerWrapper timerWrapper;
	private ITimerWrapperFactory timerWrapperFactory;
	private bool waitForDelay;
	private IWorkerItem workerItem;

	public StartTests()
	{
		SetUpTimerWrapperFactory();
		SetUpWorkerItem();

		timerWorkerItem = new TimerWorkerItem(timerWrapperFactory);
	}

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

	private void SetUpTimerWrapperFactory()
	{
		timerWrapperFactory = A.Fake<ITimerWrapperFactory>();

		timerWrapper = A.Fake<ITimerWrapper>();

		A.CallTo(() => timerWrapperFactory.CreateTimerWrapper())
		.Returns(timerWrapper);
	}

	private void SetUpWorkerItem()
	{
		workerItem = A.Fake<IWorkerItem>();

		interval = 45.Seconds();
		numberOfTimesToRun = -1;
		waitForDelay = true;

		A.CallTo(() => workerItem.Interval)
		.ReturnsLazily(() => interval);

		A.CallTo(() => workerItem.WaitOnWorkBeforeDelay())
		.ReturnsLazily(() => waitForDelay);

		A.CallTo(() => workerItem.NumberOfTimesToRun())
		.ReturnsLazily(() => numberOfTimesToRun);
	}
}