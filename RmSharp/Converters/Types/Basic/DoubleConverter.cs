using RmSharp.Extensions;
using RmSharp.Tokens;
using System.IO;

namespace RmSharp.Converters.Types.Basic
{
    public class DoubleConverter : RmTypeConverter<double>
    {
        public override object Read( BinaryReader reader )
        {
            return reader.ReadDoubleCustom( );
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            writer.WriteDouble( ( double ) instance );
        }
    }
}