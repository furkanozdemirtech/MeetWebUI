using System.ComponentModel.DataAnnotations.Schema;

namespace CreatedMeetWebUI.Models
{
    public class USER
    {
        public int ID { get; set; }

        public string EMAIL { get; set; }
        public bool EMAILCONFIRMED { get; set; }
        public string SECURRITYSTAMP { get; set; }
        public string PHONENUMBER { get; set; }

        public bool NUMBERCONFIRMED { get; set; }
        public string USERNAME { get; set; }

        public string NAME { get; set; }

        public string SURNAME { get; set; }
        public string PASSWORD { get; set; }
        [NotMapped]
        public bool JOINMEET { get; set; } = false;
    }
}
