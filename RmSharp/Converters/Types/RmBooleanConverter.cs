using System.IO;
using RmSharp.Enums;

namespace RmSharp.Converters.Types
{
    public class BooleanConverter : RmConverter<bool>
    {
        public override RubyControlToken[] Tokens => new[] { RubyControlToken.True, RubyControlToken.False };

        public override object Read( BinaryReader reader )
        {
            // Assume the token has already been read, so this is just to return the value
            return reader.BaseStream.Position == RubyControlToken.True ? true : false;
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            bool value = ( bool ) instance;
            writer.Write( value ? ( byte ) RubyControlToken.True : ( byte ) RubyControlToken.False );
        }
    }
}