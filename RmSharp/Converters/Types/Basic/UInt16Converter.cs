using RmSharp.Extensions;
using System.IO;

namespace RmSharp.Converters.Types.Basic
{
    public class UInt16Converter : RmTypeConverter<ushort>
    {
        public override object Read( BinaryReader reader )
        {
            return reader.ReadNumeric<ushort>( );
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            writer.WriteNumeric( instance );
        }
    }
}