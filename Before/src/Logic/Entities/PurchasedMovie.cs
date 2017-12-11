using System;
using Newtonsoft.Json;

namespace Logic.Entities
{
    public class PurchasedMovie : Entity
    {
        public virtual long MovieId { get; set; }

        public virtual Movie Movie { get; set; }

        public virtual long CustomerId { get; set; }

        private decimal _price;

        public virtual Euros Price
        {
            get => (Euros)this._price;
            set => this._price = value;
        }

        public virtual DateTime PurchaseDate { get; set; }

        private DateTime? _expirationDate;

        public virtual ExpirationDate ExpirationDate
        {
            get => (ExpirationDate)this._expirationDate;
            set => this._expirationDate = value;
        }
    }
}
