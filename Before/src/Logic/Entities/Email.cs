using System;
using Logic.Utils;

namespace Logic.Entities
{
    public class Email : ValueObject<Email>
    {
        public string Value { get; }

        protected Email(string value)
        {
            Value = value;
        }

        public static Result<Email> Create(string email)
        {
            email = (email ?? string.Empty).Trim();

            if (email.Length == 0)
            {
                return Result.Fail<Email>("Email should not be empty");
            }

            if (email.Length > 100)
            {
                return Result.Fail<Email>("Email is too long");
            }

            return Result.Ok(new Email(email));
        }

        protected override bool EqualsCore(Email other)
        {
            return Value.Equals(other.Value, StringComparison.CurrentCultureIgnoreCase);
        }

        protected override int GetHashCodeCore()
        {
            return Value.GetHashCode();
        }
    }
}