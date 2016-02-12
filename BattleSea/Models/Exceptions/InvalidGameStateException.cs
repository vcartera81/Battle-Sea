using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BattleSea.Models.Exceptions
{
    public class InvalidGameStateException : Exception
    {
        public InvalidGameStateException(string message) : base(message)
        {
            
        }
    }
}