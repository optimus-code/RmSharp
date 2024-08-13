using System.IO;
using System;

namespace RmSharp
{
    public class RmReader : IDisposable
    {
        private readonly BinaryReader _reader;

        public RmReader( Stream stream )
        {
            _reader = new BinaryReader( stream );
        }

        public T Deserialise<T>( )
        {
            return ( T ) Deserialise( typeof( T ) );
        }

        private object Deserialise( Type type )
        {
            var classReader = new LcfClassConverter( type );
            return classReader.Read( _reader, null );
        }

        public short ReadHeader( )
        {
            return _reader.ReadInt16( );
        }

        public void Dispose( )
        {
            _reader?.Dispose( );
        }
    }
}