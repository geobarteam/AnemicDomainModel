using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Utils;

namespace Logic.Entities
{
    public class Euros : ValueObject<Euros>
    {
        private const decimal MaxDollarAmount = 1_000_000;
        private decimal _value;

        public decimal Value
        {
            get => this._value;
        }

        private Euros(decimal value)
        {
            this._value = value;
        }

        public static Result<Euros> Create(decimal euro)
        {
            if (euro < 0)
                return Result.Fail<Euros>("Euro cannot be negative!");
            if (euro > MaxDollarAmount)
                return Result.Fail<Euros>("Euro exceeds max limit!");
            if (euro % 0.01m > 0)
                return Result.Fail<Euros>("Euro should not contain a part be less than a cent!");

            return Result.Ok<Euros>(new Euros(euro));
        }

        public static Euros Of(decimal euro)
        {
            return Create(euro).Value;
        }

        protected override bool EqualsCore(Euros other)
        {
            return this.Value == other.Value;
        }

        protected override int GetHashCodeCore()
        {
            return this.Value.GetHashCode();
        }

        public static implicit operator decimal(Euros euros)
        {
            return euros.Value;
        }

        public static explicit operator Euros(decimal euros)
        {
            return Euros.Create(euros).Value;
        }

        public static Euros operator *(Euros euros, decimal multiplier)
        {
            return new Euros(euros.Value * multiplier);
        }

        public static Euros operator *(Euros euros1, Euros euros2)
        {
            return new Euros(euros1.Value * euros2.Value);
        }

        public static Euros operator +(Euros euros1, Euros euros2)
        {
            return new Euros(euros1.Value + euros2.Value);
        }

        public static Euros operator -(Euros euros1, Euros euros2)
        {
            return new Euros(euros1.Value - euros2.Value);
        }
    }
}
