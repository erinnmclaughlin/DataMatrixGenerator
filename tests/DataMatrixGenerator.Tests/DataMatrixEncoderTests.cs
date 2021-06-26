using Xunit;

namespace DataMatrixGenerator.Tests
{
    public class DataMatrixEncoderTests
    {
        [Fact]
        public void Encode_ValidContent_ShouldEncodeDataMatrixCodeCorrectly()
        {
            var content = "abc-12345";
            var encodedContent = DataMatrixEncoder.Encode(content, 8);
            Assert.NotNull(encodedContent);

            var decodedContent = DataMatrixDecoder.Decode(encodedContent);
            Assert.NotNull(decodedContent);

            Assert.Equal(content, decodedContent);
        }
    }
}
