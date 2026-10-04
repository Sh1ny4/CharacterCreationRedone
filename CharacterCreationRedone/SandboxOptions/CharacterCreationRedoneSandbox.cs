using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace CharacterCreationRedone.SandboxOptions
{
    [HarmonyPatch(typeof(CharacterCreationCampaignBehavior), nameof(CharacterCreationCampaignBehavior.InitializeData))]
    public class CharacterCreationRedoneSandboxMenus : CharacterCreationCampaignBehavior
    {
        [HarmonyPrefix]
        static bool Prefix(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.ChangeReviewPageDescription(new TextObject("{=W6pKpEoT}You prepare to set off for a grand adventure in Calradia! Here is your character. Continue if you are ready, or go back to make changes.", null));
            new CharacterCreationRedoneSandboxParentsMenu().ParentsMenu(characterCreationManager);
            new CharacterCreationRedoneSandboxEducationMenu().EducationMenu(characterCreationManager);
            new CharacterCreationRedoneSandboxIdiomMenu().FavoriteIdiomMenu(characterCreationManager);
            new CharacterCreationRedoneSandboxYouthMenu().StartInLifeMenu(characterCreationManager);
            new CharacterCreationRedoneSandboxReasonMenu().ReasonMenu(characterCreationManager);
            new CharacterCreationRedoneSandboxAgeMenu().AgeSelectionMenu(characterCreationManager);
            return false;
        }
    }
    [HarmonyPatch(typeof(CharacterCreationCampaignBehavior), nameof(CharacterCreationCampaignBehavior.InitializeCharacterCreationCultures))]
    public class InitializeCharacterCreationCulturesPatch : CharacterCreationCampaignBehavior
    {
        [HarmonyPrefix]
        static bool Prefix(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.AddCharacterCreationCulture(Game.Current.ObjectManager.GetObject<CultureObject>("aserai"), 1, 10);
            characterCreationManager.CharacterCreationContent.AddCharacterCreationCulture(Game.Current.ObjectManager.GetObject<CultureObject>("battania"), 1, 10);
            characterCreationManager.CharacterCreationContent.AddCharacterCreationCulture(Game.Current.ObjectManager.GetObject<CultureObject>("empire"), 1, 10);
            characterCreationManager.CharacterCreationContent.AddCharacterCreationCulture(Game.Current.ObjectManager.GetObject<CultureObject>("khuzait"), 1, 10);
            characterCreationManager.CharacterCreationContent.AddCharacterCreationCulture(Game.Current.ObjectManager.GetObject<CultureObject>("nord"), 1, 10);
            characterCreationManager.CharacterCreationContent.AddCharacterCreationCulture(Game.Current.ObjectManager.GetObject<CultureObject>("sturgia"), 1, 10);
            characterCreationManager.CharacterCreationContent.AddCharacterCreationCulture(Game.Current.ObjectManager.GetObject<CultureObject>("vlandia"), 1, 10);
            return false;
        }
    }
}
