namespace BuildingBlocks.Framework.Entities
{
    public class Error
    {
        public string Message { get; }
        public string Info { get; }
        public Error(string message, string info)
        {
            Message = message;
            Info = info;
        }
        public Error(string message) =>
            Message = message;
    }
}
