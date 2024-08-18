using RmSharp.Converters;
using RmSharp.Exceptions;
using System;
using System.IO;

namespace RmSharp
{
    public static class RmSerialiser
    {
        public static T Deserialise<T>( Stream stream )
        {
            RmConverterFactory.Reset( );

            using ( var reader = new RmReader( stream ) )
            {
                return reader.Deserialise<T>( );
            }
        }

        public static void Serialise<T>( Stream stream, T value )
        {
            RmConverterFactory.Reset( );

            using ( var writer = new RmWriter( stream ) )
            {
                writer.Serialise( value );
            }
        }
    }
}