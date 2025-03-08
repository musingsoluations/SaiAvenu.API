using SriSai.Domain.Interface;

namespace SriSai.Domain.Imp
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}