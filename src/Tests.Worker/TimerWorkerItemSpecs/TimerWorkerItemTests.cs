using FakeItEasy;
using FatCat.Worker;
using FatCat.Worker.Wrappers;
using Humanizer;

namespace Tests.FatCat.Worker.TimerWorkerItemSpecs;

public abstract class TimerWorkerItemTests
{
	protected readonly TimerWorkerItem timerWorkerItem;
	protected TimeSpan interval;
	protected int numberOfTimesToRun;
	protected ITimerWrapper timerWrapper;
	protected ITimerWrapperFactory timerWrapperFactory;
	protected bool waitForDelay;
	protected IWorkerItem workerItem;

	protected TimerWorkerItemTests()
	{
		SetUpTimerWrapperFactory();
		SetUpWorkerItem();

		timerWorkerItem = new TimerWorkerItem(timerWrapperFactory);
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