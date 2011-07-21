namespace VDMS.I.Vehicle
{
    public enum ImportErrorCode : int
    {
        InvalidIssuedNumber = 1,
        UpdateFailed = 2,
        ItemNotExist = 3,
        Ok = 0,
        ImportDateTooLate = 4,
        ImportDateLocked = 5,
        InvalidImportDate = 6,
        ImportDateLessThanBaseDate = 7,
        OrdersDoesNotConfirmed = 8,
    }

    public class ImportError
    {
        public ImportError()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}