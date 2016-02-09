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
        public IActionResult Get()
        {
            return Ok(new
            {
                items = _RecordRepository.GetAll() ?? new Record[] {}
            });

        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var record = _RecordRepository.Get(id);

            if (record == null)
            {
                return HttpNotFound();
            }

            return Ok(record);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Record record)
        {
            if (record == null)
            {
                return HttpBadRequest("No record supplied.");
            }

            if (string.IsNullOrWhiteSpace(record.Id))
            {
                return HttpBadRequest("Record Id not supplied.");
            }

            //if (_Records.ContainsKey(record.Id))
            //{
            //    Response.StatusCode = 409;
            //    return new { message = string.Format("Record with Id '{0}' already exists.", record.Id) };
            //}

            _RecordRepository.Add(record);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]Record record)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpBadRequest("Record Id not supplied.");
            }

            if (record == null)
            {
                return HttpBadRequest("No record supplied.");
            }

            if (string.IsNullOrWhiteSpace(record.Id))
            {
                return HttpBadRequest("Invalid record Id.");
            }

            id = id.Trim();

            //if (!_Records.ContainsKey(id))
            //{
            //    Response.StatusCode = 404;
            //    return new { message = "Record not found." };
            //}

            _RecordRepository.Update(id, record);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpBadRequest("Record Id not supplied.");
            }

            //if (!_Records.ContainsKey(id))
            //{
            //    Response.StatusCode = 404;
            //    return new { message = "Record not found." };
            //}

            _RecordRepository.Delete(id);
            return Ok();
        }
    }

}
