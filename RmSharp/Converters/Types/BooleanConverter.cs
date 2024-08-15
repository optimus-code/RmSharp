using RmSharp.Extensions;
using RmSharp.Tokens;
using System.IO;

namespace RmSharp.Converters.Types
{
    public class BooleanConverter : RmTypeConverter<bool>
    {
        public override object Read( BinaryReader reader )
        {
            var token = reader.ReadToken( RubyMarshalToken.True, RubyMarshalToken.False );

            if ( token is RubyMarshalToken.True )
                return true;
            
            return false;
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            bool value = ( bool ) instance;
            writer.Write( value ? RubyMarshalToken.True : RubyMarshalToken.False );
        }
    }
}