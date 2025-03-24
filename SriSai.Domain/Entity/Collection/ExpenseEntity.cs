using SriSai.Domain.Entity.Base;

namespace SriSai.Domain.Entity.Collection;

public class ExpenseEntity : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public ExpenseType Type { get; set; }
    public decimal Amount { get; set; }
    public DateOnly Date { get; set; }
}

public enum ExpenseType
{
    Recurring = 1,
    AdHoc = 2
}
