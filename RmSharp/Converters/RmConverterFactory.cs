using RmSharp.Converters.Types;
using RmSharp.Converters.Types.Basic;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;

namespace RmSharp.Converters
{
    public static class RmConverterFactory
    {
        public static SymbolConverter SymbolConverter
        {
            get;
            private set;
        } = new( );

        private static readonly Dictionary<Type, RmTypeConverter> _typeConverters =
            new Dictionary<Type, RmTypeConverter>
            {
                { typeof( BigInteger ), new BigIntegerConverter( ) },
                { typeof( bool ), new BooleanConverter( ) },
                { typeof( double ), new DoubleConverter( ) },
                { typeof( short ), new Int16Converter( ) },
                { typeof( int ), new Int32Converter( ) },
                { typeof( long ), new Int64Converter( ) },
                { typeof( Regex ), new RegexConverter( ) },
                { typeof( float ), new SingleConverter( ) },
                { typeof( string ), new StringConverter( ) },
                { typeof( ushort ), new UInt16Converter( ) },
                { typeof( uint ), new UInt32Converter( ) },
                { typeof( ulong ), new UInt64Converter( ) },
            };

        private static readonly Dictionary<Type, RmTypeConverter> _classConverters = [];

        public static RmTypeConverter GetConverter( Type type )
        {
            if ( _typeConverters.TryGetValue( type, out var converter ) )
            {
                return converter;
            }
            else if ( type.IsGenericType && type.GetGenericTypeDefinition() == typeof( List<> ) )
            {
                return new ListConverter( type );
            }
            else if ( type.IsArray )
            {
                return new ArrayConverter( type );
            }
            else if ( type.IsGenericType && type.GetGenericTypeDefinition( ) == typeof( Dictionary<,> ) )
            {
                return new DictionaryConverter( type );
            }
            else if ( type.IsClass )
            {
                if ( _classConverters.TryGetValue( type, out converter ) )
                {
                    return converter;
                }

                converter = new ClassConverter( type );
                _classConverters.Add( type, converter );
                return converter;
            }
            return null;
        }

        public static void Reset( )
        {
            SymbolConverter.Reset( );
        }
    }
}