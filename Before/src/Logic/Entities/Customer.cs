using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Utils;
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

        public virtual CustomerStatus Status { get; protected set; }

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

        public virtual Result CanPromote()
        {
            if (this.Status.IsAdvanced)
            {
                return Result.Fail("The customer already has the Advanced status");
            }
            // at least 2 active movies during the last 30 days
            if (this.PurchasedMovies.Count(x => x.ExpirationDate == ExpirationDate.Infinite || x.ExpirationDate.Date >= DateTime.UtcNow.AddDays(-30)) < 2)
                return Result.Fail("Customer has not 2 active movies the last 30 days");

            // at least 100 dollars spent during the last year
            if (this.PurchasedMovies.Where(x => x.PurchaseDate > DateTime.UtcNow.AddYears(-1)).Sum(x => x.Price) < 100m)
                return Result.Fail("Customer has not spent at least 100 dollars during the last year");

            return Result.Ok();
        }

        public virtual void Promote()
        {
            if (CanPromote().IsFailure)
            {
                throw new Exception();
            }

            this.Status = this.Status.Promote();
        }

        public virtual void PurchaseMovie(Movie movie)
        {
            var price = movie.CalculatePrice(this.Status);
            this.AddPurchasedMovies(movie, movie.GetExpirationDate(), price);

        }
    }
}
