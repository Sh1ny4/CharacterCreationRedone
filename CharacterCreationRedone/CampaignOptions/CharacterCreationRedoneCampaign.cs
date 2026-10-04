using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Localization;
using StoryMode.GameComponents.CampaignBehaviors;
using StoryMode.StoryModeObjects;

namespace CharacterCreationRedone.CampaignOptions
{
    [HarmonyPatch(typeof(StoryModeCharacterCreationCampaignBehavior), nameof(StoryModeCharacterCreationCampaignBehavior.InitializeData))]
    public class CharacterCreationRedoneCampaign
    {
        [HarmonyPrefix]
        static bool Prefix(CharacterCreationManager characterCreationManager)
        {
            Hero.MainHero.Mother = StoryModeHeroes.MainHeroMother;
            Hero.MainHero.Father = StoryModeHeroes.MainHeroFather;
            characterCreationManager.CharacterCreationContent.ChangeReviewPageDescription(new TextObject("{=wbhKgpmr}You prepare to set off with your brother on a mission of vengeance and rescue. Here is your character. Continue if you are ready, or go back to make changes.", null));
            characterCreationManager.DeleteNarrativeMenuWithId("narrative_age_selection_menu");
            characterCreationManager.DeleteNarrativeMenuWithId("narrative_adulthood_menu");
            new CharacterCreationRedoneCampaignEscapeMenu().AddEscapeMenu(characterCreationManager);
            return false;        
        }
    }
}
