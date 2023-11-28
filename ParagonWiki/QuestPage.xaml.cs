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
        giver.Text = "A quest given by " + quest.QuestGiver;
		rewards.ItemsSource = quest.QuestReward;
        acquiredRewards.ItemsSource = quest.acquiredRewards;

        if (quest.QuestReward == null)
        {
            rewardSection.Clear();
            rewardSection.Children.Add(new Label
            {
                Text = "<i>" + quest.QuestGiver + " does not offer any reward for this quest.</i>",
                TextType = TextType.Html,
                FontSize = 15
            });
        }
        if (quest.acquiredRewards.Count == 0)
        {
            acquiredRewardSection.Clear();
            acquiredRewardSection.Children.Add(new Label
            {
                Text = "<i>No item has been rewarded before this quest.</i>",
                TextType = TextType.Html,
                FontSize =15
            });
        }
    }

    private async void rewards_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        ListView lv = sender as ListView;
        string rewardName = string.Empty;

        // styleID is believed to be the name of the MAUI control
        if (lv.StyleId == "acquiredRewards")
        {
            rewardName = (acquiredRewards.SelectedItem as Item).Name;
        } else if (lv.StyleId == "rewards")
        {
            rewardName = (rewards.SelectedItem as Item).Name;
        }
       
        Item reward = (from item in MainPage.Searchables where item.Name.Equals(rewardName, StringComparison.OrdinalIgnoreCase) select item).First() as Item;
        if (reward == null)
        {
            return;
        }
        await Navigation.PushAsync(new DescriptionPage(reward), true);
    }
}