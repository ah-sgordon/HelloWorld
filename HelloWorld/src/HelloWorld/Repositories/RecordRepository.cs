using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using HelloWorld.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelloWorld.Repositories
{
    public interface IRecordRepository
    {
        Task<IEnumerable<Record>> GetAll();

        Task<Record> Get(string id);

        Task Add(Record record);

        Task Update(string id, Record record);

        Task Delete(string id);
    }

    public class RecordRepository : IRecordRepository
    {
        private AmazonDynamoDBClient _DynamoDbClient;
        private DynamoDBContext _DynamoDbContext;

        public RecordRepository()
        {
            _DynamoDbClient = new AmazonDynamoDBClient(new StoredProfileAWSCredentials(), AWSConfigs.RegionEndpoint);
            _DynamoDbContext = new DynamoDBContext(_DynamoDbClient, new DynamoDBContextConfig { Conversion = DynamoDBEntryConversion.V2, TableNamePrefix = "HelloWorld." });
        }

        public async Task Add(Record record)
        {
            await _DynamoDbContext.SaveAsync(record);
 
            //TODO: Figure out how to add a condition to prevent save if record already exists
            //await _DynamoDbContext.SaveAsync(record, new DynamoDBOperationConfig { QueryFilter = new List<ScanCondition> { new ScanCondition("Id", Amazon.DynamoDBv2.DocumentModel.ScanOperator.  ) } );
        }

        public async Task Delete(string id)
        {
            await _DynamoDbContext.DeleteAsync<Record>(id);
        }

        public async Task<Record> Get(string id)
        {
            return await _DynamoDbContext.LoadAsync<Record>(id);
        }

        public async Task<IEnumerable<Record>> GetAll()
        {
            var returnValue = new List<Record>();
            var search = _DynamoDbContext.ScanAsync<Record>(new List<ScanCondition>());
            foreach (var record in await search.GetRemainingAsync())
            {
                returnValue.Add(record);
            }

            return returnValue;
        }

        public async Task Update(string id, Record record)
        {
            await _DynamoDbContext.SaveAsync(record);

            //TODO: Figure out how to add a condition to allow save only if record already exists
            //await _DynamoDbContext.SaveAsync(record, new DynamoDBOperationConfig { QueryFilter = new List<ScanCondition> { new ScanCondition("Id", Amazon.DynamoDBv2.DocumentModel.ScanOperator.  ) } );
        }
    }
}
