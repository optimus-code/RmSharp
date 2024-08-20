using RmSharp.Attributes;
using RmSharp.Exceptions;
using RmSharp.Extensions;
using RmSharp.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RmSharp.Converters
{
    public class ClassConverter : RmTypeConverter
    {
        private readonly List<string> _readSymbols = new List<string>();
        private readonly List<string> _writeSymbols = new List<string>( );
        private readonly RmNameAttribute _typeName;
        private readonly List<(PropertyInfo, RmNameAttribute)> _instanceVariables = [];
        private readonly SymbolConverter _symbolConverter = RmConverterFactory.SymbolConverter;

        public ClassConverter( Type type ) 
            : base( type )
        {
            _typeName = type.GetCustomAttribute<RmNameAttribute>( );

            if ( _typeName == null )
                throw new RmException( $"No RmName attribute on type '{type.FullName}'." );

            var properties = type.GetProperties( BindingFlags.Instance | BindingFlags.Public );

            if ( properties == null || !properties.Any() )
                throw new RmException( $"No properties accessible on type '{type.FullName}'." );

            foreach ( var property in properties )
            {
                var nameAttribute = property.GetCustomAttribute<RmNameAttribute>( );

                if ( nameAttribute == null )
                    continue;

                _instanceVariables.Add((property, nameAttribute));
            }
        }

        public override object Read( BinaryReader reader )
        {
            return reader.ReadValue( ( token ) =>
            {
                var className = ( string ) _symbolConverter.Read( reader );

                if ( className != _typeName.Name )
                    throw new RmException( $"Class name '{className}' does not match expected type '{_typeName.Name}'." );

                var instance = Activator.CreateInstance( Type );
                var count = reader.ReadFixNum<int>( );

                for ( var i = 0; i < count; i++ )
                {
                    var symbolName = ( string ) _symbolConverter.Read( reader );                    

                    var property = _instanceVariables
                        .Where( i => symbolName == "@" + i.Item2.Name )
                        .Select( i => i.Item1 )
                        .FirstOrDefault( );

                    if ( property == null )
                        throw new RmException( $"Instance variable '{symbolName}' not found in type '{Type.FullName}'." );

                    var typeConverter = RmConverterFactory.GetConverter( property.PropertyType );

                    if ( typeConverter == null )
                        throw new RmException( $"No type converter found for '{property.PropertyType.FullName}'." );

                    object value = typeConverter.Read( reader );
                    property.SetValue( instance, value );
                }

                return instance;
            }, RubyMarshalToken.Object, RubyMarshalToken.UserClass );
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            writer.WriteValue( instance, RubyMarshalToken.Object, ( ) =>
            {
                _symbolConverter.Write( writer, _typeName.Name );

                writer.WriteFixNum( _instanceVariables.Count );

                foreach ( var instanceVariable in _instanceVariables )
                {
                    var rubyName = "@" + instanceVariable.Item2.Name;

                    _symbolConverter.Write( writer, rubyName );

                    var typeConverter = RmConverterFactory.GetConverter( instanceVariable.Item1.PropertyType );

                    if ( typeConverter == null )
                        throw new RmException( $"No type converter found for '{instanceVariable.Item1.PropertyType.FullName}'." );

                    typeConverter.Write( writer, instanceVariable.Item1.GetValue( instance ) );
                }
            } );
        }
    }
}