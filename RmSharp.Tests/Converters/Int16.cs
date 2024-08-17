using RmSharp.Converters;
using RmSharp.Tokens;

namespace RmSharp.Tests.Converters
{
    [TestClass]
    public class Int16
    {
        private readonly byte[] _rubyMarshalData = new byte[] { ( byte ) RubyMarshalToken.Fixnum, 0x02, 0xD2, 0x04 };
        private readonly short _expectedValue = 1234;

        [TestMethod]
        public void Read( )
        {
            var converter = RmConverterFactory.GetConverter( typeof( short ) );
            using ( var memoryStream = new MemoryStream( _rubyMarshalData ) )
            using ( var reader = new BinaryReader( memoryStream ) )
            {
                var result = converter.Read( reader );

                Assert.IsNotNull( result );
                Assert.IsInstanceOfType( result, typeof( short ) );
                Assert.AreEqual( _expectedValue, result );
            }
        }

        [TestMethod]
        public void Write( )
        {
            var converter = RmConverterFactory.GetConverter( typeof( short ) );
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