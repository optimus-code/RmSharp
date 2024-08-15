using RmSharp.Extensions;
using System.IO;

namespace RmSharp.Converters.Types
{
    public class Int32Converter : RmTypeConverter<int>
    {
        public override object Read( BinaryReader reader )
        {
            return reader.ReadFixNum<int>( );
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            writer.WriteFixNum( instance );
        }
    }
}