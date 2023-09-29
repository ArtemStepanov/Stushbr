using System.ComponentModel.DataAnnotations;

namespace Stushbr.PaymentsGatewayWeb.ViewModels.Requests;

public class ClientRequest
{
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Неверно указано имя покупателя")]
    public string FirstName { get; set; } = default!;

    [StringLength(100, MinimumLength = 1, ErrorMessage = "Неверно указана фамилия покупателя")]
    public string SecondName { get; set; } = default!;

    [EmailAddress(ErrorMessage = "Адрес электронной почты указан неверно")]
    public string Email { get; set; } = default!;

    [StringLength(12, MinimumLength = 12, ErrorMessage = "Номер указан неверно")]
    public string PhoneNumber { get; set; } = default!;
}
