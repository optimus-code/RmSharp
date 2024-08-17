using RmSharp.Extensions;
using RmSharp.Tokens;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace RmSharp.Converters.Types.Basic
{
    public class RegexConverter : RmTypeConverter<Regex>
    {
        public override object Read( BinaryReader reader )
        {
            return reader.ReadRegex( );
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            writer.WriteRegex( ( Regex ) instance );
        }
    }
}