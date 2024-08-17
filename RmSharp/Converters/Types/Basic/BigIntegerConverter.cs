using RmSharp.Extensions;
using System.IO;
using System.Numerics;

namespace RmSharp.Converters.Types.Basic
{
    public class BigIntegerConverter : RmTypeConverter<BigInteger>
    {
        public override object Read( BinaryReader reader )
        {
            return reader.ReadNumeric<BigInteger>( );
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            writer.WriteNumeric( instance );
        }
    }
}