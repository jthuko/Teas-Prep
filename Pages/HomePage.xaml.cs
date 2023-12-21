namespace MblexApp;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}
    private void OnQuestionsCardTapped(object sender, EventArgs e)
    {
        // Navigate to PublicQuestionsPage
        Navigation.PushAsync(new PublicQuestionsPage());
    }

    private void OnFlashcardsCardTapped(object sender, EventArgs e)
    {
        // Navigate to PublicFlashcardsPage
        Navigation.PushAsync(new PublicFlashcardPage());
    }
}