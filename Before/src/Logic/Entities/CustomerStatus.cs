using System;
using Logic.Utils;
using Remotion.Linq.Utilities;

namespace Logic.Entities
{
    public class CustomerStatus:ValueObject<CustomerStatus>
    {
        public static CustomerStatus Regular = new CustomerStatus(CustomerStatusType.Regular, ExpirationDate.Infinite);

        public virtual CustomerStatusType Type { get; private set; }

        private DateTime? _expirationDate;

        public ExpirationDate ExpirationDate => (ExpirationDate)this._expirationDate;

        public bool IsAdvanced => this.Type == CustomerStatusType.Advanced && !this.ExpirationDate.IsExpired;

        private CustomerStatus()
        {
            
        }

        private CustomerStatus(CustomerStatusType type, ExpirationDate expirationDate) 
            : this()
        {
            this.Type = type;
            this._expirationDate = expirationDate ?? throw new ArgumentEmptyException(nameof(expirationDate));
        }

        protected override bool EqualsCore(CustomerStatus other)
        {
            return this.Type == other.Type && this.ExpirationDate == other.ExpirationDate;
        }

        protected override int GetHashCodeCore()
        {
            return this.Type.GetHashCode() ^ this.ExpirationDate.GetHashCode();
        }

        public decimal GetDiscount() => IsAdvanced ? 0.25m : 0m;

        public CustomerStatus Promote()
        {
            this.Type = CustomerStatusType.Advanced;
            this._expirationDate = DateTime.UtcNow.AddYears(1);
            return this;
        }
    }

    public enum CustomerStatusType
    {
        Regular = 1,
        Advanced = 2
    }
}
