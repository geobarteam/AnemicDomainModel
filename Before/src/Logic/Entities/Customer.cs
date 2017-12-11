using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Remoting.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
            set => _email = value;
        }

        public virtual CustomerStatus Status { get; set; }

        private DateTime? _statusExpirationDate;

        public virtual ExpirationDate StatusExpirationDate
        {
            get => (ExpirationDate)this._statusExpirationDate;
            set => this._statusExpirationDate = value;
        }

        private decimal _moneySpent;

        public virtual Euros MoneySpent
        {
            get => (Euros)this._moneySpent;
            set => this._moneySpent = value;
        }

        public virtual IList<PurchasedMovie> PurchasedMovies { get; set; }
    }
}
