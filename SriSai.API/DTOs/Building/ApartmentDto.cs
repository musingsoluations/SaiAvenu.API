public class ApartmentDto
{
    public required string ApartmentNumber { get; set; }
    public required Guid OwnerId { get; set; }
    public required Guid? RenterId { get; set; }
}

