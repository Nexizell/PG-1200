namespace Facebook.Unity.Mobile
{
    public interface IMobileFacebookCallbackHandler : IFacebookCallbackHandler
    {
        void OnAppInviteComplete(string message);

        void OnFetchDeferredAppLinkComplete(string message);

        void OnRefreshCurrentAccessTokenComplete(string message);
    }
}
