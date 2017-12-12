using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api
{
    public class Envelope<T>
    {
        public T Result { get; }
        public string ErrorMessage { get; }
        public DateTime TimeGenerated { get; }

        protected internal Envelope(T result, string errorMessage)
        {
            this.Result = result;
            this.ErrorMessage = errorMessage;
            this.TimeGenerated = DateTime.UtcNow; 
        }
    }

    public class Envelope : Envelope<string>
    {
        protected Envelope(string errorMessage)
            : base(null, errorMessage)
        {
            
        }

        public static Envelope Ok()
        {
            return new Envelope(null);
        }

        public static Envelope<T> Ok<T>(T result)
        {
            return new Envelope<T>(result, null);
        }

        public static Envelope Error(string errorMessage)
        {
            return new Envelope(errorMessage);
        }
    }
}
