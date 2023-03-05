using System.Reflection;
using FakeItEasy;
using FatCat.Toolkit;
using FatCat.Toolkit.Injection;
using FatCat.Worker;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Worker.WorkerRunnerSpecs;

public class StartTests
{
	private readonly WorkerRunner workerRunner;
	private List<Assembly> assemblies;
	private TimeSpan interval;
	private IReflectionTools reflectionTools;
	private ISystemScope systemScope;
	private ITimerWrapper timerWrapper;
	private ITimerWrapperFactory timeWrapperFactory;
	private IWorker worker;
	private List<Type> workerTypes;

	public StartTests()
	{
		SetUpReflectionTools();
		SetUpTimeWrapperFactory();
		SetUpSystemScope();

		workerRunner = new WorkerRunner(reflectionTools, systemScope) { TimerWrapperFactory = timeWrapperFactory };
	}

	[Fact]
	public void CreateATimerWrapperForeachWorker()
	{
		workerRunner.Start();

		A.CallTo(() => timeWrapperFactory.CreateTimerWrapper())
		.MustHaveHappened(workerTypes.Count, Times.Exactly);
	}

	[Fact]
	public void CreateAWorkerForeachFoundType()
	{
		workerRunner.Start();

		foreach (var workerType in workerTypes)
		{
			A.CallTo(() => systemScope.Resolve(workerType))
			.MustHaveHappened();
		}
	}

	[Fact]
	public void FindTypesImplementingIWorker()
	{
		workerRunner.Start();

		A.CallTo(() => reflectionTools.FindTypesImplementing<IWorker>(assemblies))
		.MustHaveHappened();
	}

	[Fact]
	public void GetDomainAssemblies()
	{
		workerRunner.Start();

		A.CallTo(() => reflectionTools.GetDomainAssemblies())
		.MustHaveHappened();
	}

	[Fact]
	public void IfStartedDoNotStartAgain()
	{
		workerRunner.Start();
		workerRunner.Start();

		A.CallTo(() => reflectionTools.GetDomainAssemblies())
		.MustHaveHappenedOnceExactly();
	}

	[Fact]
	public void SaveTimersOnRunner()
	{
		workerRunner.Start();

		workerRunner.Timers
					.Should()
					.HaveCount(workerTypes.Count);
	}

	[Fact]
	public void StartTheTimerWrapper()
	{
		workerRunner.Start();

		A.CallTo(() => timerWrapper.Start(worker.DoWork, interval, true))
		.MustHaveHappened(workerTypes.Count, Times.Exactly);
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
		timeWrapperFactory = A.Fake<ITimerWrapperFactory>();

		timerWrapper = A.Fake<ITimerWrapper>();

		A.CallTo(() => timeWrapperFactory.CreateTimerWrapper())
		.Returns(timerWrapper);
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
}