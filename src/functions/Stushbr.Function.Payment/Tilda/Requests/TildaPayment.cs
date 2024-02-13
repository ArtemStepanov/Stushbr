using System.Linq;

namespace Stushbr.Function.Payment.Tilda.Requests;

public sealed record TildaPayment(string Name, string Email, string Phone, TildaPayment.PaymentItem Payment, string FormId, string FormName)
{
    public sealed record PaymentItem(string Sys, string SystranId, string OrderId, ProductItem[] Products, string Amount);

    public sealed record ProductItem(string Name, int Quantity, float Amount, string Price, string ExternalId);

    public bool IsValid()
    {
        // todo: add proper validation
        return !string.IsNullOrWhiteSpace(Name) &&
               !string.IsNullOrWhiteSpace(Email) &&
               !string.IsNullOrWhiteSpace(Phone) &&
               !string.IsNullOrWhiteSpace(FormId) &&
               !string.IsNullOrWhiteSpace(FormName) &&
               !string.IsNullOrWhiteSpace(Payment.Sys) &&
               !string.IsNullOrWhiteSpace(Payment.SystranId) &&
               !string.IsNullOrWhiteSpace(Payment.OrderId) &&
               Payment.Products.Length > 0 &&
               Payment.Products.All(x => !string.IsNullOrWhiteSpace(x.Name) &&
                                         x is { Quantity: > 0, Amount: > 0 } &&
                                         !string.IsNullOrWhiteSpace(x.Price) &&
                                         !string.IsNullOrWhiteSpace(x.ExternalId));
    }
}