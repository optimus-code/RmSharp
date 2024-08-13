using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace RmSharp.Converters.Types
{
    /// <summary>
    /// Provides methods to convert lists of type <typeparamref name="T"/> in the Ruby Marshal format.
    /// </summary>
    /// <typeparam name="T">The element type of the list.</typeparam>
    public class RmListConverter<T> : RmConverter<List<T>>
    {
        private readonly RmConverter _elementConverter;
        private readonly Type _elementType;

        /// <summary>
        /// Initializes a new instance of the <see cref="RmListConverter{T}"/> class.
        /// </summary>
        public RmListConverter( )
        {
            _elementType = typeof( T );
            _elementConverter = RmConverterFactory.GetConverter( _elementType );
        }

        /// <summary>
        /// Reads a list of elements of type <typeparamref name="T"/> from the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader to read from.</param>
        /// <param name="length">The length of the data to read, if applicable.</param>
        /// <returns>The list of elements read from the binary reader.</returns>
        public override object Read( BinaryReader reader )
        {
            int count = reader.ReadInt32( );
            var list = new List<T>( count );

            for ( int i = 0; i < count; i++ )
            {
                list.Add( ( T ) _elementConverter.Read( reader ) );
            }

            return list;
        }

        /// <summary>
        /// Writes the specified list of elements to the binary writer.
        /// </summary>
        /// <param name="writer">The binary writer to write to.</param>
        /// <param name="value">The list of elements to write.</param>
        public override void Write( BinaryWriter writer, object value )
        {
            var list = ( List<T> ) value;

            // Write the count of items in the list
            writer.Write( list.Count );

            foreach ( var item in list )
            {
                _elementConverter.Write( writer, item );
            }
        }
    }
}