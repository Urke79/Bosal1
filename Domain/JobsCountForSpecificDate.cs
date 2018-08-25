using System.Runtime.Serialization;

namespace Domain
{
    public class JobsCountForSpecificDate
    {
        public string Date { get; set; }
        public int Count { get; set; }
    }
}
