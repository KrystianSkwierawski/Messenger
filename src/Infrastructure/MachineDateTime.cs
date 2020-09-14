using Domain.Interfaces;
using System;

namespace Infrastructure
{
    public class MachineDateTime : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
