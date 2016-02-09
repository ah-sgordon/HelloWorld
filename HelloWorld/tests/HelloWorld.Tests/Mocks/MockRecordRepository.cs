using System;
using System.Collections.Generic;
using HelloWorld.Models;
using HelloWorld.Repositories;
using LightMock;

namespace HelloWorld.Tests.Mocks
{
    public class MockRecordRepository : IRecordRepository
    {
        public MockRecordRepository() : this(new MockContext<IRecordRepository>())
        {
        }

        public MockContext<IRecordRepository> Context { get; private set; }

        private IInvocationContext<IRecordRepository> InvocationContext
        {
            get { return Context; }
        }

        public MockRecordRepository(MockContext<IRecordRepository> context)
        {
            Context = context;
        }

        public void Add(Record record)
        {
            InvocationContext.Invoke(p => p.Add(record));
        }

        public void Delete(string id)
        {
            InvocationContext.Invoke(p => p.Delete(id));
        }

        public Record Get(string id)
        {
            return InvocationContext.Invoke(p => p.Get(id));
        }

        public IEnumerable<Record> GetAll()
        {
            return InvocationContext.Invoke(p => p.GetAll());
        }

        public void Update(string id, Record record)
        {
            InvocationContext.Invoke(p => p.Update(id, record));
        }
    }
}
