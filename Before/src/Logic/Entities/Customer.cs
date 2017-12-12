using System;
using System.Collections.Generic;
using System.Linq;
using Remotion.Linq.Utilities;

namespace Logic.Entities
{
    public class Customer : Entity
    {
        private string _name;
        public virtual CustomerName Name
        {
            get => (CustomerName)this._name;
            set => _name = value;
        }

        private string _email;

        public virtual Email Email
        {
            get => (Email)this._email;
            protected set => _email = value;
        }

        public virtual CustomerStatus Status { get; set; }

        private decimal _moneySpent;

        public virtual Euros MoneySpent
        {
            get => (Euros)this._moneySpent;
            protected set => this._moneySpent = value;
        }

        protected Customer()
        {
            this._purchasedMovies = new List<PurchasedMovie>();    
        }
        
        public Customer(CustomerName name, Email email):this()
        {
            this._name = name ?? throw new ArgumentEmptyException(nameof(name));
            this._email = email ?? throw new ArgumentEmptyException(nameof(email));

            this.MoneySpent = Euros.Of(0);
            this.Status = CustomerStatus.Regular;
        }


        private IList<PurchasedMovie> _purchasedMovies;

        public virtual IReadOnlyList<PurchasedMovie> PurchasedMovies
        {
            get => this._purchasedMovies.ToList();
        }

        public virtual void AddPurchasedMovies(Movie movie, ExpirationDate expirationDate, Euros price)
        {
            this._moneySpent += Euros.Of(price);
            var purchasedMovie = new PurchasedMovie(this, movie, price, expirationDate);
            this._purchasedMovies.Add(purchasedMovie);
        }
    }
}
