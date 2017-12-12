using System;
using Newtonsoft.Json;
using Remotion.Linq.Utilities;

namespace Logic.Entities
{
    public class PurchasedMovie : Entity
    {

        public virtual Movie Movie { get; protected set; }

        public virtual Customer Customer { get; protected set; }

        private decimal _price;

        public virtual Euros Price
        {
            get => (Euros)this._price;
            protected set => this._price = value;
        }

        public virtual DateTime PurchaseDate { get; protected set; }

        private DateTime? _expirationDate;

        public virtual ExpirationDate ExpirationDate
        {
            get => (ExpirationDate)this._expirationDate;
            protected set => this._expirationDate = value;
        }

        protected PurchasedMovie()
        {
            
        }

        internal PurchasedMovie(Customer customer, Movie movie, Euros price, ExpirationDate expirationDate) 
            : this()
        {
            if (price == null || price.IsZero)
            {
                throw new ArgumentException(nameof(price));
            }

            if (expirationDate == null || expirationDate.IsExpired)
            {
                throw new ArgumentException(nameof(expirationDate));
            }

            this.Customer = customer ?? throw new ArgumentNullException(nameof(customer));
            this.Movie = movie ?? throw new ArgumentNullException(nameof(movie));
            this.Price = price;
            this.ExpirationDate = expirationDate;
            this.PurchaseDate = DateTime.UtcNow;

        }
    }
}
