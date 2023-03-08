using FakeItEasy;
using FatCat.Worker;
using FatCat.Worker.Wrappers;
using Humanizer;

namespace Tests.FatCat.Worker.TimerWorkerItemSpecs;

public abstract class TimerWorkerTests
{
	protected readonly TimerWorker timerWorker;
	protected TimeSpan interval;
	protected int numberOfTimesToRun;
	protected ITimerWrapper timerWrapper;
	protected ITimerWrapperFactory timerWrapperFactory;
	protected bool waitForDelay;
	protected IWorker worker;

	protected TimerWorkerTests()
	{
		SetUpTimerWrapperFactory();
		SetUpWorkerItem();

		timerWorker = new TimerWorker(timerWrapperFactory);
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
		worker = A.Fake<IWorker>();

		interval = 45.Seconds();
		numberOfTimesToRun = -1;
		waitForDelay = true;

		A.CallTo(() => worker.Interval)
		.ReturnsLazily(() => interval);

		A.CallTo(() => worker.WaitOnWorkBeforeDelay())
		.ReturnsLazily(() => waitForDelay);
	}
}