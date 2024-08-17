using RmSharp.Extensions;
using RmSharp.Tokens;
using System.IO;

namespace RmSharp.Converters.Types.Basic
{
    public class BooleanConverter : RmTypeConverter<bool>
    {
        public override object Read( BinaryReader reader )
        {
            return reader.ReadValue( ( token ) =>
            {
                if ( token is RubyMarshalToken.True )
                    return true;

                return false;
            }, RubyMarshalToken.True, RubyMarshalToken.False );
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            bool value = ( bool ) instance;
            writer.Write( value ? RubyMarshalToken.True : RubyMarshalToken.False );
        }
    }
}