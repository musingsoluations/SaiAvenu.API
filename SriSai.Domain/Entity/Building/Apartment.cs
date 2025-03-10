using SriSai.Domain.Entity.Base;
using SriSai.Domain.Entity.Users;

namespace SriSai.Domain.Entity.Building;

public class Apartment : EntityBase
{
    public required string ApartmentNumber { get; set; }
    public required Guid OwnerId { get; set; }
    public UserEntity Owner { get; set; }
    public Guid? RenterId { get; set; }
    public UserEntity? Renter { get; set; }
}