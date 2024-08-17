using RmSharp.Converters;
using RmSharp.Tokens;

namespace RmSharp.Tests.Converters
{
    [TestClass]
    public class Int64
    {
        private readonly byte[] _rubyMarshalData = new byte[] { ( byte ) RubyMarshalToken.Fixnum, 0x03, 0xA0, 0x86, 0x01 };
        private readonly long _expectedValue = 100000;

        [TestMethod]
        public void Read( )
        {
            var converter = RmConverterFactory.GetConverter( typeof( long ) );
            using ( var memoryStream = new MemoryStream( _rubyMarshalData ) )
            using ( var reader = new BinaryReader( memoryStream ) )
            {
                var result = converter.Read( reader );

                Assert.IsNotNull( result );
                Assert.IsInstanceOfType( result, typeof( long ) );
                Assert.AreEqual( _expectedValue, result );
            }
        }

        [TestMethod]
        public void Write( )
        {
            var converter = RmConverterFactory.GetConverter( typeof( long ) );
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