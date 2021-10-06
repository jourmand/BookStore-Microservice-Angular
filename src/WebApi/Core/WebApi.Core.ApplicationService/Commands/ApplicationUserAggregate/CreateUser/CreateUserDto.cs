﻿using System;

namespace WebApi.Core.ApplicationService.Commands.ApplicationUserAggregate.CreateUser
{
    public class CreateUserCommandDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    
}
