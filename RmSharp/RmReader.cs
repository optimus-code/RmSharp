using System.IO;
using System;
using RmSharp.Converters;
using RmSharp.Exceptions;

namespace RmSharp
{
    public class RmReader : IDisposable
    {
        const short HEADER_SIGNATURE = 0x0804;

        private readonly BinaryReader _reader;

        public RmReader( Stream stream )
        {
            _reader = new BinaryReader( stream );
        }

        public T Deserialise<T>( )
        {
            return ( T ) Deserialise( typeof( T ) );
        }

        public object Deserialise( Type type )
        {
            var header = ReadHeader( );

            if ( header != HEADER_SIGNATURE )
                throw new RmException( $"Invalid header signature, expected {HEADER_SIGNATURE:X4} but got {header:X4}" );

            var converter = RmConverterFactory.GetConverter( type );

            if ( converter == null )
                throw new RmException( $"No converter for type '{type.FullName}'." );

            return converter.Read( _reader );
        }

        private short ReadHeader( )
        {
            return _reader.ReadInt16( );
        }

        public void Dispose( )
        {
            _reader?.Dispose( );
        }
    }
}