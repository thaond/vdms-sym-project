namespace VDMS.I.Service
{
	public enum VerifyExchangeErrorCode : int
	{
		OK = 0,
		InvalidSpareCode = 1,
		SpareNumberNotFound = 2,
		UpdateDataFailed = 6,
		WrongFormat = 7,
		CommentIsBlank = 8,
		ExchangePartHeaderNotFound = 9,
	}
}