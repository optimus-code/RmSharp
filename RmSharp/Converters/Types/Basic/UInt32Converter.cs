using RmSharp.Extensions;
using System.IO;

namespace RmSharp.Converters.Types.Basic
{
    public class UInt32Converter : RmTypeConverter<uint>
    {
        public override object Read( BinaryReader reader )
        {
            return reader.ReadNumeric<uint>( );
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            writer.WriteNumeric( instance );
        }
    }
}