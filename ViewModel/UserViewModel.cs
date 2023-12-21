using MblexApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace MblexApp.ViewModel
{
    public class UserViewModel : INotifyPropertyChanged
    {

        private readonly AppService appService;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<UserQuestion> UserQuestions { get; set; }
        public int UserId { get; private set; }

        private bool isAnswerCorrect;
        public bool IsAnswerCorrect
        {
            get { return isAnswerCorrect; }
            set
            {
                if (isAnswerCorrect != value)
                {
                    isAnswerCorrect = value;
                    OnPropertyChanged(nameof(IsAnswerCorrect));
                }
            }
        }

        private void OnPropertyChanged(string v)
        {
            throw new NotImplementedException();
        }

        private UserQuestion  currentQuestion; // Keep track of the current question

        public ICommand SelectAnswerCommand { get; private set; }

        public UserViewModel(AppService appService)
        {
            this.appService = appService;
            UserQuestions = new ObservableCollection<UserQuestion>();
          //  LoadUserQuestions();
           
        }
       

        // Implement the logic to obtain the current question based on the selectedChoiceIndex
        private UserQuestion GetCurrentQuestion(int selectedChoiceIndex)
        {
            // Replace this with your actual logic to obtain the current question.
            // You might use the selectedChoiceIndex or other criteria to determine the current question.
            // Return the appropriate Question object or null if the current question is not found.
            // Example: 
            // return PublicQuestions.FirstOrDefault(q => q.Id == selectedChoiceIndex);

            // For demonstration purposes, returning the first question in the list.
            return UserQuestions.FirstOrDefault();
        }
       
        /*
        private void LoadUserQuestions()
        {
            // Load questions owned by the current user.
            UserQuestions.Clear();
           // var userQuestions = questionService.GetUserQuestions(UserId); // Replace UserId with the current user's ID.
          //  foreach (var question in userQuestions)
          //  {
          //      UserQuestions.Add(question);
           }
        }

        public void AddQuestion(Question question)
        {
            // Add a new question owned by the current user.
            question.UserID = UserId; // Set the user ID for the new question.
            questionService.AddQuestion(question);
            LoadUserQuestions(); // Reload the user's questions.
        }

        public void UpdateQuestion(Question question)
        {
            // Update a question owned by the current user.
            questionService.UpdateQuestion(question);
            LoadUserQuestions(); // Reload the user's questions.
        }

        public void DeleteQuestion(int questionId)
        {
            // Delete a question owned by the current user.
            questionService.DeleteQuestion(questionId);
            LoadUserQuestions(); // Reload the user's questions.
        }
        // Other properties and methods as needed
        */
    }
}
