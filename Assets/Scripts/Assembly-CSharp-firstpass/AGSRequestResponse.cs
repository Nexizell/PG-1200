public class AGSRequestResponse
{
	public string error;

	public int userData;

	protected const string PLATFORM_NOT_SUPPORTED_ERROR = "PLATFORM_NOT_SUPPORTED";

	protected const string JSON_PARSE_ERROR = "ERROR_PARSING_JSON";

	public bool IsError()
	{
		return !string.IsNullOrEmpty(error);
	}
}
