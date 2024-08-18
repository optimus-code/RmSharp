using RmSharp.Extensions;
using RmSharp.Tokens;
using System;
using System.IO;

namespace RmSharp.Converters
{
    public class DynamicConverter : RmConverter
    {
        public override object Read( BinaryReader reader )
        {
            // Peek the byte to find out a token to process
            if ( reader.PeekByte( out var byteValue ) )
            {
                if ( Enum.IsDefined( typeof( RubyMarshalToken ), byteValue ) )
                {
                    var tokenConverter = RmConverterFactory.GetConverter( ( RubyMarshalToken ) byteValue );

                    if ( tokenConverter != null )
                    {
                        return tokenConverter.Read( reader );
                    }
                }
            }

            return null;
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            var converter = RmConverterFactory.GetConverter( instance.GetType() );

            if ( converter != null )
            {
                converter.Write( writer, instance );
            }
        }
    }
}
