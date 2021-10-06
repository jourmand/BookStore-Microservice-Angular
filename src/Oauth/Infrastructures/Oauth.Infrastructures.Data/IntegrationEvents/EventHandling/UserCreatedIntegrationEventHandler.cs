using System;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Framework.Entities;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Oauth.Infrastructures.Data.Entities;
using Serilog.Context;

namespace Oauth.Infrastructures.Data.IntegrationEvents.EventHandling
{
    public class UserCreatedIntegrationEventHandler :
        IConsumer<Events.UserCreatedIntegrationEvent>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserCreatedIntegrationEventHandler> _logger;

        public UserCreatedIntegrationEventHandler(UserManager<ApplicationUser> userManager,
            ILogger<UserCreatedIntegrationEventHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<Events.UserCreatedIntegrationEvent> context)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{context.MessageId}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", context.MessageId,
                    "", context.Message);

                var identityResult = await _userManager.CreateAsync(
                    new ApplicationUser { Email = context.Message.Email, UserName = context.Message.Email },
                    context.Message.Password);

                if (!identityResult.Succeeded)
                    throw new Exception(@$"User with email {context.Message.Email} ended with {identityResult.Succeeded} Error 
                                    {string.Join(", ", identityResult.Errors.Select(o => o.Description))}");
            }
        }
    }
}
