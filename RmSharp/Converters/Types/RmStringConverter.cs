using System.IO;
using System.Text;
using RmSharp.Enums;

namespace RmSharp.Converters.Types
{
    public class RmStringConverter : RmConverter<string>
    {
        public override RubyControlToken[] Tokens => [RubyControlToken.String];

        public override object Read( BinaryReader reader )
        {
            int length = reader.ReadInt32( );
            return Encoding.UTF8.GetString( reader.ReadBytes( length ) );
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            string value = ( string ) instance;
            byte[] bytes = Encoding.UTF8.GetBytes( value );
            writer.Write( bytes.Length );
            writer.Write( bytes );
        }
    }
}