namespace CreatedMeetWebUI.Models
{
    public class LOGS
    {
        public int ID { get; set; }
        public string DESCRIPTION { get; set; }
        public DateTime TIME { get; set; }
        public bool STATUS { get; set; }

        public DateTimeOffset Offset { get; set; }
    }
}
