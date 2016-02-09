using HelloWorld.Controllers;
using HelloWorld.Tests.Mocks;
using Xunit;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HelloWorld.Tests
{
    public class RecordsControllerTests
    {
        [Fact]
        public void TestGetAllReturnsTwoResults()
        {
            var records = new[] 
            {
                new Models.Record {  Id = "1", Value = "Value 1" },
                new Models.Record {  Id = "2", Value = "Value 2" }
            };

            var recordRepository = new MockRecordRepository();
            recordRepository.Context
                .Arrange(p => p.GetAll())
                .Returns(records);
            var target = new RecordsController(recordRepository);

            var result = target.Get();

            var expected = JsonConvert.SerializeObject(new { items = records });
            Assert.Equal(expected, JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void TestGetAllEmptyReturnsNoResults()
        {
            var records = new Models.Record[] { };

            var recordRepository = new MockRecordRepository();
            recordRepository.Context
                .Arrange(p => p.GetAll())
                .Returns(records);
            var target = new RecordsController(recordRepository);

            var result = target.Get();

            var expected = JsonConvert.SerializeObject(new { items = records });
            Assert.Equal(expected, JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void TestGetAllNullReturnsNoResults()
        {
            Models.Record[] records = null;
            var recordRepository = new MockRecordRepository();
            recordRepository.Context
                .Arrange(p => p.GetAll())
                .Returns(records);
            var target = new RecordsController(recordRepository);

            var result = target.Get();

            var expected = JsonConvert.SerializeObject(new { items = new Models.Record[] { } });
            Assert.Equal(expected, JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void TestGetById()
        {
            var record = new Models.Record { Id = "1", Value = "Value 1" };

            var recordRepository = new MockRecordRepository();
            recordRepository.Context
                .Arrange(p => p.Get("1"))
                .Returns(record);
            var target = new RecordsController(recordRepository);

            var result = target.Get("1");

            var expected = JsonConvert.SerializeObject(record);
            Assert.Equal(expected, JsonConvert.SerializeObject(result));
        }
    }
}
