using RmSharp.Exceptions;
using System;
using System.IO;

namespace RmSharp
{
    public static class RmSerialiser
    {
        const short HEADER_SIGNATURE = 0x0804;

        public static T Deserialise<T>( Stream stream )
        {
            using ( var reader = new RmReader( stream ) )
            {
                var header = reader.ReadHeader( );

                if ( header != HEADER_SIGNATURE )
                    throw new RmException( $"Invalid header signature, expected {HEADER_SIGNATURE:X4} but got {header:X4}" );

                return reader.Deserialise<T>( );
            }
        }

        public static void Serialise<T>( Stream stream, T value )
        {
            throw new NotImplementedException( );
        }
    }
}