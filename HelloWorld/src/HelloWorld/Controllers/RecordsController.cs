using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using System.Linq;
using HelloWorld.Models;
using HelloWorld.Repositories;

namespace HelloWorld.Controllers
{
    [Route("[controller]")]
    public class RecordsController : Controller
    {
        private IRecordRepository _RecordRepository;

        public RecordsController(IRecordRepository recordRepository)
        {
            _RecordRepository = recordRepository;
        }

        [HttpGet]
        public object Get()
        {
            return new
            {
                items = _RecordRepository.GetAll() ?? new Record[] {}
            };
        }

        [HttpGet("{id}")]
        public object Get(string id)
        {
            var record = _RecordRepository.Get(id);

            if (record == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return record;
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

            //if (_Records.ContainsKey(record.Id))
            //{
            //    Response.StatusCode = 409;
            //    return new { message = string.Format("Record with Id '{0}' already exists.", record.Id) };
            //}

            _RecordRepository.Add(record);
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

            //if (!_Records.ContainsKey(id))
            //{
            //    Response.StatusCode = 404;
            //    return new { message = "Record not found." };
            //}

            _RecordRepository.Update(id, record);
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

            //if (!_Records.ContainsKey(id))
            //{
            //    Response.StatusCode = 404;
            //    return new { message = "Record not found." };
            //}

            _RecordRepository.Delete(id);
            return null;
        }
    }

}
