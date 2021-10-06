﻿using System.Collections.Generic;

namespace BuildingBlocks.Framework.Entities
{
    public class ErrorDetails
    {
        public string Title { get; private set; }
        public int? Status { get; private set; }
        public string Instance { get; private set; }
        public IEnumerable<Error> Errors { get; private set; }

        public ErrorDetails(int? status, string title, string instance, IEnumerable<Error> errors = null)
        {
            Title = title;
            Status = status;
            Instance = instance;
            Errors = errors;
        }
    }
}
