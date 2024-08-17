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
        public static void WriteFixNum( this BinaryWriter bw, object value )
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
                bw.Write( ( byte ) 0 ); // 0 is encoded directly
            }
            else if ( numericValue > 0 && numericValue <= 122 )
            {
                // Directly encode small positive integers (range 1-122)
                bw.Write( ( byte ) ( numericValue + 5 ) ); // Ruby Marshal format adds 5
            }
            else if ( numericValue < 0 && numericValue >= -123 )
            {
                // Directly encode small negative integers (range -1 to -123)
                bw.Write( ( byte ) ( numericValue - 5 ) ); // Ruby Marshal format subtracts 5
            }
            else if ( numericValue >= 123 && numericValue <= 0xFF )
            {
                // Values between 123 and 255 (2 bytes, 0x01 marker)
                bw.Write( ( byte ) 1 ); // Marker indicating a single following byte
                bw.Write( ( byte ) numericValue );
            }
            else if ( numericValue >= -256 && numericValue <= -124 )
            {
                // Negative values between -256 and -124 (2 bytes, 0xFF marker)
                bw.Write( ( byte ) 0xFF ); // Marker indicating a single following negative byte
                bw.Write( ( byte ) ( -numericValue ) ); // Convert to positive and write
            }
            else if ( numericValue >= 0x100 && numericValue <= 0x7FFF )
            {
                // Values that can fit into 2 bytes (3 bytes total)
                bw.Write( ( byte ) 2 ); // Marker indicating two following bytes
                bw.Write( ( ushort ) numericValue ); // Write 2-byte value
            }
            else if ( numericValue >= -0x8000 && numericValue <= -257 )
            {
                // Negative values that can fit into 2 bytes (3 bytes total)
                bw.Write( ( byte ) 0xFE ); // Marker indicating two following negative bytes
                bw.Write( ( ushort ) ( -numericValue ) ); // Convert to positive and write
            }
            else if ( numericValue >= 0x8000 && numericValue <= 0xFFFFFF )
            {
                // Values that can fit into 3 bytes (4 bytes total)
                bw.Write( ( byte ) 3 ); // Marker indicating three following bytes
                bw.Write( ( byte ) ( numericValue & 0xFF ) ); // LSB
                bw.Write( ( byte ) ( ( numericValue >> 8 ) & 0xFF ) ); // Middle byte
                bw.Write( ( byte ) ( ( numericValue >> 16 ) & 0xFF ) ); // MSB
            }
            else if ( numericValue >= -0xFFFFFF && numericValue <= -0x8001 )
            {
                // Negative values that can fit into 3 bytes (4 bytes total)
                bw.Write( ( byte ) 0xFD ); // Marker indicating three following negative bytes
                uint absValue = ( uint ) ( -numericValue );
                bw.Write( ( byte ) ( absValue & 0xFF ) ); // LSB
                bw.Write( ( byte ) ( ( absValue >> 8 ) & 0xFF ) ); // Middle byte
                bw.Write( ( byte ) ( ( absValue >> 16 ) & 0xFF ) ); // MSB
            }
            else if ( numericValue >= 0x1000000 && numericValue <= 0x7FFFFFFF )
            {
                // Values that can fit into 4 bytes (5 bytes total)
                bw.Write( ( byte ) 4 ); // Marker indicating four following bytes
                bw.Write( ( int ) numericValue ); // Write 4-byte value
            }
            else if ( numericValue >= -0x80000000 && numericValue <= -0x1000001 )
            {
                // Negative values that can fit into 4 bytes (5 bytes total)
                bw.Write( ( byte ) 0xFC ); // Marker indicating four following negative bytes
                bw.Write( ( uint ) ( -numericValue ) ); // Convert to positive and write
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