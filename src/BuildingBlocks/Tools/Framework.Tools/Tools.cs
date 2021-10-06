using System;

namespace BuildingBlocks.Framework.Tools
{
    public static class Tools
    {
        public static bool IsGuid(string value) => Guid.TryParse(value, out _);
        
    }
}