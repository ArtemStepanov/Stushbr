using System.ComponentModel.DataAnnotations;

namespace Stushbr.PaymentsGatewayWeb.ViewModels.Requests;

public class ClientRequest
{
    public string FirstName { get; set; }

    public string SecondName { get; set; }

    [EmailAddress(ErrorMessage = "Адрес электронной почты указан неверно")]
    public string Email { get; set; }

    [Phone(ErrorMessage = "Номер указан неверно")]
    public string PhoneNumber { get; set; }
}
