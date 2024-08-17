using RmSharp.Converters;

namespace RmSharp.Tests.Converters
{
    [TestClass]
    public class Boolean
    {
        private readonly byte[] _rubyMarshalData = new byte[] { 0x54 };  // Represents `true`
        private readonly bool _expectedValue = true;

        [TestMethod]
        public void Read( )
        {
            var converter = RmConverterFactory.GetConverter( typeof( bool ) );
            using ( var memoryStream = new MemoryStream( _rubyMarshalData ) )
            using ( var reader = new BinaryReader( memoryStream ) )
            {
                var result = converter.Read( reader );

                Assert.IsNotNull( result );
                Assert.IsInstanceOfType( result, typeof( bool ) );
                Assert.AreEqual( _expectedValue, result );
            }
        }

        [TestMethod]
        public void Write( )
        {
            var converter = RmConverterFactory.GetConverter( typeof( bool ) );
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