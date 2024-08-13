using System.Collections.Generic;
using RmSharp.Enums;

namespace RmSharp.Converters
{
    internal static class RmConverterFactory
    {
        private static readonly Dictionary<RubyControlToken, RmConverter> _converters =
            [
            ];

        public RmConverter GetConverter( Type type )
        {
            if ( _converters.TryGetValue( type, out var converter ) )
            {

            }
        }
    }
}