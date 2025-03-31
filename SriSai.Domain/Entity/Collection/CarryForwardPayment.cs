using SriSai.Domain.Entity.Base;
using System;

namespace SriSai.Domain.Entity.Collection;

public class CarryForwardPayment : EntityBase
{
    public decimal Amount { get; set; }
    public DateOnly Date { get; set; }
    public string? Description { get; set; }
}
