using RmSharp.Tokens;

namespace RmSharp.Exceptions
{
    public class RmInvalidTokenException : RmException
    {
        public RmInvalidTokenException( RubyMarshalToken got, params RubyMarshalToken[] expected )
            : base( $"Invalid token processed expected '{string.Join( ",", expected)} got '{got}'." )
        {
        }
    }
}