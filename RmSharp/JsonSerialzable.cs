using System.Text.Json.Nodes;

namespace RmSharp
{
    public interface IJsonSerialzable
    {
        public abstract JsonNode? ToJson( );
    }
}
