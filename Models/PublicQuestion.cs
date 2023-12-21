using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TeasPrep.Models
{
    //Main public questions list
    public class PublicQuestion
    {
        public int QuestionID { get; set; }
        public string Text { get; set; }
        public bool IsPublic { get; set; }
        public int? UserID { get; set; }
        public int SubjectID { get; set; }
        public List<Choice> Choices { get; set; }
    }
}
