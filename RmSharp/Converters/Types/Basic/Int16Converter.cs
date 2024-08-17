using RmSharp.Extensions;
using System.IO;

namespace RmSharp.Converters.Types.Basic
{
    public class Int16Converter : RmTypeConverter<short>
    {
        public override object Read( BinaryReader reader )
        {
            return reader.ReadNumeric<short>( );
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            writer.WriteNumeric( instance );
        }
    }
}