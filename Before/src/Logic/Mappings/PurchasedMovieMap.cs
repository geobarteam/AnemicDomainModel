using System;
using FluentNHibernate.Mapping;
using Logic.Entities;

namespace Logic.Mappings
{
    public class PurchasedMovieMap : ClassMap<PurchasedMovie>
    {
        public PurchasedMovieMap()
        {
            Id(x => x.Id);

            Map(x => x.Price).CustomType<DateTime?>().Access.CamelCaseField(Prefix.Underscore); 
            Map(x => x.PurchaseDate);
            Map(x => x.ExpirationDate).CustomType<DateTime?>().Access.CamelCaseField(Prefix.Underscore);
          
            References(x => x.Movie);
            References(x => x.Customer);
        }
    }
}
