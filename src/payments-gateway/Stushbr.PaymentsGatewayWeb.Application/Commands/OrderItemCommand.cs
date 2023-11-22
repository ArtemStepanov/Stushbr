using Stushbr.Core.Mediatr.Abstractions;
using Stushbr.PaymentsGatewayWeb.Application.Commands.Results;
using System.ComponentModel.DataAnnotations;

namespace Stushbr.PaymentsGatewayWeb.Application.Commands;

public record OrderItemCommand(
    [Range(1, int.MaxValue, ErrorMessage = "Не указан идентификатор продукта")]
    int Id,
    [Required(ErrorMessage = "Не заполнена информация о клиенте")]
    OrderItemCommand.ClientRequest ClientInfo
) : ICommand<OrderItemResponse>
{
    public record ClientRequest(
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Неверно указано имя покупателя")]
        string FirstName,
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Неверно указана фамилия покупателя")]
        string SecondName,
        [EmailAddress(ErrorMessage = "Адрес электронной почты указан неверно")]
        string Email,
        [StringLength(20, MinimumLength = 12, ErrorMessage = "Номер указан неверно")]
        string PhoneNumber
    );
}