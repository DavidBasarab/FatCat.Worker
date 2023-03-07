using FakeItEasy;
using Xunit;

namespace Tests.FatCat.Worker.TimerWorkerItemSpecs;

public class StopTests : TimerWorkerItemTests
{
	public StopTests() { timerWorkerItem.Start(workerItem); }

	[Fact]
	public void CallDisposeOnTimer()
	{
		timerWorkerItem.Stop();

		A.CallTo(() => timerWrapper.Dispose())
		.MustHaveHappened();
	}
}