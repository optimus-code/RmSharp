using RmSharp.Converters;
using System;

namespace RmSharp.Attributes
{
    public class RmConverterAttribute( Type converter ) : Attribute
    {
        public Type Converter
        {
            get;
            private set;
        } = converter;
    }

    public class RmConverterAttribute<TConverter> : RmConverterAttribute
        where TConverter : RmConverter
    {
        public RmConverterAttribute( ) : base( typeof( TConverter ) )
        {
        }
    }
}