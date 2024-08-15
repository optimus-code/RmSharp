using RmSharp.Tokens;
using System.IO;

namespace RmSharp.Converters.Tokens
{
    /// <summary>
    /// Converts a nil token to null and vice versa.
    /// </summary>
    public class RmNilTokenConverter : RmTokenConverter
    {
        public RmNilTokenConverter( ) : base( RubyMarshalToken.Nil )
        {
        }

        public override object Read( BinaryReader reader )
        {
            // Nothing to read
            return null;
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            writer.Write( ( byte ) Token );
        }
    }
}