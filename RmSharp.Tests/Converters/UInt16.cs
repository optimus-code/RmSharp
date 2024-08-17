using RmSharp.Converters;
using RmSharp.Tokens;

namespace RmSharp.Tests.Converters
{
    [TestClass]
    public class UInt16
    {
        private readonly byte[] _rubyMarshalData = new byte[] { ( byte ) RubyMarshalToken.Fixnum, 0x03, 0x50, 0xC3, 0x00 };  // Represents `50000` for a 16-bit unsigned integer
        private readonly ushort _expectedValue = 50000;

        [TestMethod]
        public void Read( )
        {
            var converter = RmConverterFactory.GetConverter( typeof( ushort ) );
            using ( var memoryStream = new MemoryStream( _rubyMarshalData ) )
            using ( var reader = new BinaryReader( memoryStream ) )
            {
                var result = converter.Read( reader );

                Assert.IsNotNull( result );
                Assert.IsInstanceOfType( result, typeof( ushort ) );
                Assert.AreEqual( _expectedValue, result );
            }
        }

        [TestMethod]
        public void Write( )
        {
            var converter = RmConverterFactory.GetConverter( typeof( ushort ) );
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