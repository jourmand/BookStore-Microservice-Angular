using System;
using System.Collections.Generic;
using BuildingBlocks.Framework.Entities;

namespace WebApi.Core.Domain.Exceptions
{
    public static class DomainExceptions
    {
        public class InvalidEntityState : Exception
        {
            public IEnumerable<Error> Errors { get; set; }
            public InvalidEntityState(IEnumerable<Error> errors) =>
                Errors = errors;

            public InvalidEntityState(string error) =>
                Errors = new[] { new Error(error) };
        }
    }
}
