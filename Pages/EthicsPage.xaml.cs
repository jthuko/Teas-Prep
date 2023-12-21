
using TeasPrep.Models;
using TeasPrep.ViewModel;
namespace TeasPrep;

public partial class EthicsPage : ContentPage
{
    private readonly string connectionString = "Server=tcp:jtappserver.database.windows.net,1433;Initial Catalog=MblexDB;Persist Security Info=False;User ID=jthuko;Password=Jnzusyo77!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    private readonly EthicsViewModel viewModel;
    public EthicsPage()
    {
        InitializeComponent();

        // Resolve dependencies using DependencyService
        var questionService = new AppService(connectionString);
        viewModel = new EthicsViewModel(questionService);
        BindingContext = viewModel;

    }
    private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && e.Value)
        {
            string selectedValue = (string)radioButton.Value;
            PublicQuestion selectedQuestion = radioButton.BindingContext as PublicQuestion;

            if (selectedQuestion != null)
            {
                // Find the choice that corresponds to the selected answer
                Choice selectedChoice = selectedQuestion.Choices.FirstOrDefault(choice => choice.ChoiceText == selectedValue);

                if (selectedChoice != null)
                {
                    // Check if the selected choice is marked as correct
                    bool isCorrect = selectedChoice.IsCorrect;
                    // Change the text color of the radiobutton based on the answer
                    if (isCorrect)
                    {
                        Color green = new Color(0, 255, 0, 255);
                        radioButton.TextColor = green;
                    }
                    else
                    {
                        Color red = new Color(255, 0, 0, 255);
                        radioButton.TextColor = red;
                    }

                }
            }
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        // Clear the text color of all RadioButtons
        ClearRadioButtonTextColors();
        // Update the ViewModel to reload the list or reset properties
        viewModel.ReloadQuestions(); // Assuming you have a method like this in your ViewModel
    }
    private void ClearRadioButtonTextColors()
    {
        // Iterate through the RadioButtons in the layout and reset text color
        foreach (var radioButton in QuestionsListView.GetVisualTreeDescendants().Where(c => c is RadioButton))
        {
            if (radioButton is RadioButton rb)
            {
                rb.TextColor = Color.FromRgba(0, 0, 0, 255);
                // Uncheck the radio button
                rb.IsChecked = false;
            }
        }
    }

}

