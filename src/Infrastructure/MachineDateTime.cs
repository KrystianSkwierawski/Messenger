using Domain.Interfaces;
using System;

namespace Infrastructure
{
    public class MachineDateTime : IDateTime
    {
        public string Now => DateTime.UtcNow.ToString();
    }
}
