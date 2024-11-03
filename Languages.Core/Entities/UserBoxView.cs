using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class UserBoxView
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; } = null!;

        public int BoxId { get; set; }

        public int BoxDay { get; set; }

        public DateTime LearnDate { get; set; }

        public string? YourAnswer { get; set; }

        public string? YourExample { get; set; }

        public int? Working { get; set; }

        public bool Priority { get; set; }

        //public string? Answer { get; set; }

        public int WrongAnswers { get; set; }

        public string EnglishWord { get; set; } = null!;

        public string PersianWords { get; set; } = null!;

        public string? UserId { get; set; }



    }
}
