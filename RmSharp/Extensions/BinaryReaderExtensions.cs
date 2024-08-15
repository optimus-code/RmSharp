using RmSharp.Exceptions;
using RmSharp.Tokens;
using System;
using System.IO;
using System.Linq;

namespace RmSharp.Extensions
{
    public static class BinaryReaderExtensions
    {
        /// <summary>
        /// Read a token from the stream
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        /// <exception cref="RmInvalidTokenException"></exception>
        public static RubyMarshalToken ReadToken( this BinaryReader reader, params RubyMarshalToken[] expected )
        {
            var token = ( RubyMarshalToken ) reader.ReadByte( );

            if ( expected?.Any( ) == true )
            {
                if ( !expected.ToHashSet( ).Contains( token ) )
                    throw new RmInvalidTokenException( token, expected );
            }

            return token;
        }

        /// <summary>
        /// Read a Ruby FixNum of the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="br"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        public static T ReadFixNum<T>( this BinaryReader br )
        {
            return ( T ) ReadFixNum( br, typeof( T ) );
        }

        /// <summary>
        /// Read a Ruby FixNum of the specified type
        /// </summary>
        /// <param name="br"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        public static object ReadFixNum( this BinaryReader br, Type type )
        {
            object value = Activator.CreateInstance( type );
            long internalValue;

            var b = br.ReadByte( );

            switch ( b )
            {
                case 0:
                    internalValue = 0;
                    break;

                case 1:
                    internalValue = br.ReadByte( );
                    break;

                case 0xff:
                    internalValue = -br.ReadByte( );
                    break;

                case 2:
                    internalValue = br.ReadUInt16( );
                    break;

                case 0xfe:
                    internalValue = -br.ReadUInt16( );
                    break;

                case 3:
                    {
                        byte[] buffer = new byte[4];
                        br.Read( buffer, 0, 3 );
                        internalValue = BitConverter.ToUInt32( buffer );
                        break;
                    }

                case 0xfd:
                    {
                        byte[] buffer = new byte[4];
                        br.Read( buffer, 0, 3 );
                        internalValue = -BitConverter.ToUInt32( buffer );
                        break;
                    }
                case 4:
                    internalValue = br.ReadUInt32( );
                    break;

                case 0xfc:
                    internalValue = -br.ReadUInt32( );
                    break;

                default:
                    {
                        var temp = ( sbyte ) b;
                        if ( temp > 0 )
                            internalValue = temp - 5;
                        else
                            internalValue = temp + 5;
                        break;
                    }
            }

            if ( value is int )
            {
                if ( internalValue > int.MaxValue || internalValue < int.MinValue )
                    throw new InvalidCastException( );

                value = ( int ) internalValue;
            }
            else if ( value is uint )
            {
                if ( internalValue < 0 || internalValue > uint.MaxValue )
                    throw new InvalidCastException( );

                value = ( uint ) internalValue;
            }
            else if ( value is ushort )
            {
                if ( internalValue < 0 || internalValue > ushort.MaxValue )
                    throw new InvalidCastException( );

                value = ( ushort ) internalValue;
            }
            else if ( value is short )
            {
                if ( internalValue > short.MaxValue || internalValue < short.MinValue )
                    throw new InvalidCastException( );

                value = ( short ) internalValue;
            }
            else
            {
                throw new InvalidCastException( "Unsupported type" );
            }

            return value;
        }
    }
}