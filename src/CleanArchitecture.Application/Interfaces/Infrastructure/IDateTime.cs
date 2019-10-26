namespace CleanArchitecture.Application.Interfaces.Infrastructure
{
    using System;

    public interface IDateTime
    {
        DateTime UtcNow { get; }
    }
}
