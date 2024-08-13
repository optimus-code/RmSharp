using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using RmSharp.Enums;

namespace RmSharp.Converters.Types
{
    public class RmRegularExpressionConverter : RmConverter<Regex>
    {
        public override RubyControlToken[] Tokens => new[] { RubyControlToken.Regexp };

        public override object Read( BinaryReader reader )
        {
            int length = reader.ReadInt32( );
            string pattern = Encoding.UTF8.GetString( reader.ReadBytes( length ) );
            RegexOptions options = ( RegexOptions ) reader.ReadByte( );
            return new Regex( pattern, options );
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            Regex regex = ( Regex ) instance;
            byte[] bytes = Encoding.UTF8.GetBytes( regex.ToString( ) );
            writer.Write( bytes.Length );
            writer.Write( bytes );
            writer.Write( ( byte ) regex.Options );
        }
    }
}