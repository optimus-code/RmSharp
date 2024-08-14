using RmSharp.Tokens;
using System;
using System.IO;

namespace RmSharp.Converters
{
    public abstract class RmConverter
    {
        public abstract object Read( BinaryReader reader );
        public abstract void Write( BinaryWriter writer, object instance );
    }

    public abstract class RmTypeConverter : RmConverter
    {
        public virtual Type Type
        {
            get;
            protected set;
        }

        public RmTypeConverter( Type type )
        {
            Type = type;
        }
    }

    public abstract class RmTypeConverter<T> : RmTypeConverter
    {
        public RmTypeConverter( ) : base( typeof( T ) )
        {

        }
    }

    public abstract class RmTokenConverter : RmConverter
    {
        public virtual RubyMarshalToken Token
        {
            get;
            protected set;
        }

        public RmTokenConverter( RubyMarshalToken token )
        {
            Token = token;
        }
    }
}