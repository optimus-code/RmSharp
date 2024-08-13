namespace RmSharp.Tokens
{
    /// <summary>
    /// Enum representing the tokens used in Ruby's Marshal format.
    /// </summary>
    public enum RubyReferebceToken : byte
    {
        /// <summary>
        /// Represents an object reference ('@' token).
        /// </summary>
        /// <remarks>
        /// This token is used to reference an object that has already been serialized, avoiding duplication.
        /// </remarks>
        ObjectReference = 0x40,

        /// <summary>
        /// Represents a symbol link (';' token).
        /// </summary>
        /// <remarks>
        /// This token refers to a previously serialized symbol, avoiding duplication and saving space.
        /// </remarks>
        SymbolLink = 0x3B,
    }
}