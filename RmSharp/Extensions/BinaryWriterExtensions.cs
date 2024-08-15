using RmSharp.Tokens;
using System;
using System.IO;

namespace RmSharp.Extensions
{
    public static class BinaryWriterExtensions
    {
        /// <summary>
        /// Write a token to the stream
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public static void Write( this BinaryWriter writer, RubyMarshalToken token )
        {
            writer.Write( ( byte ) token );
        }

        /// <summary>
        /// Write a Ruby FixNum of the specified type
        /// </summary>
        /// <param name="bw"></param>
        /// <param name="value"></param>
        public static void WriteFixNum( this BinaryWriter bw, object value )
        {
            if ( value is int intValue )
            {
                WriteInt32( bw, intValue );
            }
            else if ( value is uint uintValue )
            {
                WriteUInt32( bw, uintValue );
            }
            else if ( value is short shortValue )
            {
                WriteInt32( bw, shortValue );
            }
            else if ( value is ushort ushortValue )
            {
                WriteUInt32( bw, ushortValue );
            }
            else
            {
                throw new InvalidCastException( "Unsupported type" );
            }
        }

        private static void WriteInt32( BinaryWriter bw, int value )
        {
            if ( value == 0 )
            {
                bw.Write( ( byte ) 0 );
            }
            else if ( value > 0 && value <= byte.MaxValue )
            {
                bw.Write( ( byte ) 1 );
                bw.Write( ( byte ) value );
            }
            else if ( value < 0 && value >= -byte.MaxValue )
            {
                bw.Write( ( byte ) 0xff );
                bw.Write( ( byte ) ( -value ) );
            }
            else if ( value > 0 && value <= ushort.MaxValue )
            {
                bw.Write( ( byte ) 2 );
                bw.Write( ( ushort ) value );
            }
            else if ( value < 0 && value >= -ushort.MaxValue )
            {
                bw.Write( ( byte ) 0xfe );
                bw.Write( ( ushort ) ( -value ) );
            }
            else
            {
                bw.Write( ( byte ) 4 );
                bw.Write( value );
            }
        }

        private static void WriteUInt32( BinaryWriter bw, uint value )
        {
            if ( value == 0 )
            {
                bw.Write( ( byte ) 0 );
            }
            else if ( value <= byte.MaxValue )
            {
                bw.Write( ( byte ) 1 );
                bw.Write( ( byte ) value );
            }
            else if ( value <= ushort.MaxValue )
            {
                bw.Write( ( byte ) 2 );
                bw.Write( ( ushort ) value );
            }
            else if ( value <= 0xFFFFFF )
            {
                bw.Write( ( byte ) 3 );
                byte[] buffer = BitConverter.GetBytes( value );
                bw.Write( buffer, 0, 3 );
            }
            else
            {
                bw.Write( ( byte ) 4 );
                bw.Write( value );
            }
        }
    }
}