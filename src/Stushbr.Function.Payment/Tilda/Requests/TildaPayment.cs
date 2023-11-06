namespace Stushbr.Function.Payment.Tilda.Requests;

public sealed record TildaPayment(string Name, string Email, string Phone, TildaPayment.PaymentItem Payment, string FormId, string FormName)
{
    public sealed record PaymentItem(string Sys, string SystranId, string OrderId, ProductItem[] Products, string Amount);

    public sealed record ProductItem(string Name, int Quantity, float Amount, string Price, string ExternalId);
}