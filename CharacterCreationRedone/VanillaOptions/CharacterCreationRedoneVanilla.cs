using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace CharacterCreationRedone.VanillaOptions
{
    [HarmonyPatch(typeof(CharacterCreationCampaignBehavior), nameof(CharacterCreationCampaignBehavior.InitializeData))]
    public class InitializeDataPatch
    {
        // The vanilla options are excluded from the project and are intended for modders to use in their mod with losing countless hours everytime.
        // these files are drag an drop and should work as is, but require the war sails DLC starting from now on. look for older commits to get the non DLC version
        // The XMLs in this mod however are the one, well, for this mod. You need to use the vanilla unsorted mess ones
        [HarmonyPrefix]
        static bool Prefix(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.ChangeReviewPageDescription(new TextObject("{=W6pKpEoT}You prepare to set off for a grand adventure in Calradia! Here is your character. Continue if you are ready, or go back to make changes.", null));
            new CharacterCreationRedoneVanillaParentsMenu().AddParentsMenu(characterCreationManager);
            new CharacterCreationRedoneVanillaChildhoodMenu().AddChildhoodMenu(characterCreationManager);
            new CharacterCreationRedoneVanillaEducationMenu().AddEducationMenu(characterCreationManager);
            new CharacterCreationRedoneVanillaYouthMenu().AddYouthMenu(characterCreationManager);
            new CharacterCreationRedoneVanillaAdulthoodMenu().AddAdulthoodMenu(characterCreationManager);
            new CharacterCreationRedoneVanillaAgeMenu().AddAgeSelectionMenu(characterCreationManager);
            return false;
        }
    }
    [HarmonyPatch(typeof(CharacterCreationCampaignBehavior), nameof(CharacterCreationCampaignBehavior.InitializeCharacterCreationCultures))]
    public class InitializeCharacterCreationCulturesPatch
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