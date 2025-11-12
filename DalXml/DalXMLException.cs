
namespace Dal
{
    [Serializable]
    internal class DalXMLException : Exception
    {
        public DalXMLException()
        {
        }

        public DalXMLException(string? message) : base(message)
        {
        }

        public DalXMLException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}