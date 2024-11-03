using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class UserBox
    {
        public int Id { get; set; }

        public string userName { get; set; }

        public int BoxId { get; set; }
        public int BoxDay { get; set; }

        public DateTime LearnDate { get; set; }
        public string? YourAnswer { get; set; }
        public string? YourExample { get; set; }
        public int? working { get; set; }
        //public string? Answer { get; set; }
        
        public int WrongAnswers {  get; set; }
        public bool Priority {  get; set; }   



    }
}
