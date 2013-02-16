using System;
using System.Diagnostics;
using Finance.Services.General;
using Quartz;
using Quartz.Impl;
using log4net;

namespace Finance.Services.Scheduling
{
    public class Scheduler<TJob> : IStartableService where TJob : IJob
    {
        private readonly ILog _log = LogManager.GetLogger(typeof (Scheduler<TJob>));

        private IScheduler _scheduler;

        public void Start()
        {
            StartScheduler();

            var jobDetail = new JobDetail("myJob", null, typeof (TJob));

            _log.Debug(string.Format("Job Details: {0}", jobDetail));

            var trigger = TriggerUtils.MakeSecondlyTrigger(20);

            trigger.StartTimeUtc = DateTime.UtcNow;
            trigger.Name = "myTrigger";

            _scheduler.ScheduleJob(jobDetail, trigger);

            _log.Info("Job is scheduled");
            _log.Info("Starting process is finished");
        }

        private void StartScheduler()
        {
            var factory = new StdSchedulerFactory();
            _scheduler = factory.GetScheduler();
            _scheduler.Start();

            _log.Info("Scheduler is started");
        }

        public void Stop()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            _log.Info("Scheduler is stopped (waiting for jobs to complete)");
            _scheduler.Shutdown(true);

            stopwatch.Stop();
            long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            _log.Info(string.Format(
                "Stopping process is finished. Waiting for jobs is completed, elapsed time - {0} ms",
                elapsedMilliseconds));
        }
    }
}