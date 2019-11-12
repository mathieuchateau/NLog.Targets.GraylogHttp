using System;
using Xunit;

namespace NLog.Targets.GraylogHttp.Tests
{
    public class MessageBuilderTests
    {
        [Fact]
        public void SimpleMessageTest()
        {
            var testMessage = new GraylogMessageBuilder()
                .WithCustomProperty("facility", "Test")
                .WithProperty("short_message", "short_message")
                .WithProperty("host", "magic")
                .WithLevel(LogLevel.Debug)
                .WithCustomProperty("logger_name", "SimpleMessageTest")
                .Render(new DateTime(1970, 1, 1, 0, 0, 10, 5, DateTimeKind.Utc));

            var expectedMessage = "{\"_facility\":\"Test\",\"short_message\":\"short_message\",\"host\":\"magic\",\"level\":7,\"_logger_name\":\"SimpleMessageTest\",\"timestamp\":10.005,\"version\":\"1.1\"}";

            Assert.Equal(expectedMessage, testMessage);
        }

        [Fact]
        public void MessageWithHugePropertyTest()
        {
            var testMessage = new GraylogMessageBuilder()
                .WithCustomProperty("facility", "Test")
                .WithProperty("short_message", "short_message")
                .WithProperty("host", "magic")
                .WithLevel(LogLevel.Debug)
                .WithCustomProperty("logger_name", "SimpleMessageTest")
                .WithProperty("longstring", new string('*', 50000))
                .Render(new DateTime(1970, 1, 1, 0, 0, 10, 5, DateTimeKind.Utc));

            var expectedMessage = $"{{\"_facility\":\"Test\",\"short_message\":\"short_message\",\"host\":\"magic\",\"level\":7,\"_logger_name\":\"SimpleMessageTest\",\"longstring\":\"{new string('*', 16383)}\",\"timestamp\":10.005,\"version\":\"1.1\"}}";

            Assert.Equal(expectedMessage, testMessage);
        }
    }
}
