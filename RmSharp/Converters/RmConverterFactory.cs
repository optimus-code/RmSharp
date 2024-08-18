using RmSharp.Converters.Types;
using RmSharp.Converters.Types.Basic;
using RmSharp.Tokens;
using System;
using System.Collections.Generic;
using System.Dynamic;
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


        private static readonly Dictionary<RubyMarshalToken, RmTypeConverter> _tokenMap =
            new Dictionary<RubyMarshalToken, RmTypeConverter>
            {
                { RubyMarshalToken.Bignum, _typeConverters[typeof( BigInteger )]},
                { RubyMarshalToken.True, _typeConverters[typeof( bool )]},
                { RubyMarshalToken.False, _typeConverters[typeof( bool )]},
                { RubyMarshalToken.Double, _typeConverters[typeof( double )]},
                { RubyMarshalToken.Fixnum, _typeConverters[typeof( long )]},
                { RubyMarshalToken.String, _typeConverters[typeof( string )]}
            };

        private static readonly Dictionary<Type, RmTypeConverter> _classConverters = [];
        private static readonly DynamicConverter _dynamicConverter = new( );

        public static RmTypeConverter GetConverter( RubyMarshalToken token )
        {
            if ( _tokenMap.TryGetValue( token, out var converter ) )
                return converter;

            if ( token == RubyMarshalToken.Array )
            {
                return new ListConverter( typeof( List<dynamic> ) );
            }
            else if ( token == RubyMarshalToken.Hash )
            {
                return new DictionaryConverter( typeof( Dictionary<dynamic, dynamic> ) );
            }
            else if ( token == RubyMarshalToken.Object )
            {
                // Handle this, it'll need to find the appropriate type based on name...

                //if ( _classConverters.TryGetValue( type, out converter ) )
                //{
                //    return converter;
                //}

                //converter = new ClassConverter( type );
                //_classConverters.Add( type, converter );
                //return converter;
            }

            return null;
        }

        public static RmConverter GetConverter( Type type )
        {
            if ( typeof( object ) == type || typeof( IDynamicMetaObjectProvider ).IsAssignableFrom( type ) )
            {
                return _dynamicConverter;
            }
            else if ( _typeConverters.TryGetValue( type, out var converter ) )
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