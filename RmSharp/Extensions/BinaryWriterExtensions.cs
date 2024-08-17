using RmSharp.Tokens;
using System;
using System.IO;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace RmSharp.Extensions
{
    public static class BinaryWriterExtensions
    {
        /// <summary>
        /// Write a token to the stream.
        /// </summary>
        /// <param name="writer"></param>
        public static void Write( this BinaryWriter writer, RubyMarshalToken token )
        {
            writer.Write( ( byte ) token );
        }

        /// <summary>
        /// Write a value to the stream, handling Nil tokens.
        /// </summary>
        /// <param name="bw"></param>
        /// <param name="value"></param>
        /// <param name="token"></param>
        /// <param name="onWrite"></param>
        public static void WriteValue( this BinaryWriter bw, object value, RubyMarshalToken token, Action onWrite )
        {
            if ( value == null )
            {
                bw.Write( RubyMarshalToken.Nil );
            }
            else
            {
                bw.Write( token );
                onWrite?.Invoke( );
            }
        }

        /// <summary>
        /// Determines if a type is nullable.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsNullableType( Type type )
        {
            return !type.IsValueType || Nullable.GetUnderlyingType( type ) != null;
        }

        /// <summary>
        /// Write a numeric value to the stream, determining whether to use WriteFixNum or WriteBigNum based on the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bw"></param>
        /// <param name="value"></param>
        public static void WriteNumeric<T>( this BinaryWriter bw, T value )
        {
            RubyMarshalToken token = RubyMarshalToken.Fixnum;

            if ( value is BigInteger bigIntValue )
            {
                token = RubyMarshalToken.Bignum;
            }
            else if ( value is long longValue && ( longValue > int.MaxValue || longValue < int.MinValue ) )
            {
                token = RubyMarshalToken.Bignum;
            }

            bw.WriteValue( value, token, ( ) =>
            {
                if ( value is BigInteger bigIntValue )
                {
                    bw.WriteBigNum( bigIntValue );
                }
                else if ( value is long longValue && ( longValue > int.MaxValue || longValue < int.MinValue ) )
                {
                    bw.WriteBigNum( new BigInteger( longValue ) );
                }
                else
                {
                    bw.WriteFixNum( value );
                }
            } );
        }

        /// <summary>
        /// Write a numeric value to the stream, determining whether to use WriteFixNum or WriteBigNum based on the value.
        /// </summary>
        /// <param name="bw"></param>
        /// <param name="value"></param>
        public static void WriteNumeric( this BinaryWriter bw, object value )
        {
            RubyMarshalToken token = RubyMarshalToken.Fixnum;

            if ( value is BigInteger bigIntValue )
            {
                token = RubyMarshalToken.Bignum;
            }
            else if ( value is long longValue && ( longValue > int.MaxValue || longValue < int.MinValue ) )
            {
                token = RubyMarshalToken.Bignum;
            }
            else if ( value is ulong ulongValue && ( ulongValue > int.MaxValue ) )
            {
                token = RubyMarshalToken.Bignum;
            }

            bw.WriteValue( value, token, ( ) =>
            {
                if ( value is BigInteger bigIntValue )
                {
                    bw.WriteBigNum( bigIntValue );
                }
                else if ( value is long longValue && ( longValue > int.MaxValue || longValue < int.MinValue ) )
                {
                    bw.WriteBigNum( new BigInteger( longValue ) );
                }
                else if ( value is ulong ulongValue && ( ulongValue > int.MaxValue ) )
                {
                    bw.WriteBigNum( ( BigInteger ) ulongValue );
                }
                else
                {
                    bw.WriteFixNum( value );
                }
            } );
        }

        /// <summary>
        /// Write a Ruby FixNum of the specified type.
        /// </summary>
        /// <param name="bw"></param>
        /// <param name="value"></param>
        private static void WriteFixNum( this BinaryWriter bw, object value )
        {
            long numericValue;

            // Convert the value to a long to handle all numeric types uniformly
            if ( value is int intValue )
            {
                numericValue = intValue;
            }
            else if ( value is uint uintValue )
            {
                numericValue = uintValue;
            }
            else if ( value is long longValue )
            {
                numericValue = longValue;
            }
            else if ( value is short shortValue )
            {
                numericValue = shortValue;
            }
            else if ( value is ushort ushortValue )
            {
                numericValue = ushortValue;
            }
            else if ( value is byte byteValue )
            {
                numericValue = byteValue;
            }
            else if ( value is sbyte sbyteValue )
            {
                numericValue = sbyteValue;
            }
            else if ( value is ulong ulongValue )
            {
                if ( ulongValue <= long.MaxValue )
                {
                    numericValue = ( long ) ulongValue;
                }
                else
                {
                    throw new InvalidCastException( "ulong value exceeds Fixnum range" );
                }
            }
            else
            {
                throw new InvalidCastException( "Unsupported type" );
            }

            // Now encode the numericValue as Fixnum according to its range
            if ( numericValue == 0 )
            {
                bw.Write( ( byte ) 0 );
            }
            else if ( numericValue > 0 && numericValue <= 122 )
            {
                bw.Write( ( byte ) 1 );
                bw.Write( ( byte ) numericValue );
            }
            else if ( numericValue < 0 && numericValue >= -123 )
            {
                bw.Write( ( byte ) numericValue );
            }
            else if ( numericValue > 122 && numericValue <= 0x7FFF )
            {
                // Values up to 32,767 require 2 bytes
                bw.Write( ( byte ) 2 ); // Indicates that 2 bytes follow
                bw.Write( ( ushort ) numericValue ); // Write 2-byte value
            }
            else if ( numericValue > 0x7FFF && numericValue <= 0xFFFF )
            {
                // Values from 32,768 to 65,535 (e.g., 50000) also require 2 bytes
                bw.Write( ( byte ) 2 ); // Indicates that 2 bytes follow (not 3!)
                bw.Write( ( ushort ) numericValue ); // Write 2-byte value
            }
            else if ( numericValue > 0xFFFF && numericValue <= 0xFFFFFF )
            {
                // Values up to 16,777,215 (2^24 - 1) require 3 bytes
                bw.Write( ( byte ) 3 ); // Indicates that 3 bytes follow
                bw.Write( ( byte ) ( numericValue & 0xFF ) ); // LSB
                bw.Write( ( byte ) ( ( numericValue >> 8 ) & 0xFF ) ); // Middle byte
                bw.Write( ( byte ) ( ( numericValue >> 16 ) & 0xFF ) ); // MSB
            }
            else if ( numericValue > 0xFFFFFF && numericValue <= 0x7FFFFFFF )
            {
                // Values up to 2,147,483,647 (2^31 - 1) require 4 bytes
                bw.Write( ( byte ) 4 ); // Indicates that 4 bytes follow
                bw.Write( ( int ) numericValue ); // Write 4-byte value
            }
            else
            {
                throw new InvalidCastException( "Value exceeds Fixnum range, consider Bignum" );
            }
        }


        /// <summary>
        /// Write a Ruby BigNum to the stream.
        /// </summary>
        /// <param name="bw"></param>
        /// <param name="value"></param>
        private static void WriteBigNum( this BinaryWriter bw, BigInteger value )
        {
            // Determine the sign and write the corresponding token
            bw.Write( value.Sign >= 0 ? ( byte ) '+' : ( byte ) '-' );

            // Convert to absolute value for writing
            BigInteger absValue = BigInteger.Abs( value );

            // Get the byte array representation of the BigInteger
            byte[] magnitude = absValue.ToByteArray( );

            // Adjust the length to account for rounding up to the nearest short
            int lengthInBytes = magnitude.Length;
            if ( lengthInBytes % 2 != 0 )
            {
                lengthInBytes++; // Ensure length is rounded up to the nearest even number
            }
            int lengthInShorts = lengthInBytes / 2;

            // Write the length in shorts as a Fixnum
            bw.WriteFixNum( lengthInShorts );

            // Write the magnitude bytes
            bw.Write( magnitude );
        }

        /// <summary>
        /// Write a double in Ruby Marshal format.
        /// </summary>
        /// <param name="bw"></param>
        /// <param name="value"></param>
        public static void WriteDouble( this BinaryWriter bw, double value )
        {
            bw.Write( RubyMarshalToken.Double );

            // Convert the double to a string with invariant culture
            var floatString = value.ToString( "R", System.Globalization.CultureInfo.InvariantCulture );
            byte[] stringBytes = System.Text.Encoding.ASCII.GetBytes( floatString );

            // Write the length of the string (as a Fixnum) - Actual length of the string without padding
            bw.WriteFixNum( stringBytes.Length );

            // Write the string bytes
            bw.Write( stringBytes );
        }


        public static void WriteRubyString( this BinaryWriter bw, string value )
        {
            bw.WriteValue( value, RubyMarshalToken.String, ( ) =>
            {
                byte[] bytes = Encoding.UTF8.GetBytes( value );
                bw.WriteFixNum( bytes.Length );
                bw.Write( bytes );
            } );
        }

        public static void WriteRegex( this BinaryWriter bw, Regex value )
        {
            bw.WriteValue( value, RubyMarshalToken.RegularExpression, ( ) =>
            {
                var pattern = value.ToString( );
                var options = value.Options;

                byte[] patternBytes = Encoding.UTF8.GetBytes( pattern );
                bw.WriteFixNum( patternBytes.Length );
                bw.Write( patternBytes );

                byte optionsByte = 0;

                if ( options.HasFlag( RegexOptions.IgnoreCase ) )
                    optionsByte |= 1;

                if ( options.HasFlag( RegexOptions.IgnorePatternWhitespace ) )
                    optionsByte |= 2;

                if ( options.HasFlag( RegexOptions.Multiline ) )
                    optionsByte |= 4;

                bw.Write( optionsByte );
            } );
        }
    }
}