using RmSharp.Exceptions;
using RmSharp.Extensions;
using RmSharp.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace RmSharp.Converters.Types
{
    public class ListConverter : RmTypeConverter
    {
        private readonly RmTypeConverter _elementConverter;

        public ListConverter( Type type ) : base( type )
        {
            var elementType = type.GetGenericArguments( )[0];
            _elementConverter = RmConverterFactory.GetConverter( elementType );
        }

        public override object Read( BinaryReader reader )
        {
            return reader.ReadValue( ( token ) =>
            {
                if ( token != RubyMarshalToken.Array )
                    return null;

                var size = reader.ReadNumeric<int>( );

                var list = ( IList ) Activator.CreateInstance( Type );

                for ( int i = 0; i < size; i++ )
                {
                    var element = _elementConverter.Read( reader );
                    list.Add( element );
                }

                return list;
            }, RubyMarshalToken.Array );
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            var list = ( IList ) instance;

            writer.WriteValue( list, RubyMarshalToken.Array, ( ) =>
            {
                writer.WriteNumeric( list.Count );

                foreach ( var element in list )
                {
                    _elementConverter.Write( writer, element );
                }
            } );
        }
    }
}