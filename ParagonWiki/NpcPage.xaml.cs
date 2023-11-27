using ParagonWiki.Classes;
using System.Linq;

namespace ParagonWiki;

public partial class NpcPage : ContentPage
{
	public NpcPage(NPC npc)
	{
		InitializeComponent();
		NpcPageInitialize(npc);
	}
    public void NpcPageInitialize(NPC npc)
	{
		title.Text = npc.Name;
		description.Text = npc.Description;

        foreach (var dialogue in npc.dialogues)
		{
			string dialogueDetails = string.Empty;

			switch (dialogue.State)
			{
				case "NOT_YET_ACTIVATED":
					dialogueDetails += $"before accepting \"{getQuestName((int)dialogue.QuestID)}\" quest";
					break;
				case "ACTIVATED_BUT_NOT_DONE":
                    dialogueDetails += $"in progress of \"{getQuestName((int)dialogue.QuestID)}\" quest";
					break;
                case "DONE":
                    dialogueDetails += $"upon finishing \"{getQuestName((int)dialogue.QuestID)}\" quest";
                    break;
                case "IDLE":
                    dialogueDetails += $"idle dialogue";
                    break;
            }

			quotes.Children.Add(new Label
			{
				Text = dialogueDetails,
				FontAttributes = FontAttributes.Bold,
				FontSize = 20
			});

			foreach (var sentence in dialogue.Sentences)
			{
                quotes.Children.Add(new Label
                {
                    Text = sentence,
                    FontAttributes = FontAttributes.Italic,
                    FontSize = 15
                });
            }
        }
	}

	private static string getQuestName(int questID)
	{
		return (from item in MainPage.Searchables where item.Type == "Quest" && ((Quest)item).QuestID == questID select item.Name).First();
    }
}