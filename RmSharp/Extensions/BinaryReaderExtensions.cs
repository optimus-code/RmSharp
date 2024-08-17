using RmSharp.Exceptions;
using RmSharp.Tokens;
using RmSharp.Types;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;

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
        /// Read a value from the stream, handling Nils.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        /// <exception cref="RmInvalidTokenException"></exception>
        public static T ReadValue<T>( this BinaryReader reader, Func<RubyMarshalToken, T> onRead, params RubyMarshalToken[] expected )
        {
            if ( onRead == null )
                throw new RmException( "No onRead callback specified" );

            return ( T ) ReadValueRaw( reader, ( token ) =>
            {
                return ( object ) onRead.Invoke( token );
            }, expected );
        }

        /// <summary>
        /// Read a value from the stream, handling Nil tokens.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="onRead"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        /// <exception cref="RmInvalidTokenException"></exception>
        public static object ReadValueRaw( this BinaryReader reader, Func<RubyMarshalToken, object> onRead, params RubyMarshalToken[] expected )
        {
            if ( onRead == null )
                throw new RmException( "No onRead callback specified" );

            // Append the Nil token to the expected tokens
            var token = reader.ReadToken( [..expected, RubyMarshalToken.Nil] );

            if ( token == RubyMarshalToken.Nil )
            {
                // Return null for reference types, or the default value for value types
                return GetDefaultForType( onRead.Method?.ReturnType );
            }

            return onRead.Invoke( token );
        }

        /// <summary>
        /// Get the default value for a given type.
        /// </summary>
        /// <param name="type">The type for which to get the default value.</param>
        /// <returns>The default value for the type.</returns>
        private static object GetDefaultForType( Type type )
        {
            if ( type == null )
                return null;

            // Handle value types by creating an instance of the type, which gives the default value
            if ( type.IsValueType )
            {
                return Activator.CreateInstance( type );
            }

            // Return null for reference types
            return null;
        }

        /// <summary>
        /// Read a numeric value from the stream, determining whether to use ReadFixNum or ReadBigNum based on the token.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="br"></param>
        /// <returns></returns>
        public static T ReadNumeric<T>( this BinaryReader br )
        {
            return ( T ) ReadNumeric( br, typeof( T ) );
        }

        /// <summary>
        /// Read a numeric value from the stream, determining whether to use ReadFixNum or ReadBigNum based on the token.
        /// </summary>
        /// <param name="br"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ReadNumeric( this BinaryReader br, Type type )
        {
            return br.ReadValue( ( token ) =>
            {
                switch ( token )
                {
                    case RubyMarshalToken.Fixnum:
                        return br.ReadFixNum( type );

                    case RubyMarshalToken.Bignum:
                        return br.ReadBigNum( type );

                    default:
                        throw new RmInvalidTokenException( token, RubyMarshalToken.Fixnum, RubyMarshalToken.Bignum );
                }
            }, RubyMarshalToken.Fixnum, RubyMarshalToken.Bignum );
        }

        /// <summary>
        /// Read a Ruby FixNum of the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="br"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        private static T ReadFixNum<T>( this BinaryReader br )
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
        private static object ReadFixNum( this BinaryReader br, Type type )
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

            if ( value is long )
            {
                if ( internalValue > long.MaxValue || internalValue < long.MinValue )
                    throw new InvalidCastException( );

                value = ( long ) internalValue;
            }
            else if ( value is ulong )
            {
                if ( ( ulong ) internalValue > ulong.MaxValue || ( ulong ) internalValue < ulong.MinValue )
                    throw new InvalidCastException( );

                value = ( ulong ) internalValue;
            }
            else if ( value is int )
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
            else if ( value is byte )
            {
                if ( internalValue > byte.MaxValue || internalValue < byte.MinValue )
                    throw new InvalidCastException( );

                value = ( byte ) internalValue;
            }
            else
            {
                throw new InvalidCastException( "Unsupported type" );
            }

            return value;
        }

        /// <summary>
        /// Read a Ruby BigNum or large integer, returning the appropriate type.
        /// </summary>
        /// <param name="br"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        private static object ReadBigNum( this BinaryReader br, Type type )
        {
            // Read the sign byte
            var sign = br.ReadByte( );

            // Read the number of 16-bit "shorts"
            var numShorts = br.ReadFixNum<int>( );

            // Prepare a list to collect the bytes
            var byteList = new List<byte>( );

            for ( int i = 0; i < numShorts; i++ )
            {
                // Read each short (2 bytes)
                byteList.Add( br.ReadByte( ) ); // First byte of the short

                if ( br.BaseStream.Position + 1 > br.BaseStream.Length )
                    break;

                if ( i == numShorts - 1 && ( byteList.Count % 2 == 1 ) )
                {
                    // If it's the last item and doesn't complete a short, peek to check if the next byte is a RubyMarshalToken
                    byte nextByte = ( byte ) br.PeekChar( );

                    // RubyMarshalToken values are typically small (e.g., Fixnum, String, Array, etc.), 
                    // so if nextByte matches a known RubyMarshalToken, stop reading.
                    if ( Enum.IsDefined( ( RubyMarshalToken ) nextByte ) )
                    {
                        break;
                    }
                }

                // If it isn't the end or if the next byte doesn't match a token, read the second byte
                byteList.Add( br.ReadByte( ) );
            }

            // Convert the list to an array
            byte[] magnitudeBytes = byteList.ToArray( );

            // Create the BigInteger using the little-endian byte array
            BigInteger temp = new BigInteger( magnitudeBytes );

            // Apply the sign
            if ( sign == ( byte ) '+' )
            {
                return type == typeof( BigInteger ) ? ( object ) temp : ( object ) ( ulong ) temp;
            }
            else if ( sign == ( byte ) '-' )
            {
                temp = -temp;
                return type == typeof( BigInteger ) ? ( object ) temp : throw new InvalidCastException( "Cannot convert negative Bignum to ulong." );
            }

            throw new NotSupportedException( "Invalid sign byte in Ruby Bignum." );
        }






        /// <summary>
        /// Read a Ruby BigNum of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="br"></param>
        /// <returns></returns>
        private static T ReadBigNum<T>( this BinaryReader br )
        {
            return ( T ) ReadBigNum( br, typeof( T ) );
        }

        /// <summary>
        /// Read a double stored in the Ruby Marshal format
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        public static double ReadDoubleCustom( this BinaryReader br )
        {
            return br.ReadValue( ( token ) =>
            {
                // Read the length of the string (stored as a Fixnum)
                int length = br.ReadFixNum<int>( );

                // Read the string bytes
                byte[] stringBytes = br.ReadBytes( length );

                // Convert the string bytes to a string
                var floatString = Encoding.ASCII.GetString( stringBytes );

                int index = floatString.IndexOf( '\0' );
                floatString = index == -1 ? floatString : floatString[0..index];

                return double.Parse( floatString, CultureInfo.InvariantCulture );
            }, RubyMarshalToken.Double );
        }

        public static string ReadRubyString( this BinaryReader br )
        {
            return br.ReadValue( ( token ) =>
            {
                if ( token == RubyMarshalToken.Nil )
                {
                    return null;
                }

                var length = br.ReadFixNum<int>( );
                return Encoding.UTF8.GetString( br.ReadBytes( length ) );
            }, RubyMarshalToken.String );
        }

        public static System.Text.RegularExpressions.Regex ReadRegex( this BinaryReader br )
        {
            return br.ReadValue( ( token ) =>
            {
                if ( token != RubyMarshalToken.RegularExpression )
                    return null;

                var strlen = br.ReadFixNum<int>( );
                var pattern = Encoding.UTF8.GetString( br.ReadBytes( strlen ) );

                var options = RegexOptions.None;
                int b = br.ReadByte( );

                if ( ( b & 1 ) != 0 )
                    options |= RegexOptions.IgnoreCase;

                if ( ( b & 2 ) != 0 )
                    options |= RegexOptions.IgnorePatternWhitespace;

                if ( ( b & 4 ) != 0 )
                    options |= RegexOptions.Multiline;

                return new Regex( pattern, options );
            }, RubyMarshalToken.RegularExpression );
        }
    }
}