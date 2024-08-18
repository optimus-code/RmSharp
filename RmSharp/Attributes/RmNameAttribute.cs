using System;

namespace RmSharp.Attributes
{
    public class RmNameAttribute( string name ) : Attribute
    {
        public string Name
        {
            get;
            private set;
        } = name;
    }
}