namespace RmSharp.Enums
{
    /// <summary>
    /// Enum representing the value tokens used in Ruby's Marshal format.
    /// </summary>
    public enum RubyValueToken : byte
    {
        /// <summary>
        /// Represents a nil value ('0' token).
        /// </summary>
        /// <remarks>
        /// This token is used to indicate a nil value in Ruby, which is equivalent to a null value in C#.
        /// </remarks>
        Nil = 0x30,

        /// <summary>
        /// Represents a true value ('T' token).
        /// </summary>
        /// <remarks>
        /// This token is used to indicate a true boolean value in Ruby.
        /// </remarks>
        True = 0x54,

        /// <summary>
        /// Represents a false value ('F' token).
        /// </summary>
        /// <remarks>
        /// This token is used to indicate a false boolean value in Ruby.
        /// </remarks>
        False = 0x46,
    }
}