namespace SriSai.Domain.Entity.Base
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public Guid? UpdatedById { get; set; }
    }
}