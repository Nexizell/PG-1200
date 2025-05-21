namespace Facebook.Unity
{
    public interface IFacebookLogger
    {
        void Log(string msg);

        void Info(string msg);

        void Warn(string msg);

        void Error(string msg);
    }
}
