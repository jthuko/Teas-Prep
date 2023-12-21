using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MblexApp.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<string> Choices { get; set; }
        public int CorrectChoiceIndex { get; set; }
        public string CorrectAnswer { get; set; }
        public bool IsAnswered { get; set; }
        

        // Reference to the owning user
        public int? UserID { get; set; }
        public UserModel User { get; set; }

        // Reference to the subject (assuming it's defined elsewhere)
        public int SubjectId { get; set; }
        public SubjectModel Subject { get; set; }

        // Indicates whether the question is public or private
        public bool IsPublic { get; set; }
        public Question(string text, List<string> choices, string correctAnswer, int correctChoiceIndex,int? userID,bool isPublic,int subjectID)
        {
            Text = text;
            Choices = choices;
            CorrectAnswer = correctAnswer;
            IsAnswered = false;
            CorrectChoiceIndex = correctChoiceIndex;
            IsPublic = isPublic;
            UserID = userID;
            SubjectId = subjectID;
        }
        private bool isCorrectAnswer;
        public bool IsCorrectAnswer
        {
            get => isCorrectAnswer;
            set
            {
                if (isCorrectAnswer != value)
                {
                    isCorrectAnswer = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
