namespace Facebook.Unity.Canvas
{
    public interface ICanvasFacebook : IFacebook
    {
        void Pay(string product, string action, int quantity, int? quantityMin, int? quantityMax, string requestId, string pricepointId, string testCurrency, FacebookDelegate<IPayResult> callback);
    }
}
