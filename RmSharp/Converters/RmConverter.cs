using System;
using System.IO;
using RmSharp.Enums;

namespace RmSharp.Converters
{
    public abstract class RmConverter
    {
        public abstract Type Type
        {
            get;
        }

        public abstract RubyControlToken[] Tokens
        {
            get;
        }

        public abstract object Read( BinaryReader reader );
        public abstract void Write( BinaryWriter writer, object instance );
    }

    public abstract class RmConverter<T> : RmConverter
    {
        public override Type Type
        {
            get;
        } = typeof( T );

        public RmConverter( ) 
            : base( )
        {
        }
    }
}