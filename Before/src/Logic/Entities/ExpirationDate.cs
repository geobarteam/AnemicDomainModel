using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Utils;

namespace Logic.Entities
{
    public class ExpirationDate : ValueObject<ExpirationDate>
    {
        public DateTime? Date { get; }

        private ExpirationDate(DateTime? date)
        {
            this.Date = date;
        }

        public bool IsExpired => this.Date != null && this.Date < DateTime.Now;
        public static readonly ExpirationDate Infinite = new ExpirationDate(null);

        public static Result<ExpirationDate> Create(DateTime date)
        {
            if (date < DateTime.UtcNow)
            {
                return Result.Fail<ExpirationDate>("Expiration date cannot be in the past!");
            }

            return Result.Ok(new ExpirationDate(date));
        }

        protected override bool EqualsCore(ExpirationDate other)
        {
            return this.Date == other.Date;
        }

        protected override int GetHashCodeCore()
        {
            return this.Date.GetHashCode();
        }

        public static implicit operator DateTime?(ExpirationDate expirationDate)
        {
            return expirationDate.Date;
        }

        public static explicit operator ExpirationDate(DateTime? expirationDate)
        {
            if (expirationDate.HasValue)
                return ExpirationDate.Create(expirationDate.Value).Value;

            return ExpirationDate.Infinite;
        }
    }
}
