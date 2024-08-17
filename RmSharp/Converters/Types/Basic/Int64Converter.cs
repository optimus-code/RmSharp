using RmSharp.Extensions;
using System.IO;

namespace RmSharp.Converters.Types.Basic
{
    public class Int64Converter : RmTypeConverter<long>
    {
        public override object Read( BinaryReader reader )
        {
            return reader.ReadNumeric<long>( );
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            writer.WriteNumeric( instance );
        }
    }
}