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
            var value = reader.ReadValue( ( token ) =>
            {
                var size = reader.ReadFixNum<int>( );

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

            if ( reader.PeekByte( out var byteValue ) &&
                byteValue == ( byte ) RubyMarshalToken.DefaultHash )
            {
                reader.ReadByte( ); // Read out the token
                var defaultValue = _valueConverter.Read( reader );

                // Do something with this later
            }
            return value;
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            var dictionary = ( IDictionary ) instance;

            writer.WriteValue( dictionary, RubyMarshalToken.Hash, ( ) =>
            {
                writer.WriteFixNum( dictionary.Count );

                foreach ( DictionaryEntry entry in dictionary )
                {
                    _keyConverter.Write( writer, entry.Key );
                    _valueConverter.Write( writer, entry.Value );
                }
            } );
        }
    }
}