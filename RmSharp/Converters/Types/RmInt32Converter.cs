using System.IO;
using RmSharp.Enums;

namespace RmSharp.Converters.Types
{
    public class RmInt32Converter : RmConverter<int>
    {
        public override RubyControlToken[] Tokens => new[] { RubyControlToken.Fixnum };

        public override object Read( BinaryReader reader )
        {
            return reader.ReadInt32( );
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            int value = ( int ) instance;
            writer.Write( value );
        }
    }
}