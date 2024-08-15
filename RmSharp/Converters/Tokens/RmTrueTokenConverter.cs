using RmSharp.Tokens;
using System.IO;

namespace RmSharp.Converters.Tokens
{
    /// <summary>
    /// Converts a true token to a Boolean value and vice versa.
    /// </summary>
    public class RmTrueTokenConverter : RmTokenConverter
    {
        public RmTrueTokenConverter( ) : base( RubyMarshalToken.True )
        {
        }

        public override object Read( BinaryReader reader )
        {
            return true;
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            writer.Write( ( byte ) Token );
        }
    }
}