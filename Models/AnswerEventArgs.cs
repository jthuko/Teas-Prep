using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeasPrep.Models
{
    public class AnswerEventArgs: EventArgs
    {
        public bool IsCorrect { get; }

        public AnswerEventArgs(bool isCorrect)
        {
            IsCorrect = isCorrect;
        }
    }
}
