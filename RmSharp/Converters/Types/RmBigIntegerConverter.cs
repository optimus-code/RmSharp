using System.IO;
using System.Numerics;
using RmSharp.Enums;

namespace RmSharp.Converters.Types
{
    public class RmBigIntegerConverter : RmConverter<BigInteger>
    {
        public override RubyControlToken[] Tokens => new[] { RubyControlToken.Bignum };

        public override object Read( BinaryReader reader )
        {
            int sign = reader.ReadByte( );
            int length = reader.ReadInt32( );
            BigInteger value = new BigInteger( reader.ReadBytes( length ) );
            return sign == '-' ? -value : value;
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            BigInteger value = ( BigInteger ) instance;
            byte[] bytes = value.ToByteArray( );
            writer.Write( value.Sign < 0 ? ( byte ) '-' : ( byte ) '+' );
            writer.Write( bytes.Length );
            writer.Write( bytes );
        }
    }
}