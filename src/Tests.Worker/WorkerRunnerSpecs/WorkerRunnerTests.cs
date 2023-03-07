using System.Reflection;
using FakeItEasy;
using FatCat.Toolkit;
using FatCat.Toolkit.Injection;
using FatCat.Worker;

namespace Tests.FatCat.Worker.WorkerRunnerSpecs;

public abstract class WorkerRunnerTests
{
	protected readonly WorkerRunner workerRunner;
	protected List<Assembly> assemblies;
	protected TimeSpan interval;
	protected IReflectionTools reflectionTools;
	protected ISystemScope systemScope;
	protected ITimerWorkerItem timerWorkerItem;
	protected ITimerWorkerItemFactory timeWorkerItemFactory;
	protected IWorker worker;
	protected List<Type> workerTypes;
	protected List<ITimerWorkerItem> timers;

	protected WorkerRunnerTests()
	{
		SetUpReflectionTools();
		SetUpTimeWrapperFactory();
		SetUpSystemScope();

		workerRunner = new WorkerRunner(reflectionTools, systemScope) { TimerWorkerItemFactory = timeWorkerItemFactory };
	}

	private void SetUpReflectionTools()
	{
		reflectionTools = A.Fake<IReflectionTools>();

		assemblies = new List<Assembly>
					{
						typeof(StartTests).Assembly,
						typeof(WorkerRunner).Assembly
					};

		A.CallTo(() => reflectionTools.GetDomainAssemblies())
		.Returns(assemblies);

		workerTypes = new List<Type>
					{
						typeof(StartTests),
						typeof(WorkerRunner)
					};

		A.CallTo(() => reflectionTools.FindTypesImplementing<IWorker>(A<List<Assembly>>._))
		.Returns(workerTypes);
	}

	private void SetUpSystemScope()
	{
		systemScope = A.Fake<ISystemScope>();

		SetUpWorker();

		A.CallTo(() => systemScope.Resolve(A<Type>._))
		.Returns(worker);
	}

	private void SetUpTimeWrapperFactory()
	{
		timeWorkerItemFactory = A.Fake<ITimerWorkerItemFactory>();

		timerWorkerItem = A.Fake<ITimerWorkerItem>();

		A.CallTo(() => timeWorkerItemFactory.CreateTimerWorkerItem())
		.Returns(timerWorkerItem);
	}

	private void SetUpWorker()
	{
		worker = A.Fake<IWorker>();

		interval = TimeSpan.FromMilliseconds(100);

		A.CallTo(() => worker.Interval)
		.Returns(interval);

		A.CallTo(() => worker.WaitOnWorkBeforeDelay())
		.Returns(true);
	}

	protected void SetUpFakeTimers()
	{
		timers = new List<ITimerWorkerItem>();

		for (var i = 0; i < 3; i++)
		{
			var timer = A.Fake<ITimerWorkerItem>();

			timers.Add(timer);

			workerRunner.Timers.Add(timer);
		}
	}
}