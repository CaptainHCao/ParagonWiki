
namespace ParagonWiki;

public partial class AboutPage : ContentPage
{
	public AboutPage()
	{
		InitializeComponent();
		InitializePersonalInfo();
	}

	private void InitializePersonalInfo()
	{
		image.Source = "indie_portrait.png";
		Name.Text = "Hiep Cao";
		Description.Text = "Software Engineer / Game developer";
		Quote.Text = "\"Shoot for the Moon. Even if you miss, you\'ll lang among the stars\" - Norman Vincent Peale";
        ContactInfo.Text = "For any inquiries, please contact <i><b>caoduchiep@gmail.com</b></i>";
    }

    private void button_clicked(object sender, EventArgs e)
    {
		string buttonName = (sender as Button).StyleId;
		switch (buttonName)
		{
			case "Portfolio":
				webview.Source = "https://caoduchiep.myportfolio.com/";
				break;
			case "Development_logs":
				webview.Source = "https://www.youtube.com/channel/UCLxKzHyi1dJausMT6FBRoxA";
                break;
        }
    }
}