
namespace VDMS.I.Vehicle
{
	public enum ItemStatus : int
	{
        NotArrived = 0, //chua den
		Imported = 1, // Da nhap
		AdmitTemporarily = 2, //
		Moved = 3,
		Sold = 4,
		Return = 5,
		Redundant = 6,
		Lacked = 7,
		VoucherCompensated = 8, // not used in ItemInstance
		ChangeEngineNumber = 9, // not used in ItemInstance
        ReceivedFromMoving = 10,    // Used in transhis, flags that an item is imported to a branch in moving action
	}

	public enum ReturnItemErrorCode
	{
		VerhicleNotExist,
		ShippingDetailError,
		SaveStateFailed,
		CanNotSendToVMEP,
        FinishReturnFailed,
        CancelRequestFailed,
        ItemHasOutOfStock, 
        InventoryLocked,
        ReleaseMoreThanImport,
        ReleaseMoreThanNow
	}
	public enum ReturnItemStatus : int
	{
		Proposed = 1,   // de nghi tra xe va chua xac nhan
		Allowed = 2,    // Da xac nhan tra xe
		NotAllow = 0,   // Tu choi tra xe
		Returned = 4,   // Dai ly xac nhan da tra xe khi nha may den thu hoi
        DealerCanceled = 5, // dai ly huy yeu cau tra xe
	}
}
