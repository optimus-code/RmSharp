using RmSharp.Extensions;
using RmSharp.Tokens;
using System.IO;

namespace RmSharp.Converters.Tokens
{
    /// <summary>
    /// Converts a false token to a Boolean value and vice versa.
    /// </summary>
    public class RmFalseTokenConverter : RmTokenConverter
    {
        public RmFalseTokenConverter( ) : base( RubyMarshalToken.False )
        {
        }

        public override object Read( BinaryReader reader )
        {
            return false;
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            writer.Write( Token );
        }
    }
}