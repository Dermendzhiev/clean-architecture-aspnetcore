namespace CleanArchitecture.Infrastructure
{
    using System;
    using CleanArchitecture.Application.Interfaces.Infrastructure;

    public class MachineDateTime : IDateTime
    {
        public DateTime UtcNow => DateTime.Now;
    }
}
