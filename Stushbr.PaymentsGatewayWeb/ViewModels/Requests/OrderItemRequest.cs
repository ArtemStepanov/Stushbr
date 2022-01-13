using System.ComponentModel.DataAnnotations;

namespace Stushbr.PaymentsGatewayWeb.ViewModels.Requests;

public class OrderItemRequest
{
    [Required(ErrorMessage = "Не заполнена информация о клиенте")]
    public ClientRequest ClientInfo { get; set; }
}
