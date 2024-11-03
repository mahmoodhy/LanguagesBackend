using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class BoxData
    {
        public int id { get; set; }
        public string EnglishWord { get; set; }
        public string PersianWords { get; set; }

        public string? Type { get; set; }
        public int Priority { get; set; }
        public int? Rank { get; set; }

    }
}
