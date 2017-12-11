using System;
using System.Linq;
using Logic.Entities;

namespace Logic.Services
{
    public class CustomerService
    {
        private readonly MovieService _movieService;

        public CustomerService(MovieService movieService)
        {
            _movieService = movieService;
        }

        private Euros CalculatePrice(CustomerStatus status, ExpirationDate statusExpirationDate, LicensingModel licensingModel)
        {
            Euros price;
            switch (licensingModel)
            {
                case LicensingModel.TwoDays:
                    price = Euros.Of(4);
                    break;

                case LicensingModel.LifeLong:
                    price = Euros.Of(8);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (status == CustomerStatus.Advanced && !statusExpirationDate.IsExpired)
            {
                price = price * 0.75m;
            }

            return price;
        }

        public void PurchaseMovie(Customer customer, Movie movie)
        {
            var expirationDate = _movieService.GetExpirationDate(movie.LicensingModel);
            decimal price = CalculatePrice(customer.Status, customer.StatusExpirationDate, movie.LicensingModel);

            var purchasedMovie = new PurchasedMovie
            {
                MovieId = movie.Id,
                CustomerId = customer.Id,
                ExpirationDate = expirationDate,
                Price = Euros.Of(price),
                PurchaseDate = DateTime.UtcNow
            };

            customer.PurchasedMovies.Add(purchasedMovie);
            customer.MoneySpent += Euros.Of(price);
        }

        public bool PromoteCustomer(Customer customer)
        {
            // at least 2 active movies during the last 30 days
            if (customer.PurchasedMovies.Count(x => x.ExpirationDate == ExpirationDate.Infinite || x.ExpirationDate.Date >= DateTime.UtcNow.AddDays(-30)) < 2)
                return false;

            // at least 100 dollars spent during the last year
            if (customer.PurchasedMovies.Where(x => x.PurchaseDate > DateTime.UtcNow.AddYears(-1)).Sum(x => x.Price) < 100m)
                return false;

            customer.Status = CustomerStatus.Advanced;
            customer.StatusExpirationDate = (ExpirationDate)DateTime.UtcNow.AddYears(1);

            return true;
        }
    }
}
