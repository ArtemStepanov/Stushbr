using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Qiwi.BillPayments.Model.Out;
using Stushbr.Application.Abstractions;
using Stushbr.Application.ExceptionHandling;
using Stushbr.Data.DataAccess.Sql;
using Stushbr.Domain.Models.Clients;
using Stushbr.PaymentsGatewayWeb.Application.Commands;
using Stushbr.PaymentsGatewayWeb.Application.Commands.Results;

namespace Stushbr.PaymentsGatewayWeb.Application.Handlers.Commands;

public sealed class OrderItemCommandHandler(
    StushbrDbContext dbContext,
    ILogger<OrderItemCommandHandler> logger,
    IQiwiService qiwiService
) : BaseRequestHandler<OrderItemCommand, OrderItemResponse>(dbContext)
{
    public override async Task<OrderItemResponse> Handle(OrderItemCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting or creating client \"{Email}\" info", request.ClientInfo.Email);

        var client = await FindOrCreateClientAsync(request, cancellationToken);
        var clientItem = client.ClientItems.First();

        BillResponse qiwiBill;
        if (clientItem.PaymentSystemBillId is null)
        {
            // Create new bill and update client item
            qiwiBill = await qiwiService.CreateBillAsync(clientItem);

            clientItem.PaymentSystemBillId = qiwiBill.BillId;
            clientItem.PaymentSystemBillDueDate = qiwiBill.ExpirationDateTime;

            DbContext.ClientItems.Update(clientItem);

            return new OrderItemResponse
            {
                Url = qiwiBill.PayUrl.ToString()
            };
        }

        if (clientItem.IsPaid)
        {
            if (clientItem.IsProcessed)
            {
                throw new BadRequestException(
                    "Вы уже оплатили данный продукт." +
                    " Информация должна быть у вас на почте." +
                    " Если вам кажется, что что-то могло пойти не так, напишите мне: @stushbrphoto"
                );
            }

            throw new BadRequestException(
                "Вы уже оплатили данный продукт." +
                " Пожалуйста, дождитесь обработки платежа." +
                " Если вам кажется, что что-то могло пойти не так, напишите мне: @stushbrphoto"
            );
        }

        qiwiBill = await qiwiService.GetBillInfoAsync(clientItem.PaymentSystemBillId);

        return new OrderItemResponse
        {
            Url = qiwiBill.PayUrl.ToString()
        };
    }

    private async Task<Client> FindOrCreateClientAsync(OrderItemCommand command, CancellationToken cancellationToken)
    {
        var client = await DbContext.Clients
            .Where(x => x.Email == command.ClientInfo.Email)
            .Include(x => x.ClientItems.Where(ci => ci.ItemId == command.Id && !ci.IsPaid && ci.PaymentSystemBillDueDate > DateTime.Now))
            .FirstOrDefaultAsync(cancellationToken);

        if (client is not null)
        {
            if (client.ClientItems.Count == 0)
            {
                client.ClientItems.Add(new ClientItem
                {
                    ItemId = command.Id
                });
            }

            return client;
        }

        logger.LogInformation("Client {Email} not found, creating new one", command.ClientInfo.Email);
        var createdClient = await DbContext.Clients.AddAsync(new Client
        {
            Email = command.ClientInfo.Email,
            FirstName = command.ClientInfo.FirstName,
            SecondName = command.ClientInfo.SecondName,
            PhoneNumber = command.ClientInfo.PhoneNumber,
            ClientItems = new List<ClientItem>
            {
                new()
                {
                    ItemId = command.Id
                }
            }
        }, cancellationToken);

        return createdClient.Entity;
    }
}