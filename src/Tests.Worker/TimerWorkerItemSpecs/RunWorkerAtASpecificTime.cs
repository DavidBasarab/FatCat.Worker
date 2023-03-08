using FakeItEasy;
using FatCat.Worker;

namespace Tests.FatCat.Worker.TimerWorkerItemSpecs;

public class RunWorkerAtASpecificTime : TimerWorkerTests
{
	private DateTime timeToRun;
	private IRunAtSpecificTimeWorker timeToRunWorker;

	public RunWorkerAtASpecificTime()
	{
		timeToRun = DateTime.Now.AddSeconds(25);

		timeToRunWorker = A.Fake<IRunAtSpecificTimeWorker>();

		A.CallTo(() => timeToRunWorker.TimeToRun)
		.Returns(timeToRun);
	}
}