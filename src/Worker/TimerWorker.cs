using System.Diagnostics.CodeAnalysis;
using FatCat.Toolkit.Logging;
using FatCat.Worker.Wrappers;

namespace FatCat.Worker;

public interface ITimerWorker : IDisposable
{
	void Start(IWorker workerToStart);

	void Stop();
}

[ExcludeFromCodeCoverage(Justification = "This is a wrapper for the timer thread thus it is not testable")]
public class TimerWorker : ITimerWorker
{
	private readonly IToolkitLogger logger;
	private readonly ITimerWrapperFactory timerWrapperFactory;

	private int runs;
	private ITimerWrapper timer;
	private IWorker worker;

	private int NumberOfTimesToRun
	{
		get
		{
			if (worker is IRunLimitedNumberWorker timesWorker) return timesWorker.NumberOfTimesToRun;

			return -1;
		}
	}

	private bool RunForever => NumberOfTimesToRun == -1;

	private bool UnderTimesToRun => runs < NumberOfTimesToRun;

	public TimerWorker(ITimerWrapperFactory timerWrapperFactory,
						IToolkitLogger logger)
	{
		this.timerWrapperFactory = timerWrapperFactory;
		this.logger = logger;
	}

	public void Dispose() => timer?.Dispose();

	public void Start(IWorker workerToStart)
	{
		worker = workerToStart;

		if (timer != null) return;

		timer = timerWrapperFactory.CreateTimerWrapper();

		if (worker is not IRunLimitedNumberWorker) timer.AutoReset = !worker.WaitOnWorkBeforeDelay();

		if (RunAtSpecificTime())
		{
			timer.AutoReset = false;

			var timeWorkItem = worker as IRunAtSpecificTimeWorker;

			timer.Interval = timeWorkItem.TimeToRun - DateTime.Now;
		}
		else timer.Interval = worker.Interval;

		timer.OnTimerElapsed = TimerElapsed;

		timer.Start();
	}

	public void Stop() => timer?.Dispose();

	private bool RunAtSpecificTime() => worker is IRunAtSpecificTimeWorker;

	private void TimerElapsed()
	{
		runs++;

		logger.Debug($"Doing work on <{worker.GetType().FullName}>");

		try { worker.DoWork().Wait(); }
		catch (Exception e)
		{
			logger.Error($"Error running worker <{worker.GetType().FullName}>");
			logger.Exception(e);
		}

		if (RunAtSpecificTime()) return;

		if (timer.AutoReset) return;

		if (RunForever || UnderTimesToRun) timer.Start();
	}
}