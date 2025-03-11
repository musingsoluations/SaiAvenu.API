using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace SriSai.API.DTOs.Building;

public record CreateApartmentDto(
    [Required] string ApartmentNumber,
    [Required] Guid OwnerId,
    Guid? RenterId
);