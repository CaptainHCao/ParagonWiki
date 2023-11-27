using ParagonWiki.Classes;
namespace ParagonWiki;

public partial class QuestPage : ContentPage
{
	public QuestPage(Quest quest)
	{
		InitializeComponent();
		InitializeQuestPage(quest);
	}

	private void InitializeQuestPage(Quest quest)
	{
        title.Text = quest.Name;
        description.Text = quest.Description;
		progress.Text = "Progress: " + quest.QuestID;

    }
}