namespace RmSharp.Tokens
{
    /// <summary>
    /// Enum representing the type tokens used in Ruby's Marshal format.
    /// </summary>
    public enum RubyTypeToken : byte
    {
        /// <summary>
        /// Represents an array ('[' token).
        /// </summary>
        /// <remarks>
        /// This token is used to indicate the start of a serialized Ruby array.
        /// </remarks>
        Array = 0x5B,

        /// <summary>
        /// Represents a string ('"' token).
        /// </summary>
        /// <remarks>
        /// This token is used to serialize Ruby strings.
        /// </remarks>
        String = 0x22,

        /// <summary>
        /// Represents a bignum ('l' token).
        /// </summary>
        /// <remarks>
        /// This token is used to serialize large integers (Bignum) in Ruby.
        /// </remarks>
        Bignum = 0x6C,

        /// <summary>
        /// Represents a fixnum ('i' token).
        /// </summary>
        /// <remarks>
        /// This token is used to serialize small integers (Fixnum) in Ruby.
        /// </remarks>
        Fixnum = 0x69,

        /// <summary>
        /// Represents a floating-point number ('f' token).
        /// </summary>
        /// <remarks>
        /// This token is used to serialize floating-point numbers (Float) in Ruby.
        /// </remarks>
        Single = 0x66,

        /// <summary>
        /// Represents a regular expression ('/' token).
        /// </summary>
        /// <remarks>
        /// This token is used to serialize Ruby regular expressions (Regexp).
        /// </remarks>
        RegularExpression = 0x2F,

        /// <summary>
        /// Represents a structure ('S' token).
        /// </summary>
        /// <remarks>
        /// This token is used to serialize Ruby structs.
        /// </remarks>
        Struct = 0x53,

        /// <summary>
        /// Represents a hash ('{' token).
        /// </summary>
        /// <remarks>
        /// This token is used to serialize Ruby hashes.
        /// </remarks>
        Hash = 0x7B,

        /// <summary>
        /// Represents a class ('c' token).
        /// </summary>
        /// <remarks>
        /// This token is used to serialize Ruby classes.
        /// </remarks>
        Class = 0x63,

        /// <summary>
        /// Represents user-defined data ('d' token).
        /// </summary>
        /// <remarks>
        /// This token indicates user-defined data types that require special handling.
        /// </remarks>
        Data = 0x64,

        /// <summary>
        /// Represents a user-defined class ('C' token).
        /// </summary>
        /// <remarks>
        /// This token is used to serialize an object that belongs to a user-defined class.
        /// </remarks>
        UserClass = 0x43,

        /// <summary>
        /// Represents a default hash ('}' token).
        /// </summary>
        /// <remarks>
        /// This token is used to serialize Ruby hashes that have a default value.
        /// </remarks>
        DefaultHash = 0x7D,
    }
}