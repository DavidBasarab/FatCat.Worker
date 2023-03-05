using System.Reflection;
using FakeItEasy;
using FatCat.Toolkit;
using FatCat.Toolkit.Injection;
using FatCat.Worker;

namespace Tests.FatCat.Worker.WorkerRunnerSpecs;

public class StartTests
{
	private List<Assembly> assmblies;
	private IReflectionTools reflectionTools;
	private ISystemScope systemScope;
	private ITimerWrapper timerWrapper;
	private ITimerWrapperFactory timeWrapperFactory;
	private IWorker worker;
	private WorkerRunner workerRunner;
	private List<Type> workerTypes;

	public StartTests()
	{
		SetUpReflectionTools();
		SetUpTimeWrapperFactory();
		SetUpSystemScope();

		workerRunner = new WorkerRunner(reflectionTools, systemScope) { TimerWrapperFactory = timeWrapperFactory };
	}

	private void SetUpReflectionTools()
	{
		reflectionTools = A.Fake<IReflectionTools>();

		assmblies = new List<Assembly>
					{
						typeof(StartTests).Assembly,
						typeof(WorkerRunner).Assembly
					};

		A.CallTo(() => reflectionTools.GetDomainAssemblies())
		.Returns(assmblies);

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

		worker = A.Fake<IWorker>();

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
}