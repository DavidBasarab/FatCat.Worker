using FakeItEasy;
using Xunit;

namespace Tests.FatCat.Worker.TimerWorkerItemSpecs;

public class StopTests : TimerWorkerTests
{
	public StopTests() { timerWorker.Start(worker); }

	[Fact]
	public void CallDisposeOnTimer()
	{
		timerWorker.Stop();

		A.CallTo(() => timerWrapper.Dispose())
		.MustHaveHappened();
	}
}