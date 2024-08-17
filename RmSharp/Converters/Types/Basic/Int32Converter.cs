using RmSharp.Extensions;
using System.IO;

namespace RmSharp.Converters.Types.Basic
{
    public class Int32Converter : RmTypeConverter<int>
    {
        public override object Read( BinaryReader reader )
        {
            return reader.ReadNumeric<int>( );
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            writer.WriteNumeric( instance );
        }
    }
}