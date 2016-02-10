using HelloWorld.Controllers;
using Xunit;
using Newtonsoft.Json;
using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;
using HelloWorld.Repositories;
using Moq;

namespace HelloWorld.Tests
{
    public class RecordsControllerTests
    {
        [Fact]
        [Trait("Type", "Unit")]
        [Trait("HappyPath", "true")]
        public async void GetAllReturnsTwoResults()
        {
            var records = new[]
            {
                new Models.Record {  Id = "1", Value = "Value 1" },
                new Models.Record {  Id = "2", Value = "Value 2" }
            };

            var mockRecordRepository = new Mock<IRecordRepository>();
            mockRecordRepository
                .Setup(p => p.GetAll())
                .Returns(async () =>
                {
                    await Task.Yield();
                    return records;
                });
            var recordRepository = mockRecordRepository.Object;
            var target = new RecordsController(recordRepository);

            var result = await target.Get();

            Assert.IsAssignableFrom<ObjectResult>(result);

            var expected = JsonConvert.SerializeObject(new { items = records });
            Assert.Equal(expected, JsonConvert.SerializeObject(((ObjectResult)result).Value));
        }

        [Fact]
        [Trait("Type", "Unit")]
        public async void GetAllReturnsNoResultsWhenRepositoryIsEmpty()
        {
            var records = new Models.Record[] { };

            var mockRecordRepository = new Mock<IRecordRepository>();
            mockRecordRepository
                .Setup(p => p.GetAll())
                .Returns(async () =>
                {
                    await Task.Yield();
                    return records;
                });
            var recordRepository = mockRecordRepository.Object;
            var target = new RecordsController(recordRepository);

            var result = await target.Get();

            Assert.IsAssignableFrom<ObjectResult>(result);
            var expected = JsonConvert.SerializeObject(new { items = records });
            Assert.Equal(expected, JsonConvert.SerializeObject(((ObjectResult)result).Value));
        }

        [Fact]
        [Trait("Type", "Unit")]
        public async void GetAllReturnsNoResultsWhenRepositoryReturnsNull()
        {
            Models.Record[] records = null;
            var mockRecordRepository = new Mock<IRecordRepository>();
            mockRecordRepository
                .Setup(p => p.GetAll())
                .Returns(async () =>
                {
                    await Task.Yield();
                    return records;
                });
            var recordRepository = mockRecordRepository.Object;
            var target = new RecordsController(recordRepository);

            var result = await target.Get();

            Assert.IsAssignableFrom<ObjectResult>(result);
            var expected = JsonConvert.SerializeObject(new { items = new Models.Record[] { } });
            Assert.Equal(expected, JsonConvert.SerializeObject(((ObjectResult)result).Value));
        }

        [Fact]
        [Trait("Type", "Unit")]
        [Trait("HappyPath", "true")]
        public async void GetByIdReturnsMatch()
        {
            var record = new Models.Record { Id = "1", Value = "Value 1" };
            var recordRepositoryMock = new Mock<IRecordRepository>();
            recordRepositoryMock
                .Setup(p => p.Get(It.Is<string>(s => s == "1")))
                .Returns(async () =>
                {
                    await Task.Yield();
                    return record;
                });
            var recordRepository = recordRepositoryMock.Object;
            var target = new RecordsController(recordRepository);

            var result = await target.Get("1");

            Assert.IsAssignableFrom<ObjectResult>(result);
            var expected = JsonConvert.SerializeObject(record);
            Assert.Equal(expected, JsonConvert.SerializeObject(((ObjectResult)result).Value));
        }


        [Fact]
        [Trait("Type", "Unit")]
        public async void GetByIdReturnsNotFoundWhenNoMatchExists()
        {
            Models.Record record = null;
            var mockRecordRepository = new Mock<IRecordRepository>();
            mockRecordRepository
                .Setup(p => p.Get("2"))
                .Returns(async () =>
                {
                    await Task.Yield();
                    return record;
                });
            var recordRepository = mockRecordRepository.Object;
            var target = new RecordsController(recordRepository);

            var result = await target.Get("2");

            Assert.IsAssignableFrom<HttpNotFoundResult>(result);
        }

        [Fact]
        [Trait("Type", "Unit")]
        public async void PostReturnsBadRequestWhenNoRecordProvided()
        {
            var mockRecordRepository = new Mock<IRecordRepository>();
            var recordRepository = mockRecordRepository.Object;
            var target = new RecordsController(recordRepository);

            var result = await target.Post(null);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.Equal("No record supplied.", ((BadRequestObjectResult)result).Value);
        }

        [Fact]
        [Trait("Type", "Unit")]
        public async void PostReturnsBadRequestWhenIdProvided()
        {
            var mockRecordRepository = new Mock<IRecordRepository>();
            var recordRepository = mockRecordRepository.Object;
            var target = new RecordsController(recordRepository);

            var result = await target.Post(new Models.Record { Id = "123", Value = "This record has no Id." });

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.Equal("Record Id is assigned automatically.  Do not provide a value.", ((BadRequestObjectResult)result).Value);
        }


        [Fact]
        [Trait("Type", "Unit")]
        [Trait("HappyPath", "true")]
        public async void PostAddsRecord()
        {
            var mockRecordRepository = new Mock<IRecordRepository>();
            mockRecordRepository
                .Setup(p => p.Add(It.IsAny<Models.Record>()))
                .Returns(async () =>
                {
                    await Task.Yield();
                });
            var recordRepository = mockRecordRepository.Object;
            var target = new RecordsController(recordRepository);

            var recordToPost = new Models.Record { Id = "", Value = "This is a good record." };
            var result = await target.Post(recordToPost);

            Assert.IsAssignableFrom<HttpOkObjectResult>(result);

            var record = ((HttpOkObjectResult)result).Value as Models.Record;

            Assert.NotNull(record);
            Assert.False(string.IsNullOrWhiteSpace(record.Id), "Record Id should not be null.");
            Assert.Equal(recordToPost.Value, record.Value);
        }

        [Fact]
        [Trait("Type", "Unit")]
        public async void PutReturnsBadRequestWhenNoIdProvided()
        {
            var mockRecordRepository = new Mock<IRecordRepository>();
            var recordRepository = mockRecordRepository.Object;
            var target = new RecordsController(recordRepository);

            var result = await target.Put("  ", new Models.Record { Id = "ABC", Value = "This is a good record." });

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.Equal("Record Id not supplied.", ((BadRequestObjectResult)result).Value);
        }

        [Fact]
        [Trait("Type", "Unit")]
        public async void PutReturnsBadRequestWhenNoRecordProvided()
        {
            var mockRecordRepository = new Mock<IRecordRepository>();
            var recordRepository = mockRecordRepository.Object;
            var target = new RecordsController(recordRepository);

            var result = await target.Put("ABC", null);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.Equal("No record supplied.", ((BadRequestObjectResult)result).Value);
        }

        [Fact]
        [Trait("Type", "Unit")]
        public async void PutReturnsBadRequestWhenRecordHasNoId()
        {
            var mockRecordRepository = new Mock<IRecordRepository>();
            var recordRepository = mockRecordRepository.Object;
            var target = new RecordsController(recordRepository);

            var result = await target.Put("ABC", new Models.Record { Id = "   ", Value = "This record has no Id." });

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.Equal("Invalid record Id.", ((BadRequestObjectResult)result).Value);
        }

        [Fact]
        [Trait("Type", "Unit")]
        [Trait("HappyPath", "true")]
        public async void PutUpdatesRecord()
        {
            var mockRecordRepository = new Mock<IRecordRepository>();
            var recordRepository = mockRecordRepository.Object;
            var target = new RecordsController(recordRepository);

            var result = await target.Put("ABC", new Models.Record { Id = "ABC", Value = "This is an updated record." });

            Assert.IsAssignableFrom<HttpOkResult>(result);

            mockRecordRepository.Verify(
                p => p.Update("ABC", It.Is<Models.Record>(q => q.Id == "ABC" && q.Value == "This is an updated record.")), 
                Times.Once);
        }

        [Fact]
        [Trait("Type", "Unit")]
        public async void DeleteReturnsBadRequestWhenNoIdProvided()
        {
            var mockRecordRepository = new Mock<IRecordRepository>();
            var recordRepository = mockRecordRepository.Object;
            var target = new RecordsController(recordRepository);

            var result = await target.Delete("   ");

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.Equal("Record Id not supplied.", ((BadRequestObjectResult)result).Value);
        }

        [Fact]
        [Trait("Type", "Unit")]
        [Trait("HappyPath", "true")]
        public async void DeleteDeletesRecord()
        {
            var mockRecordRepository = new Mock<IRecordRepository>();
            var recordRepository = mockRecordRepository.Object;
            var target = new RecordsController(recordRepository);

            var result = await target.Delete("ABC");

            Assert.IsAssignableFrom<HttpOkResult>(result);

            mockRecordRepository.Verify(
                p => p.Delete("ABC"),
                Times.Once());
        }
    }
}
