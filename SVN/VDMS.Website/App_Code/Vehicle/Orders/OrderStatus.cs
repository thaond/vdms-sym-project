using System.Linq;

namespace VDMS.I.Vehicle
{
    public enum OrderStatus : int
    {
        Draft = 0,
        Sent = 1,
        Confirmed = 2,      // co so ben tiptop tuong ung
        Deleted = 3,
        Approved = 4,       // dang xu ly, chua co so ben tiptop tuong ung
        PaymentConfirmed = 5,      // ke toan xac nhan thanh toan
    }
    public class OrderStatusAct
    {
        public static bool CanChangeBonusStatus(int status)
        {
            return (new int[] { 
                (int)OrderStatus.Draft, 
                (int)OrderStatus.Sent, 
                (int)OrderStatus.Approved, 
            }).Contains(status);
        }

        public static string GetName(int status)
        {
            return GetName((OrderStatus)status);
        }
        public static string GetName(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.Draft: return "Chưa gửi";
                case OrderStatus.Confirmed: return "Xác nhận";
                case OrderStatus.Deleted: return "Xoá";
                case OrderStatus.Approved: return "Đang xử lý";
                case OrderStatus.PaymentConfirmed: return "Đã thanh toán";
                default: return "";
            }
        }
    }

    public enum ImportItemStatus : int
    {
        NotArrived = 0,
        Imported = 1,
        AdmitTemporarily = 2
    }

    public enum DeliveredOrderStatus : int
    {
        NotDeliveredAll = 1,
        DeliveredAll = 2
    }
}