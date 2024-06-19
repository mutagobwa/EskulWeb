namespace Eskul.Custom
{
    public interface ILoggerErr
    {
        void Info(string info);
        void Request(string message, string msgId, bool xml = false);
        void Response(string message, string resp, bool xml = false);
        void Error(string message, Exception ex);
    }
}
