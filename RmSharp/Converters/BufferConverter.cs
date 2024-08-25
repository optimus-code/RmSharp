using RmSharp.Attributes;
using RmSharp.Exceptions;
using RmSharp.Extensions;
using RmSharp.Tokens;
using System;
using System.Collections.Generic;
using System.IO;

namespace RmSharp.Converters
{
    public class BufferConverter : RmConverter
    {
        private readonly SymbolConverter symbolConverter = RmConverterFactory.SymbolConverter;
        private readonly RmBufferAttribute _bufferAttribute;
        private readonly RmConverter _customConverter;

        public BufferConverter( RmBufferAttribute bufferAttribute )
        {
            _bufferAttribute = bufferAttribute;
            _customConverter = ( RmConverter ) Activator.CreateInstance( bufferAttribute.Converter );
        }

        public override object Read( BinaryReader reader )
        {
            var token = reader.ReadToken( RubyMarshalToken.UserDefined );

            if ( token != RubyMarshalToken.UserDefined )
                return null;

            var name = ( string ) symbolConverter.Read( reader );

            if ( name != _bufferAttribute.Name )
                throw new RmException( $"Mismatch in expected name for RmBufferAttribute, got '{name}' expected '{_bufferAttribute.Name}." );

            var length = reader.ReadFixNum<int>( );
            var buffer = reader.ReadBytes( length );

            using ( var ms = new MemoryStream( buffer ) )
            using ( var subReader = new BinaryReader( ms ) )
            {
                return _customConverter.Read( subReader );
            }
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            writer.Write( ( byte ) RubyMarshalToken.UserDefined );

            symbolConverter.Write( writer, _bufferAttribute.Name );

            using ( var ms = new MemoryStream( ) )
            using ( var subWriter = new BinaryWriter( ms ) )
            {
                _customConverter.Write( subWriter, instance );
                subWriter.Flush( );

                int length = ( int ) ms.Length;
                writer.WriteFixNum( length );

                writer.Write( ms.ToArray( ) );
            }
        }
    }
}
