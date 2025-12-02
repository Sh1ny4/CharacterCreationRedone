using CharacterCreationRedone.SandboxOptions;
using HarmonyLib;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Localization;

namespace CharacterCreationRedone.CharacterCreationOptions
{
    [HarmonyPatch(typeof(CharacterCreationCampaignBehavior), nameof(CharacterCreationCampaignBehavior.InitializeData))]
    public class CharacterCreationRedoneSandbox : CharacterCreationCampaignBehavior, ICharacterCreationContentHandler
    {
        [HarmonyPrefix]
        static bool Prefix(ref CharacterCreationRedoneSandbox __instance, CharacterCreationManager characterCreationManager)
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
}
