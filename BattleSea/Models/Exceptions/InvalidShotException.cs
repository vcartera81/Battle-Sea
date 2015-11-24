using System;

namespace BattleSea.Models.Exceptions
{
    public class InvalidShotException : Exception
    {
        public InvalidShotException(string message) : base(message)
        {
        }
    }
}