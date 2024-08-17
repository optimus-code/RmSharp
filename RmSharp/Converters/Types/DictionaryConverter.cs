using RmSharp.Extensions;
using RmSharp.Tokens;
using System;
using System.Collections;
using System.IO;

namespace RmSharp.Converters.Types
{
    public class DictionaryConverter : RmTypeConverter
    {
        private readonly RmTypeConverter _keyConverter;
        private readonly RmTypeConverter _valueConverter;

        public DictionaryConverter( Type type ) : base( type )
        {
            var genericArgs = type.GetGenericArguments( );
            var keyType = genericArgs[0];
            var valueType = genericArgs[1];

            _keyConverter = RmConverterFactory.GetConverter( keyType );
            _valueConverter = RmConverterFactory.GetConverter( valueType );
        }

        public override object Read( BinaryReader reader )
        {
            return reader.ReadValue( ( token ) =>
            {
                var size = reader.ReadNumeric<int>( );

                var dictionary = ( IDictionary ) Activator.CreateInstance( Type );

                if ( size > 0 )
                {
                    for ( var i = 0; i < size; i++ )
                    {
                        var key = _keyConverter.Read( reader );
                        var value = _valueConverter.Read( reader );

                        dictionary.Add( key, value );
                    }
                }

                return dictionary;
            }, RubyMarshalToken.Hash );
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            var dictionary = ( IDictionary ) instance;

            writer.WriteValue( dictionary, RubyMarshalToken.Hash, ( ) =>
            {
                writer.WriteNumeric( dictionary.Count );

                foreach ( DictionaryEntry entry in dictionary )
                {
                    _keyConverter.Write( writer, entry.Key );
                    _valueConverter.Write( writer, entry.Value );
                }
            } );
        }
    }
}