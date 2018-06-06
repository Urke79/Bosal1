using System.Runtime.Serialization;

namespace Domain
{
    [DataContract]
    public class JobsCountForSpecificDate
    {
        [DataMember]
        public string Date { get; set; }
        [DataMember]
        public int Count { get; set; }
    }
}
