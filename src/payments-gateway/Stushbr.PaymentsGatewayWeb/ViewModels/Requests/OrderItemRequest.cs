using System.ComponentModel.DataAnnotations;

namespace Stushbr.PaymentsGatewayWeb.ViewModels.Requests;

public class OrderItemRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Не указан идентификатор продукта")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Не заполнена информация о клиенте")]
    public ClientRequest ClientInfo { get; set; } = default!;
}