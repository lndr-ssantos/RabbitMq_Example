using FluentScheduler;
using MessegesReceiver.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MessegesReceiver.Jobs
{
    public class OrderJob : IJob
    {
        private readonly IServiceProvider _serviceProvider;

        public OrderJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Execute()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var orderService = scope.ServiceProvider.GetRequiredService<OrderService>();
                orderService.Execute();
            }
        }
    }
}
