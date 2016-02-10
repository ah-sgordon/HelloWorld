using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using System.Linq;
using HelloWorld.Models;
using HelloWorld.Repositories;
using System.Threading.Tasks;
using System;

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
        public async Task<IActionResult> Get()
        {
            return Ok(new
            {
                items = await _RecordRepository.GetAll() ?? new Record[] { }
            });

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var record = await _RecordRepository.Get(id);

            if (record == null)
            {
                return HttpNotFound();
            }

            return Ok(record);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Record record)
        {
            if (record == null)
            {
                return HttpBadRequest("No record supplied.");
            }

            if (!string.IsNullOrWhiteSpace(record.Id))
            {
                return HttpBadRequest("Record Id is assigned automatically.  Do not provide a value.");
            }

            record.Id = Guid.NewGuid().ToString();
            await _RecordRepository.Add(record);
            return Ok(record);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]Record record)
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

            await _RecordRepository.Update(id, record);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
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

            await _RecordRepository.Delete(id);
            return Ok();
        }
    }

}
