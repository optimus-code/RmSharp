using RmSharp.Converters;
using RmSharp.Tokens;

namespace RmSharp.Tests.Converters
{
    [TestClass]
    public class Int32
    {
        // Test data for the value 32000
        private readonly byte[] _rubyMarshalData32000 = new byte[] { 0x69, 0x02, 0x00, 0x7D };
        private readonly int _expectedValue32000 = 32000;

        private readonly byte[] _rubyMarshalData3 = new byte[] { 0x69, 0x08 };
        private readonly int _expectedValue3 = 3;


        [TestMethod]
        public void Read32000( )
        {
            var converter = RmConverterFactory.GetConverter( typeof( int ) );
            using ( var memoryStream = new MemoryStream( _rubyMarshalData32000 ) )
            using ( var reader = new BinaryReader( memoryStream ) )
            {
                var result = converter.Read( reader );

                Assert.IsNotNull( result );
                Assert.IsInstanceOfType( result, typeof( int ) );
                Assert.AreEqual( _expectedValue32000, result );
            }
        }

        [TestMethod]
        public void Write32000( )
        {
            var converter = RmConverterFactory.GetConverter( typeof( int ) );
            using ( var memoryStream = new MemoryStream( ) )
            using ( var writer = new BinaryWriter( memoryStream ) )
            {
                converter.Write( writer, _expectedValue32000 );

                byte[] writtenData = memoryStream.ToArray( );
                CollectionAssert.AreEqual( _rubyMarshalData32000, writtenData );
            }
        }

        [TestMethod]
        public void Read3( )
        {
            var converter = RmConverterFactory.GetConverter( typeof( int ) );
            using ( var memoryStream = new MemoryStream( _rubyMarshalData3 ) )
            using ( var reader = new BinaryReader( memoryStream ) )
            {
                var result = converter.Read( reader );

                Assert.IsNotNull( result );
                Assert.IsInstanceOfType( result, typeof( int ) );
                Assert.AreEqual( _expectedValue3, result );
            }
        }

        [TestMethod]
        public void Write3( )
        {
            var converter = RmConverterFactory.GetConverter( typeof( int ) );
            using ( var memoryStream = new MemoryStream( ) )
            using ( var writer = new BinaryWriter( memoryStream ) )
            {
                converter.Write( writer, _expectedValue3 );

                byte[] writtenData = memoryStream.ToArray( );
                CollectionAssert.AreEqual( _rubyMarshalData3, writtenData );
            }
        }
    }
}
