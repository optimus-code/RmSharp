using RmSharp.Extensions;
using System.IO;

namespace RmSharp.Converters.Types.Basic
{
    public class UInt64Converter : RmTypeConverter<ulong>
    {
        public override object Read( BinaryReader reader )
        {
            return reader.ReadNumeric<ulong>( );
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            writer.WriteNumeric( instance );
        }
    }
}