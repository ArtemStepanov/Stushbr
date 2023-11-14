using System.ComponentModel.DataAnnotations;

namespace Stushbr.Domain.Abstractions;

public interface IIdentifier
{
    [Key]
    int Id { get; set; }
}
