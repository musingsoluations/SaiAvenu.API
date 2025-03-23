using SriSai.Domain.Entity.Base;

namespace SriSai.Domain.Entity.Collection
{
    public class Payment : EntityBase
    {
        public decimal Amount { get; set; }
        public DateTime PaidDate { get; set; }
        public Guid FeeCollectionId { get; set; }
        public FeeCollectionEntity FeeCollection { get; set; }

        public string PaymentMethod { get; set; }
    }
}