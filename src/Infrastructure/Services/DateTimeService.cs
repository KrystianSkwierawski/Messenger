using Domain.Interfaces;
using System;

namespace Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public string Now => DateTime.UtcNow.ToString();
    }
}
