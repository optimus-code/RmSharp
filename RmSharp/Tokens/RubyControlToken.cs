namespace RmSharp.Tokens
{
    /// <summary>
    /// Enum representing the tokens used in Ruby's Marshal format.
    /// </summary>
    public enum RubyControlToken : byte
    {
        /// <summary>
        /// Represents an object ('o' token).
        /// </summary>
        /// <remarks>
        /// This token is used to indicate the start of a serialized Ruby object.
        /// </remarks>
        Object = 0x6F,

        /// <summary>
        /// Represents an instance variable ('I' token).
        /// </summary>
        /// <remarks>
        /// This token indicates that the object has instance variables that follow the token.
        /// </remarks>
        InstanceVariable = 0x49,

        /// <summary>
        /// Represents an extended module ('e' token).
        /// </summary>
        /// <remarks>
        /// This token is used when an object is extended by a module.
        /// </remarks>
        Extended = 0x65,

        /// <summary>
        /// Represents user-defined data serialization logic ('u' token).
        /// </summary>
        /// <remarks>
        /// This token is used for objects that have custom serialization logic.
        /// </remarks>
        UserDefined = 0x75,

        /// <summary>
        /// Represents a user-defined marshal ('U' token).
        /// </summary>
        /// <remarks>
        /// This token indicates that the object has a custom marshalling method.
        /// </remarks>
        UserMarshal = 0x55,

        /// <summary>
        /// Represents a module ('m' token).
        /// </summary>
        /// <remarks>
        /// This token is used to serialize Ruby modules.
        /// </remarks>
        Module = 0x6D,
    }
}