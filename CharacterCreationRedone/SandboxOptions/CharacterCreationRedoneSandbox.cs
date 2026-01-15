using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace CharacterCreationRedone.SandboxOptions
{
    [HarmonyPatch(typeof(CharacterCreationCampaignBehavior), nameof(CharacterCreationCampaignBehavior.InitializeData))]
    public class CharacterCreationRedoneSandboxMenus : CharacterCreationCampaignBehavior, ICharacterCreationContentHandler
    {
        [HarmonyPrefix]
        static bool Prefix(ref CharacterCreationRedoneSandboxMenus __instance, CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.ChangeReviewPageDescription(new TextObject("{=W6pKpEoT}You prepare to set off for a grand adventure in Calradia! Here is your character. Continue if you are ready, or go back to make changes.", null));
            var parentsmenu = new CharacterCreationRedoneSandboxParentsMenu();
            var educationmenu = new CharacterCreationRedoneSandboxEducationMenu();
            var idiomsmenu = new CharacterCreationRedoneSandboxIdiomMenu();
            var youthmenu = new CharacterCreationRedoneSandboxYouthMenu();
            var reasonmenu = new CharacterCreationRedoneSandboxReasonMenu();
            var agemenus = new CharacterCreationRedoneSandboxAgeMenu();
            parentsmenu.ParentsMenu(characterCreationManager);
            educationmenu.EducationMenu(characterCreationManager);
            idiomsmenu.FavoriteIdiomMenu(characterCreationManager);
            youthmenu.StartInLifeMenu(characterCreationManager);
            reasonmenu.ReasonMenu(characterCreationManager);
            agemenus.AgeSelectionMenu(characterCreationManager);
            return false;
        }
    }
    [HarmonyPatch(typeof(CharacterCreationCampaignBehavior), nameof(CharacterCreationCampaignBehavior.InitializeCharacterCreationCultures))]
    public class CharacterCreationRedoneSandboxCulture
    {
        [HarmonyPostfix]
        static void PostFix(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.AddCharacterCreationCulture(Game.Current.ObjectManager.GetObject<CultureObject>("nord"), 1, 30);
        }
    }
}
