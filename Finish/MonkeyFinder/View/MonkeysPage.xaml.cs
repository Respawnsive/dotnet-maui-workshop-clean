namespace MonkeyFinder.View;

public partial class MonkeysPage : ContentPage
{
	public MonkeysPage(MonkeysViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}

