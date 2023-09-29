using Microsoft.EntityFrameworkCore;
using Stushbr.Application.Abstractions;
using Stushbr.Data.DataAccess.Sql;
using Stushbr.Domain.Models;

namespace Stushbr.Application.Services;

public class ClientService : CrudServiceBase<Client>, IClientService
{
    public ClientService(StushbrDbContext dbContext) : base(dbContext)
    {
    }

    public Task<Client?> TryGetClientByEmailAsync(string email)
    {
        return GetItemsAsync(x => x.Email == email).SingleOrDefaultAsync();
    }
}
