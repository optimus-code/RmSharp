using RmSharp.Converters;
using RmSharp.Tokens;

namespace RmSharp.Tests.Converters
{

    [TestClass]
    public class BigInteger
    {
        private readonly byte[] _rubyMarshalData = new byte[]
   {
    (byte)RubyMarshalToken.Bignum, // Bignum token (0x2B)
    (byte)'+',                     // Sign byte ('+' for positive)
    0x08,                          // Length of the Bignum in shorts (2.5 shorts, 5 bytes)
    0x00, 0xE4,                    // 1st short (0x00E4)
    0x0B, 0x54,                    // 2nd short (0x540B)
    0x02                        // Half-short (0x02)
   };

        private readonly System.Numerics.BigInteger _expectedValue = new System.Numerics.BigInteger( 10000000000 );


        [TestMethod]
        public void Read( )
        {
            var converter = RmConverterFactory.GetConverter( typeof( System.Numerics.BigInteger ) );
            using ( var memoryStream = new MemoryStream( _rubyMarshalData ) )
            using ( var reader = new BinaryReader( memoryStream ) )
            {
                var result = converter.Read( reader );

                Assert.IsNotNull( result );
                Assert.IsInstanceOfType( result, typeof( System.Numerics.BigInteger ) );
                Assert.AreEqual( _expectedValue, result );
            }
        }

        [TestMethod]
        public void Write( )
        {
            var converter = RmConverterFactory.GetConverter( typeof( System.Numerics.BigInteger ) );
            using ( var memoryStream = new MemoryStream( ) )
            using ( var writer = new BinaryWriter( memoryStream ) )
            {
                converter.Write( writer, _expectedValue );

                byte[] writtenData = memoryStream.ToArray( );
                CollectionAssert.AreEqual( _rubyMarshalData, writtenData );
            }
        }
    }
}