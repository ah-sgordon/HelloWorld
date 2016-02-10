using HelloWorld.Repositories;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using Xunit;

namespace HelloWorld.Tests
{
    public class RecordRepositoryTests
    {
        public RecordRepositoryTests()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("config.json");

            var configuration = builder.Build();

            AwsConfigurator.ConfigureAws(configuration);
        }

        [Fact]
        [Trait("Type", "Integration")]
        public async void GetAll()
        {
            var target = new RecordRepository();
            var records = await target.GetAll();
        }


        [Fact]
        [Trait("Type", "Integration")]
        public async void GetById()
        {
            var target = new RecordRepository();
            var record = await target.Get("AAA");
        }

        [Fact]
        [Trait("Type", "Integration")]
        public async void Add()
        {
            var target = new RecordRepository();
            var record = new Models.Record
            {
                Id = Guid.NewGuid().ToString(),
                Value = "This value was added programmatically!"
            };
            await target.Add(record);

            var persistedRecord = await target.Get(record.Id);

            Assert.Equal(JsonConvert.SerializeObject(record), JsonConvert.SerializeObject(persistedRecord));

            await target.Delete(record.Id);
        }

        [Fact]
        [Trait("Type", "Integration")]
        public async void Update()
        {
            var target = new RecordRepository();
            var record = new Models.Record
            {
                Id = Guid.NewGuid().ToString(),
                Value = "This value was added programmatically!"
            };
            await target.Add(record);
            var persistedRecord = await target.Get(record.Id);
            Assert.Equal(JsonConvert.SerializeObject(record), JsonConvert.SerializeObject(persistedRecord));

            record.Value = "This value was UPDATED programmatically!";
            await target.Update(record.Id, record);

            persistedRecord = await target.Get(record.Id);
            Assert.Equal(JsonConvert.SerializeObject(record), JsonConvert.SerializeObject(persistedRecord));

            await target.Delete(record.Id);
        }

        [Fact]
        [Trait("Type", "Integration")]
        public async void Delete()
        {
            var target = new RecordRepository();
            var record = new Models.Record
            {
                Id = Guid.NewGuid().ToString(),
                Value = "This value was added programmatically!"
            };
            await target.Add(record);
            var persistedRecord = await target.Get(record.Id);
            Assert.Equal(JsonConvert.SerializeObject(record), JsonConvert.SerializeObject(persistedRecord));

            await target.Delete(record.Id);

            persistedRecord = await target.Get(record.Id);
            Assert.Null(persistedRecord);
        }
    }
}
