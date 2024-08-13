namespace RmSharp.Tokens
{
    /// <summary>
    /// Enum representing the tokens used in Ruby's Marshal format.
    /// </summary>
    public enum RubyMetaDataToken : byte
    {
        /// <summary>
        /// Represents a symbol (':' token).
        /// </summary>
        /// <remarks>
        /// This token is used for Ruby symbols, often as identifiers or property names in serialization.
        /// </remarks>
        Symbol = 0x3A,
    }
}