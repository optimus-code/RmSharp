using RmSharp.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace RmSharp.Converters
{
    public class RmTokefnConverter : RmConverter
    {
        private readonly Type _targetType;
        private readonly Dictionary<string, PropertyInfo> _properties;

        public RmTokefnConverter( Type targetType )
        {
            _targetType = targetType;

            // Map property names to their PropertyInfo objects for quick access
            _properties = new Dictionary<string, PropertyInfo>( );
            foreach ( var prop in _targetType.GetProperties( BindingFlags.Public | BindingFlags.Instance ) )
            {
                _properties[prop.Name] = prop;
            }
        }

        public override object Read( BinaryReader reader )
        {
            // Create an instance of the target type
            var instance = Activator.CreateInstance( _targetType );

            // Core loop for reading tokens and mapping them to properties
            var token = ( byte ) reader.ReadByte( );

            // Check for Object or InstanceVariable control tokens
            if ( token != ( byte ) RubyMarshalToken.Object && token != ( byte ) RubyMarshalToken.InstanceVariable )
            {
                throw new NotSupportedException( $"Expected object or instance variable, found {token:X2}" );
            }

            // Read and validate the class name
            var className = ( string ) RmConverterFactory.GetConverter( RubyTypeToken.String ).Read( reader );
            if ( className != _targetType.Name )
            {
                throw new InvalidOperationException( $"Expected class '{_targetType.Name}', but found '{className}'" );
            }

            // Read the number of properties
            int propertyCount = reader.ReadInt32( );
            for ( int i = 0; i < propertyCount; i++ )
            {
                // Read the property name (symbol) token
                var symbolToken = reader.ReadByte( );
                if ( symbolToken != ( byte ) RubyMetaDataToken.Symbol )
                {
                    throw new InvalidOperationException( $"Expected symbol token, but found {symbolToken:X2}" );
                }

                var symbolName = ( string ) RmConverterFactory.GetConverter( RubyMetaDataToken.Symbol ).Read( reader );

                // Check if the symbol corresponds to a property on the target type
                if ( _properties.TryGetValue( symbolName, out var propertyInfo ) )
                {
                    // Get the appropriate converter for the property type
                    var converter = RmConverterFactory.GetConverter( propertyInfo.PropertyType );
                    var value = converter.Read( reader );

                    // Set the value on the property
                    propertyInfo.SetValue( instance, value );
                }
                else
                {
                    throw new InvalidOperationException( $"No property found for symbol '{symbolName}' in {_targetType.Name}" );
                }
            }

            return instance;
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            // Write the Object control token
            writer.Write( ( byte ) RubyMarshalToken.Object );

            // Write the class name as a string
            RmConverterFactory.GetConverter( RubyTypeToken.String ).Write( writer, _targetType.Name );

            // Get all public instance properties
            var properties = _targetType.GetProperties( BindingFlags.Public | BindingFlags.Instance );
            writer.Write( properties.Length ); // Write the number of properties

            foreach ( var prop in properties )
            {
                // Write the symbol token for the property name
                writer.Write( ( byte ) RubyMetaDataToken.Symbol );
                RmConverterFactory.GetConverter( RubyMetaDataToken.Symbol ).Write( writer, prop.Name );

                // Write the value using the appropriate converter
                var converter = RmConverterFactory.GetConverter( prop.PropertyType );
                var value = prop.GetValue( instance );
                converter.Write( writer, value );
            }
        }
    }
}