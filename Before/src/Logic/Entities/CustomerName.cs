using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Utils;

namespace Logic.Entities
{
    public class CustomerName : ValueObject<CustomerName>
    {
        public string Value { get; }

        public CustomerName(string value)
        {
            Value = value;
        }

        protected override bool EqualsCore(CustomerName other)
        {
            return Value.Equals(other.Value, StringComparison.CurrentCultureIgnoreCase);
        }

        protected override int GetHashCodeCore()
        {
            return Value.GetHashCode();
        }
    }
}
