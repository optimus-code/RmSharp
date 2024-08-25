using RmSharp.Exceptions;
using RmSharp.Extensions;
using RmSharp.Tokens;
using System;
using System.IO;

namespace RmSharp.Converters.Types
{
    public class ArrayConverter : RmTypeConverter
    {
        private readonly RmConverter _elementConverter;

        public ArrayConverter( Type type ) : base( type )
        {
            var elementType = type.GetElementType( );
            _elementConverter = RmConverterFactory.GetConverter( elementType );
        }

        public override object Read( BinaryReader reader )
        {
            return reader.ReadValue( ( token ) =>
            {
                var size = reader.ReadFixNum<int>( );

                Array array = Array.CreateInstance( Type.GetElementType( ), size );

                for ( int i = 0; i < size; i++ )
                {
                    var element = _elementConverter.Read( reader );
                    array.SetValue( element, i );
                }

                return array;
            }, RubyMarshalToken.Array );
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            var array = ( Array ) instance;

            writer.WriteValue( array, RubyMarshalToken.Array, ( ) =>
            {
                writer.WriteFixNum( array.Length );

                foreach ( var element in array )
                {
                    _elementConverter.Write( writer, element );
                }
            } );
        }
    }
}