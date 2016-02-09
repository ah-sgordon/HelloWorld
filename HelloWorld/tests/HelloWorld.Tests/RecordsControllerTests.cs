using HelloWorld.Controllers;
using HelloWorld.Tests.Mocks;
using Xunit;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using LightMock;

namespace HelloWorld.Tests
{
    public class RecordsControllerTests
    {
        [Fact]
        [Trait("Type", "Unit")]
        [Trait("HappyPath", "true")]
        public void GetAllReturnsTwoResults()
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

            Assert.IsAssignableFrom<ObjectResult>(result);

            var expected = JsonConvert.SerializeObject(new { items = records });
            Assert.Equal(expected, JsonConvert.SerializeObject(((ObjectResult)result).Value));
        }

        [Fact]
        [Trait("Type", "Unit")]
        public void GetAllReturnsNoResultsWhenRepositoryIsEmpty()
        {
            var records = new Models.Record[] { };

            var recordRepository = new MockRecordRepository();
            recordRepository.Context
                .Arrange(p => p.GetAll())
                .Returns(records);
            var target = new RecordsController(recordRepository);

            var result = target.Get();

            Assert.IsAssignableFrom<ObjectResult>(result);
            var expected = JsonConvert.SerializeObject(new { items = records });
            Assert.Equal(expected, JsonConvert.SerializeObject(((ObjectResult)result).Value));
        }

        [Fact]
        [Trait("Type", "Unit")]
        public void GetAllReturnsNoResultsWhenRepositoryReturnsNull()
        {
            Models.Record[] records = null;
            var recordRepository = new MockRecordRepository();
            recordRepository.Context
                .Arrange(p => p.GetAll())
                .Returns(records);
            var target = new RecordsController(recordRepository);

            var result = target.Get();

            Assert.IsAssignableFrom<ObjectResult>(result);
            var expected = JsonConvert.SerializeObject(new { items = new Models.Record[] { } });
            Assert.Equal(expected, JsonConvert.SerializeObject(((ObjectResult)result).Value));
        }

        [Fact]
        [Trait("Type", "Unit")]
        [Trait("HappyPath", "true")]
        public void GetByIdReturnsMatch()
        {
            var record = new Models.Record { Id = "1", Value = "Value 1" };

            var recordRepository = new MockRecordRepository();
            recordRepository.Context
                .Arrange(p => p.Get("1"))
                .Returns(record);
            var target = new RecordsController(recordRepository);

            var result = target.Get("1");

            Assert.IsAssignableFrom<ObjectResult>(result);
            var expected = JsonConvert.SerializeObject(record);
            Assert.Equal(expected, JsonConvert.SerializeObject(((ObjectResult)result).Value));
        }


        [Fact]
        [Trait("Type", "Unit")]
        public void GetByIdReturnsNotFoundWhenNoMatchExists()
        {
            Models.Record record = null;
            var recordRepository = new MockRecordRepository();
            recordRepository.Context
               .Arrange(p => p.Get("1"))
               .Returns(record);
            var target = new RecordsController(recordRepository);

            var result = target.Get("2");

            Assert.IsAssignableFrom<HttpNotFoundResult>(result);
        }

        [Fact]
        [Trait("Type", "Unit")]
        public void PostReturnsBadRequestWhenNoRecordProvided()
        {
            var recordRepository = new MockRecordRepository();
            var target = new RecordsController(recordRepository);

            var result = target.Post(null);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.Equal("No record supplied.", ((BadRequestObjectResult)result).Value);
        }

        [Fact]
        [Trait("Type", "Unit")]
        public void PostReturnsBadRequestWhenNoIdProvided()
        {
            var recordRepository = new MockRecordRepository();
            var target = new RecordsController(recordRepository);

            var result = target.Post(new Models.Record { Id = "  ", Value = "This record has no Id." });

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.Equal("Record Id not supplied.", ((BadRequestObjectResult)result).Value);
        }


        [Fact]
        [Trait("Type", "Unit")]
        [Trait("HappyPath", "true")]
        public void PostAddsRecord()
        {
            Models.Record record = null;

            var recordRepository = new MockRecordRepository();
            recordRepository.Context
                .Arrange(p => p.Add(The<Models.Record>.IsAnyValue))
                .Callback<Models.Record>(p => record = p);
            var target = new RecordsController(recordRepository);

            var recordToPost = new Models.Record { Id = "ABC", Value = "This is a good record." };
            var result = target.Post(recordToPost);

            Assert.IsAssignableFrom<HttpOkResult>(result);
            Assert.NotNull(record);
            Assert.Equal(JsonConvert.SerializeObject(recordToPost), JsonConvert.SerializeObject(record));
        }

        [Fact]
        [Trait("Type", "Unit")]
        public void PutReturnsBadRequestWhenNoIdProvided()
        {
            var recordRepository = new MockRecordRepository();
            var target = new RecordsController(recordRepository);

            var result = target.Put("  ", new Models.Record { Id = "ABC", Value = "This is a good record." });

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.Equal("Record Id not supplied.", ((BadRequestObjectResult)result).Value);
        }

        [Fact]
        [Trait("Type", "Unit")]
        public void PutReturnsBadRequestWhenNoRecordProvided()
        {
            var recordRepository = new MockRecordRepository();
            var target = new RecordsController(recordRepository);

            var result = target.Put("ABC", null);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.Equal("No record supplied.", ((BadRequestObjectResult)result).Value);
        }

        [Fact]
        [Trait("Type", "Unit")]
        public void PutReturnsBadRequestWhenRecordHashNoId()
        {
            var recordRepository = new MockRecordRepository();
            var target = new RecordsController(recordRepository);

            var result = target.Put("ABC", new Models.Record { Id = "   ", Value = "This record has no Id." });

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.Equal("Invalid record Id.", ((BadRequestObjectResult)result).Value);
        }

        [Fact]
        [Trait("Type", "Unit")]
        [Trait("HappyPath", "true")]
        public void PutUpdatesRecord()
        {
            var recordRepository = new MockRecordRepository();
            var target = new RecordsController(recordRepository);

            var result = target.Put("ABC", new Models.Record { Id = "ABC", Value = "This is an updated record." });

            Assert.IsAssignableFrom<HttpOkResult>(result);

            recordRepository.Context.Assert(
                p => p.Update("ABC", The<Models.Record>.Is(q => q.Id == "ABC" && q.Value == "This is an updated record.")), 
                Invoked.Exactly(1));
        }

        [Fact]
        [Trait("Type", "Unit")]
        public void DeleteReturnsBadRequestWhenNoIdProvided()
        {
            var recordRepository = new MockRecordRepository();
            var target = new RecordsController(recordRepository);

            var result = target.Delete("   ");

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.Equal("Record Id not supplied.", ((BadRequestObjectResult)result).Value);
        }

        [Fact]
        [Trait("Type", "Unit")]
        [Trait("HappyPath", "true")]
        public void DeleteDeletesRecord()
        {
            var recordRepository = new MockRecordRepository();
            var target = new RecordsController(recordRepository);

            var result = target.Delete("ABC");

            Assert.IsAssignableFrom<HttpOkResult>(result);

            recordRepository.Context.Assert(
                p => p.Delete("ABC"),
                Invoked.Exactly(1));
        }
    }
}
