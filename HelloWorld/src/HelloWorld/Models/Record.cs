using System.Collections.Generic;

namespace HelloWorld.Models
{
    public class Record
    {
        public string Id { get; set; }
        public string Value { get; set; }

        public List<string> CategoryIdList { get; set; }
    }
}
