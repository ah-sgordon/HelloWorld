using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using System.Linq;
using HelloWorld.Models;

namespace HelloWorld.Controllers
{
    [Route("[controller]")]
    public class RecordsController : Controller
    {
        private static Dictionary<string, Record> _Records = new Dictionary<string, Record>
        {
            { "AAA", new Record { Id = "AAA", Value="Value 1" } },
            { "BBB", new Record { Id = "BBB", Value="Value 1" } }
        };

        [HttpGet]
        public object Get()
        {
            return new
            {
                items = _Records.Values.ToList()
            };
        }

        [HttpGet("{id}")]
        public object Get(string id)
        {
            Record record;
            if (!_Records.TryGetValue(id, out record))
            {
                Response.StatusCode = 404;
                return null;
            }

            return new { item = record };
        }

        [HttpPost]
        public object Post([FromBody]Record record)
        {
            if (record == null)
            {
                Response.StatusCode = 400;
                return new { message = "No record supplied." };
            }

            if (string.IsNullOrWhiteSpace(record.Id))
            {
                Response.StatusCode = 400;
                return new { message = "Record Id not supplied." };
            }

            record.Id = record.Id.Trim();

            if (_Records.ContainsKey(record.Id))
            {
                Response.StatusCode = 409;
                return new { message = string.Format("Record with Id '{0}' already exists.", record.Id) };
            }

            _Records.Add(record.Id, record);
            return null;
        }

        [HttpPut("{id}")]
        public object Put(string id, [FromBody]Record record)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                Response.StatusCode = 400;
                return new { message = "Record Id not supplied." };
            }

            if (record == null)
            {
                Response.StatusCode = 400;
                return new { message = "No record supplied." };
            }

            if (string.IsNullOrWhiteSpace(record.Id))
            {
                Response.StatusCode = 400;
                return new { message = "Invalid record Id." };
            }

            id = id.Trim();

            if (!_Records.ContainsKey(id))
            {
                Response.StatusCode = 404;
                return new { message = "Record not found." };
            }

            _Records[id] = record;
            return null;
        }

        [HttpDelete("{id}")]
        public object Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                Response.StatusCode = 400;
                return new { message = "Record Id not supplied." };
            }

            id = id.Trim();

            if (!_Records.ContainsKey(id))
            {
                Response.StatusCode = 404;
                return new { message = "Record not found." };
            }

            _Records.Remove(id);
            return null;
        }
    }

}
