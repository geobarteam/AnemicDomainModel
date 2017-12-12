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

        private Euros CalculatePrice(CustomerStatus customerStatus, LicensingModel licensingModel)
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

            if (customerStatus.IsAdvanced)
            {
                price = price * 0.75m;
            }

            return Euros.Of(price);
        }

        public void PurchaseMovie(Customer customer, Movie movie)
        {
            var expirationDate = _movieService.GetExpirationDate(movie.LicensingModel);
            var price = CalculatePrice(customer.Status, movie.LicensingModel);
            
            customer.AddPurchasedMovies(movie, expirationDate, price);
            
        }


        public bool PromoteCustomer(Customer customer)
        {
            // at least 2 active movies during the last 30 days
            if (customer.PurchasedMovies.Count(x => x.ExpirationDate == ExpirationDate.Infinite || x.ExpirationDate.Date >= DateTime.UtcNow.AddDays(-30)) < 2)
                return false;

            // at least 100 dollars spent during the last year
            if (customer.PurchasedMovies.Where(x => x.PurchaseDate > DateTime.UtcNow.AddYears(-1)).Sum(x => x.Price) < 100m)
                return false;

            customer.Status = customer.Status.Promote();

            return true;
        }
    }
}
