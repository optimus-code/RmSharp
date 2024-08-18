using System;
using System.IO;
using RmSharp.Converters;
using RmSharp.Exceptions;

namespace RmSharp
{
    public class RmWriter : IDisposable
    {
        const short HEADER_SIGNATURE = 0x0804;

        private readonly BinaryWriter _writer;

        public RmWriter( Stream stream )
        {
            _writer = new BinaryWriter( stream );
        }

        public void Serialise<T>( T obj )
        {
            Serialise( obj, typeof( T ) );
        }

        public void Serialise( object obj, Type type )
        {
            WriteHeader( );

            var converter = RmConverterFactory.GetConverter( type );

            if ( converter == null )
                throw new RmException( $"No converter for type '{type.FullName}'." );

            converter.Write( _writer, obj );
        }

        private void WriteHeader( )
        {
            _writer.Write( HEADER_SIGNATURE );
        }

        public void Dispose( )
        {
            _writer?.Dispose( );
        }
    }
}