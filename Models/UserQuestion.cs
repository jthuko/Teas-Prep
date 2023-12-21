using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MblexApp.Models
{
    public class UserQuestion
    {
        public int QuestionID { get; set; }
        public string Text { get; set; }
        public bool IsPublic { get; set; }
        public int? UserID { get; set; }
        public int SubjectID { get; set; }
        public List<Choice> Choices
        {
            get; set;
        }
    }
}
