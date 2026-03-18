using Hangfire;
using SENSEI.HANGFIRE;

namespace SENSEI.WEB.SchedulerJobs
{
    public class HangfireJobs
    {
        private readonly IRecurringJobManager _jobManager;

        public HangfireJobs(IRecurringJobManager jobManager)
        {
            _jobManager = jobManager;
        }

        public void RegisterJobs()
        {
            _jobManager.AddOrUpdate<ExpirationJob>(
                "lesson-expiration-job",
                job => job.Execute(),
                "59 23 * * *" // Every day at 11:59 PM
            );
        }
    }
}
