using RmSharp.Extensions;
using RmSharp.Tokens;
using System;
using System.Collections;
using System.IO;

namespace RmSharp.Converters.Types
{
    public class DictionaryWithDefaultConverter : RmTypeConverter
    {
        private readonly RmTypeConverter _keyConverter;
        private readonly RmTypeConverter _valueConverter;
        private readonly RmTypeConverter _dictionaryConverter;

        public DictionaryWithDefaultConverter( Type type ) : base( type )
        {
            var genericArgs = type.GetGenericArguments( );
            var dictionaryType = genericArgs[0];
            var valueType = genericArgs[1];

            _valueConverter = RmConverterFactory.GetConverter( valueType );
            _dictionaryConverter = new DictionaryConverter( dictionaryType );
        }

        public override object Read( BinaryReader reader )
        {
            var dictionary = ( IDictionary ) _dictionaryConverter.Read( reader );

            var defaultValue = reader.ReadValue( ( token ) =>
            {
                return _valueConverter.Read( reader );
            }, RubyMarshalToken.DefaultHash );

            return (dictionary, defaultValue);
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            var value = ( ( IDictionary, object) ) instance;

            _dictionaryConverter.Write( writer, value.Item1 );
            _valueConverter.Write( writer, value.Item2 );
        }
    }
}