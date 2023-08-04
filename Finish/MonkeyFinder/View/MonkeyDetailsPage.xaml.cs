namespace MonkeyFinder;

public partial class MonkeyDetailsPage : ContentPage
{
	public MonkeyDetailsPage(MonkeyDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}