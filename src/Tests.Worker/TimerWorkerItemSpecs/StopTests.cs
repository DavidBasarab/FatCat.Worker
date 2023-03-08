using FakeItEasy;
using Xunit;

namespace Tests.FatCat.Worker.TimerWorkerItemSpecs;

public class StopTests : TimerWorkerItemTests
{
	public StopTests() { timerWorker.Start(workerItem); }

	[Fact]
	public void CallDisposeOnTimer()
	{
		timerWorker.Stop();

		A.CallTo(() => timerWrapper.Dispose())
		.MustHaveHappened();
	}
}