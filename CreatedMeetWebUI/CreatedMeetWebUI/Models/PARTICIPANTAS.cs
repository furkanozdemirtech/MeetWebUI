using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CreatedMeetWebUI.Models
{
    public class PARTICIPANTAS
    {
        [Key, Column(Order = 0)]
        public int USER_ID { get; set; }
        [Key, Column(Order = 1)]
        public int MEET_ID { get; set; }
    }
}
