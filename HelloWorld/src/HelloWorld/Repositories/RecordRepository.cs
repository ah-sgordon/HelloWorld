using HelloWorld.Models;
using System.Collections.Generic;

namespace HelloWorld.Repositories
{
    public interface IRecordRepository
    {
        IEnumerable<Record> GetAll();

        Record Get(string id);

        void Add(Record record);

        void Update(string id, Record record);

        void Delete(string id);
    }

    public class RecordRepository : IRecordRepository
    {
        private Dictionary<string, Record> _Records = new Dictionary<string, Record>
        {
            { "AAA", new Record { Id = "AAA", Value="Value 1" } },
            { "BBB", new Record { Id = "BBB", Value="Value 2" } }
        };


        public void Add(Record record)
        {
            record.Id = record.Id.Trim();

            lock (_Records)
            {
                _Records.Add(record.Id, record);
            }
        }

        public void Delete(string id)
        {
            id = id.Trim();

            lock (_Records)
            {
                _Records.Remove(id);
            }
        }

        public Record Get(string id)
        {
            Record returnValue;
            return _Records.TryGetValue(id, out returnValue) ? returnValue : null;
        }

        public IEnumerable<Record> GetAll()
        {
            return _Records.Values;
        }

        public void Update(string id, Record record)
        {
            id = id.Trim();
            record.Id = record.Id.Trim();

            lock (_Records)
            {
                if (!_Records.ContainsKey(id.Trim()))
                {
                    return;
                }

                _Records[id] = record;
            }
        }
    }
}
