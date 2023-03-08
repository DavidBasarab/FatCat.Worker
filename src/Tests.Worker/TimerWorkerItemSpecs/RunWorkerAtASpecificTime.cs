using FakeItEasy;
using FatCat.Worker;
using FluentAssertions;
using Humanizer;
using Xunit;

namespace Tests.FatCat.Worker.TimerWorkerItemSpecs;

public class RunWorkerAtASpecificTime : TimerWorkerTests
{
	private readonly DateTime timeToRun;
	private readonly IRunAtSpecificTimeWorker timeToRunWorker;

	public RunWorkerAtASpecificTime()
	{
		timeToRun = DateTime.Now.AddSeconds(25);

		timeToRunWorker = A.Fake<IRunAtSpecificTimeWorker>();

		A.CallTo(() => timeToRunWorker.TimeToRun)
		.Returns(timeToRun);
	}

	[Fact]
	public void AutoRestIsFalse()
	{
		timerWorker.Start(timeToRunWorker);

		timerWrapper.AutoReset
					.Should()
					.BeFalse();
	}

	[Fact]
	public void SetIntervalToBeTimeToRunMinusDateTimeNow()
	{
		timerWorker.Start(timeToRunWorker);

		var expectedTime = timeToRunWorker.TimeToRun - DateTime.Now;

		timerWrapper.Interval
					.Should()
					.BeCloseTo(expectedTime, 1.Seconds());
	}
}