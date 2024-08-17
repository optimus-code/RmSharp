using RmSharp.Extensions;
using System.IO;

namespace RmSharp.Converters.Types.Basic
{
    public class StringConverter : RmTypeConverter<string>
    {
        public override object Read( BinaryReader reader )
        {
            return reader.ReadRubyString( );
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            writer.WriteRubyString( ( string ) instance );
        }
    }
}