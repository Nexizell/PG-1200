namespace Facebook.Unity.Canvas
{
    public interface ICanvasFacebookCallbackHandler : IFacebookCallbackHandler
    {
        void OnPayComplete(string message);

        void OnFacebookAuthResponseChange(string message);

        void OnUrlResponse(string message);

        void OnHideUnity(bool hide);
    }
}
