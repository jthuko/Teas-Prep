using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeasPrep.Models
{
    public class Choice
    {
        public int ChoiceID { get; set; }
        public int QuestionID { get; set; }
        public string ChoiceText { get; set; }
        public bool IsCorrect { get; set; }
    }
}
