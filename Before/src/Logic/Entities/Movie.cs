using System;
using Newtonsoft.Json;

namespace Logic.Entities
{
    public abstract class Movie : Entity
    {
        public virtual string Name { get; protected set; }

        protected virtual LicensingModel LicensingModel { get; set; }

        public virtual Euros CalculatePrice(CustomerStatus customerStatus)
        {
            var modifier = 1 - customerStatus.GetDiscount();
            return this.CalculatePriceCore() * modifier;
        }

        protected abstract Euros CalculatePriceCore();


        public abstract ExpirationDate GetExpirationDate();
    }

    public class TwoDaysMovie : Movie
    {
        protected override Euros CalculatePriceCore()
        {
            return Euros.Of(4);
        }

        public override ExpirationDate GetExpirationDate()
        {
            return (ExpirationDate)DateTime.UtcNow.AddDays(2);
        }
    }

    public class LongLivedMovie : Movie
    {
        protected override Euros CalculatePriceCore()
        {
            return Euros.Of(8);
        }

        public override ExpirationDate GetExpirationDate()
        {
            return ExpirationDate.Infinite;
        }
    }
}
