namespace CleanArchitecture.Domain.SeedWork
{
    using System;

    public class DomainException : Exception
    {
        public DomainException(string message)
            : base(message)
        {
        }

        public DomainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public DomainException(Exception innerException)
            : base(string.Empty, innerException)
        {
        }
    }
}
