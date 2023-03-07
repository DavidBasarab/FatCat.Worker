using FakeItEasy;
using Xunit;

namespace Tests.FatCat.Worker.TimerWorkerItemSpecs;

public class TimerElapsedTests : TimerWorkerItemTests
{
	public TimerElapsedTests() => timerWorkerItem.Start(workerItem);

	[Fact]
	public void IfAutoResetIsFalseStartTimer()
	{
		A.CallTo(() => timerWrapper.AutoReset)
		.Returns(false);

		timerWrapper.OnTimerElapsed();

		A.CallTo(() => timerWrapper.Start())
		.MustHaveHappened(2, Times.Exactly);
	}

	[Fact]
	public void IfAutRestartIsTrueDoNotStart()
	{
		A.CallTo(() => timerWrapper.AutoReset)
		.Returns(true);

		timerWrapper.OnTimerElapsed();

		A.CallTo(() => timerWrapper.Start())
		.MustHaveHappened(1, Times.Exactly);
	}

	[Fact]
	public void WillExecuteWorkerItemDoWork()
	{
		timerWrapper.OnTimerElapsed();

		A.CallTo(() => workerItem.DoWork())
		.MustHaveHappened();
	}
}