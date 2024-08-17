using RmSharp.Extensions;
using System;
using System.IO;

namespace RmSharp.Converters.Types.Basic
{
    public class SingleConverter : RmTypeConverter<float>
    {
        public override object Read( BinaryReader reader )
        {
            return ( float ) reader.ReadDoubleCustom( );
        }

        public override void Write( BinaryWriter writer, object instance )
        {
            double convertedValue = ( double ) (float) instance;
            double roundedValue = Math.Round( convertedValue, 7 );
            writer.WriteDouble( roundedValue );
        }
    }
}