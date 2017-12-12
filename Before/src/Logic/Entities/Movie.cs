using System;
using Newtonsoft.Json;

namespace Logic.Entities
{
    public class Movie : Entity
    {
        public virtual string Name { get; protected set; }

        public virtual LicensingModel LicensingModel { get; protected set; }

        protected Movie() { }

        public Movie(string name, LicensingModel licensingModel)
            : this()
        {
            this.Name = name;
            this.LicensingModel = licensingModel;
        }

        public virtual Euros CalculatePrice(CustomerStatus customerStatus)
        {
            var modifier = 1 - customerStatus.GetDiscount();
            switch (this.LicensingModel)
            {
                case LicensingModel.TwoDays:
                    return Euros.Of(4) * modifier;

                case LicensingModel.LifeLong:
                    return Euros.Of(8) * modifier;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public virtual ExpirationDate GetExpirationDate()
        {

            ExpirationDate result;

            switch (this.LicensingModel)
            {
                case LicensingModel.TwoDays:
                    result = (ExpirationDate)DateTime.UtcNow.AddDays(2);
                    break;

                case LicensingModel.LifeLong:
                    result = ExpirationDate.Infinite;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;

        }
    }
}
