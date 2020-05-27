using FluentScheduler;
using System;

namespace MessegesReceiver.Jobs
{
    public class JobsRegistry : Registry
    {
        public JobsRegistry(IServiceProvider serviceProvider)
        {
            NonReentrantAsDefault();
            Schedule(new OrderJob(serviceProvider)).ToRunNow().AndEvery(15).Seconds();
        }
    }
}
