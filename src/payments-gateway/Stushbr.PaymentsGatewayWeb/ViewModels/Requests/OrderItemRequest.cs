using System.ComponentModel.DataAnnotations;

namespace Stushbr.PaymentsGatewayWeb.ViewModels.Requests;

public class OrderItemRequest
{
    [Required(ErrorMessage = "Не указан идентификатор продукта")]
    public string Id { get; set; } = default!;
    
    [Required(ErrorMessage = "Не заполнена информация о клиенте")]
    public ClientRequest ClientInfo { get; set; } = default!;
}
