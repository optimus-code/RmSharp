using RmSharp.Converters;
using System;

namespace RmSharp.Attributes
{
    public class RmBufferAttribute( string name, Type converter ) : Attribute
    {
        public string Name
        {
            get;
            private set;
        } = name;

        public Type Converter
        {
            get;
            private set;
        } = converter;
    }

    public class RmBufferAttribute<TConverter> : RmBufferAttribute
        where TConverter : RmConverter
    {
        public RmBufferAttribute( string name ) : base( name, typeof( TConverter ) )
        {
        }
    }
}
