namespace Facebook.Unity
{
    public interface IInternalResult : IResult
    {
        string CallbackId { get; }
    }
}
