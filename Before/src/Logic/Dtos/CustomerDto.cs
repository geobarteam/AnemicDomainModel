using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Dtos
{
    public class CustomerDto 
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
        
        public string Status { get; set; }

        public virtual DateTime? StatusExpirationDate { get; set; }

        public virtual decimal MoneySpent { get; set; }

        public virtual IList<PurchasedMovieDto> PurchasedMovies { get; set; }
    }
}
