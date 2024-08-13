using System;

namespace RmSharp.Exceptions
{
    public class RmException : Exception
    {
        public RmException( string errorMessage = null ) : base( errorMessage )
        { 
        }
    }
}