namespace SriSai.API.DTOs.Collection
{
    public class PaymentReminderRequestDto
    {
        public string ApartmentName { get; set; } = string.Empty;
        public decimal RequiredAmount { get; set; }
        public string RequiredFor { get; set; } = string.Empty;
        public DateOnly ForMonth { get; set; }
        public DateOnly PaymentDueDate { get; set; }
    }
}