using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Library;
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
            __instance.AddParentsMenu(characterCreationManager);
            __instance.AddEducationMenu(characterCreationManager);
            __instance.FavoriteIdiomMenu(characterCreationManager);
            __instance.StartInLifeMenu(characterCreationManager);
            __instance.AddReasonMenu(characterCreationManager);
            __instance.AddAgeSelectionMenu(characterCreationManager);
            return false;
        }

        public string GetMotherEquipmentId(CharacterCreationManager characterCreationManager, string occupationType, string cultureId)
        {
            characterCreationManager.CharacterCreationContent.TryGetEquipmentToUse(occupationType, out string str);
            return "mother_char_creation_" + str + "_" + cultureId;
        }

        public string GetFatherEquipmentId(CharacterCreationManager characterCreationManager, string occupationType, string cultureId)
        {
            characterCreationManager.CharacterCreationContent.TryGetEquipmentToUse(occupationType, out string str);
            return "father_char_creation_" + str + "_" + cultureId;
        }

        public string GetPlayerChildhoodAgeEquipmentId(CharacterCreationManager characterCreationManager, string parentOccupationType, string cultureId, bool isFemale)
        {
            characterCreationManager.CharacterCreationContent.TryGetEquipmentToUse(parentOccupationType, out string text);
            return string.Concat(new string[] { "player_char_creation_childhood_age_", cultureId, "_", text, "_", isFemale ? "f" : "m" });
        }

        public string GetPlayerEducationAgeEquipmentId(CharacterCreationManager characterCreationManager, string parentOccupationType, string cultureId, bool isFemale)
        {
            characterCreationManager.CharacterCreationContent.TryGetEquipmentToUse(parentOccupationType, out string text);
            return string.Concat(new string[] { "player_char_creation_education_age_", cultureId, "_", text, "_", isFemale ? "f" : "m" });
        }

        public string GetPlayerEquipmentId(CharacterCreationManager characterCreationManager, string occupationType, string cultureId, bool isFemale)
        {
            characterCreationManager.CharacterCreationContent.TryGetEquipmentToUse(occupationType, out string text);
            return string.Concat(new string[] { "player_char_creation_", cultureId, "_", text, "_", isFemale ? "f" : "m" });
        }


        /// <summary>
        /// Parents menu
        /// </summary>
        public List<NarrativeMenuCharacterArgs> GetParentMenuNarrativeMenuCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>
        {
            new NarrativeMenuCharacterArgs("mother_character", 33, "mother_char_creation_none_" + characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "act_character_creation_female_default_standing", "spawnpoint_player_1", "", "", null, true, true),
            new NarrativeMenuCharacterArgs("father_character", 33, "father_char_creation_none_" + characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "act_character_creation_male_default_standing", "spawnpoint_player_1", "", "", null, true, false)
        };
        }

        public void AddParentsMenu(CharacterCreationManager characterCreationManager)
        {
            List<NarrativeMenuCharacter> list = new List<NarrativeMenuCharacter>();
            BodyProperties bodyProperties2;
            BodyProperties bodyProperties;
            FaceGen.GenerateParentKey(bodyProperties = (bodyProperties2 = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1)), CharacterObject.PlayerCharacter.Race, ref bodyProperties2, ref bodyProperties);
            bodyProperties2 = new BodyProperties(new DynamicBodyProperties(33f, 0.3f, 0.2f), bodyProperties2.StaticProperties);
            bodyProperties = new BodyProperties(new DynamicBodyProperties(33f, 0.5f, 0.5f), bodyProperties.StaticProperties);
            list.Add(new NarrativeMenuCharacter("mother_character", bodyProperties2, CharacterObject.PlayerCharacter.Race, true));
            list.Add(new NarrativeMenuCharacter("father_character", bodyProperties, CharacterObject.PlayerCharacter.Race, false));
            NarrativeMenu narrativeMenu = new NarrativeMenu("narrative_parent_menu", "start", "narrative_childhood_menu", new TextObject("{=!}Family", null), new TextObject("{=XgFU1pCx}You were born into a family of...", null), list, new NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate(this.GetParentMenuNarrativeMenuCharacterArgs));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_ARais", new TextObject("{=CCR_Family_Choice_ARais}rais", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAseraiKinsfolkNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiKinsfolkNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AseraiKinsfolkNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_AMamluks", new TextObject("{=CCR_Family_Choice_AMamluks}mamluks", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAseraiSlaveNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiSlaveNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AseraiSlaveNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_AMerchant", new TextObject("{=CCR_Family_Choice_AMerchant}merchants", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAseraiPhysicianNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiPhysicianNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AseraiPhysicianNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_AFarmers", new TextObject("{=CCR_Family_Choice_AFarmers}farmers", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAseraiFarmerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiFarmerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AseraiFarmerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_AArtisans", new TextObject("{=CCR_Family_Choice_AArtisans}artisans", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAseraiHerderNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiHerderNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AseraiHerderNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_AThugs", new TextObject("{=CCR_Family_Choice_AThugs}thugs", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAseraiArtisanNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiArtisanNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AseraiArtisanNarrativeOptionOnSelect), null));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Bchieftains", new TextObject("{=CCR_Family_Choice_Bchieftains}chieftains", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaRetainerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaRetainerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.BattaniaRetainerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Bhealers", new TextObject("{=CCR_Family_Choice_Bhealers}healers", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaHealerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaHealerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.BattaniaHealerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Bfarmers", new TextObject("{=CCR_Family_Choice_Bfarmers}farmers", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaFarmerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaFarmerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.BattaniaFarmerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Bartisans", new TextObject("{=CCR_Family_Choice_Bartisans}artisans", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaArtisanNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaArtisanNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.BattaniaArtisanNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Bforesters", new TextObject("{=CCR_Family_Choice_Bforesters}foresters", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaHunterNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaHunterNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.BattaniaHunterNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Bbards", new TextObject("{=CCR_Family_Choice_Bbards}bards", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaBardNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaBardNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.BattaniaBardNarrativeOptionOnSelect), null));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Earistocrates", new TextObject("{=CCR_Family_Choice_Earistocrates}aristocrates", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireLandlordNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireLandlordNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EmpireLandlordNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Emerchants", new TextObject("{=CCR_Family_Choice_Emerchants}merchants", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireUrbanNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireUrbanNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EmpireUrbanNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Efreeholders", new TextObject("{=CCR_Family_Choice_Efreeholders}freeholders", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireFarmerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireFarmerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EmpireFarmerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Eartisans", new TextObject("{=CCR_Family_Choice_Eartisans}artisans", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireArtisanNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireArtisanNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EmpireArtisanNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Esoldiers", new TextObject("{=CCR_Family_Choice_Esoldiers}soldiers", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireHunterNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireHunterNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EmpireHunterNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Evagabonds", new TextObject("{=CCR_Family_Choice_Evagabonds}vagabonds", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireVagabondNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireVagabondNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EmpireVagabondNarrativeOptionOnSelect), null));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Knoyans", new TextObject("{=CCR_Family_Choice_Knoyans}noyans", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitRetainerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitRetainerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.KhuzaitRetainerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Knomads", new TextObject("{=CCR_Family_Choice_Knomads}nomads", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitMerchantNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitMerchantNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.KhuzaitMerchantNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Kmerchants", new TextObject("{=CCR_Family_Choice_Kmerchants}merchants", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitHerderNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitHerderNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.KhuzaitHerderNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Kartisans", new TextObject("{=CCR_Family_Choice_Kartisans}artisans", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitFarmerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitFarmerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.KhuzaitFarmerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Kwarriors", new TextObject("{=CCR_Family_Choice_Kwarriors}warriors", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitHealerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitHealerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.KhuzaitHealerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Kthugs", new TextObject("{=CCR_Family_Choice_Kthugs}thugs", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitNomadHerderNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitNomadHerderNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.KhuzaitNomadHerderNarrativeOptionOnSelect), null));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Sboyars", new TextObject("{=CCR_Family_Choice_Sboyars}boyars", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaCompanionNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaCompanionNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.SturgiaCompanionNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Smerchants", new TextObject("{=CCR_Family_Choice_Smerchants}merchants", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaTraderNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaTraderNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.SturgiaTraderNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Sfarmers", new TextObject("{=CCR_Family_Choice_Sfarmers}farmers", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaFarmerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaFarmerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.SturgiaFarmerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Sartisans", new TextObject("{=CCR_Family_Choice_Sartisans}artisans", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaArtisanNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaArtisanNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.SturgiaArtisanNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Swarriors", new TextObject("{=CCR_Family_Choice_Swarriors}warriors", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaHunterNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaHunterNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.SturgiaHunterNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Sraiders", new TextObject("{=CCR_Family_Choice_Sraiders}raiders", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaVagabondNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaVagabondNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.SturgiaVagabondNarrativeOptionOnSelect), null));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Vbarons", new TextObject("{=CCR_Family_Choice_Vbarons}barons", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaRetainerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaRetainerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaRetainerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Vmerchants", new TextObject("{=CCR_Family_Choice_Vmerchants}merchants", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaMerchantNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaMerchantNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaMerchantNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Vyeomens", new TextObject("{=CCR_Family_Choice_Vyeomens}yeomen", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaFarmerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaFarmerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaFarmerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Vartisans", new TextObject("{=CCR_Family_Choice_Vartisans}artisans", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaBlacksmithNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaBlacksmithNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaBlacksmithNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Vsoldiers", new TextObject("{=CCR_Family_Choice_Vsoldiers}soldiers", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaHunterNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaHunterNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaHunterNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Vmercenaries", new TextObject("{=CCR_Family_Choice_Vmercenaries}mercenaries", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaMercenaryNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaMercenaryNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaMercenaryNarrativeOptionOnSelect), null));

            characterCreationManager.AddNewMenu(narrativeMenu);
        }

        public void GetEmpireLandlordNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.Polearm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }

        public bool EmpireLandlordNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }

        public void EmpireLandlordNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("retainer");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_side_to_side_1";
            string fatherAnimation = "act_character_creation_male_default_side_to_side_1";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetEmpireUrbanNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }

        public bool EmpireUrbanNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }

        public void EmpireUrbanNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("merchant_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_mother_front";
            string fatherAnimation = "act_character_creation_male_default_mother_front";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetEmpireFarmerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Athletics, DefaultSkills.Polearm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool EmpireFarmerNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }

        public void EmpireFarmerNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("farmer");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_father_sitting";
            string fatherAnimation = "act_character_creation_male_default_father_sitting";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetEmpireArtisanNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Crossbow };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool EmpireArtisanNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }

        public void EmpireArtisanNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("artisan_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_side_to_side_2";
            string fatherAnimation = "act_character_creation_male_default_side_to_side_2";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetEmpireHunterNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting, DefaultSkills.Bow };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }

        public bool EmpireHunterNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }

        public void EmpireHunterNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("hunter");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_side_to_side_3";
            string fatherAnimation = "act_character_creation_male_default_side_to_side_3";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetEmpireVagabondNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Throwing };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }

        public bool EmpireVagabondNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }

        public void EmpireVagabondNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("vagabond_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_hugging";
            string fatherAnimation = "act_character_creation_male_default_hugging";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        new public void UpdateParentEquipment(CharacterCreationManager characterCreationManager, MBEquipmentRoster motherEquipment, MBEquipmentRoster fatherEquipment, string motherAnimation, string fatherAnimation)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId.Equals("mother_character"))
                {
                    narrativeMenuCharacter.SetEquipment(motherEquipment);
                    narrativeMenuCharacter.SetAnimationId(motherAnimation);
                }
                if (narrativeMenuCharacter.StringId.Equals("father_character"))
                {
                    narrativeMenuCharacter.SetEquipment(fatherEquipment);
                    narrativeMenuCharacter.SetAnimationId(fatherAnimation);
                }
            }
        }

        public void GetVlandiaRetainerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.Polearm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }

        public bool VlandiaRetainerNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }

        public void VlandiaRetainerNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("retainer");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_side_to_side_1";
            string fatherAnimation = "act_character_creation_male_default_side_to_side_1";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetVlandiaMerchantNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool VlandiaMerchantNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }

        public void VlandiaMerchantNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("merchant_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_mother_front";
            string fatherAnimation = "act_character_creation_male_default_mother_front";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetVlandiaFarmerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.Crossbow };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool VlandiaFarmerNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }

        public void VlandiaFarmerNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("farmer");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_father_sitting";
            string fatherAnimation = "act_character_creation_male_default_father_sitting";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetVlandiaBlacksmithNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.TwoHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }

        public bool VlandiaBlacksmithNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }

        public void VlandiaBlacksmithNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("artisan_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_side_to_side_2";
            string fatherAnimation = "act_character_creation_male_default_side_to_side_2";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetVlandiaHunterNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting, DefaultSkills.Crossbow };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }

        public bool VlandiaHunterNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }

        public void VlandiaHunterNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("hunter");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_side_to_side_3";
            string fatherAnimation = "act_character_creation_male_default_side_to_side_3";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetVlandiaMercenaryNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Crossbow };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }

        public bool VlandiaMercenaryNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }

        public void VlandiaMercenaryNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("merchant_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_hugging";
            string fatherAnimation = "act_character_creation_male_default_hugging";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetSturgiaCompanionNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.TwoHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }

        public bool SturgiaCompanionNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }

        public void SturgiaCompanionNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("retainer");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_side_to_side_1";
            string fatherAnimation = "act_character_creation_male_default_side_to_side_1";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetSturgiaTraderNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Tactics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }

        public bool SturgiaTraderNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }

        public void SturgiaTraderNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("merchant_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_mother_front";
            string fatherAnimation = "act_character_creation_male_default_mother_front";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetSturgiaFarmerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Athletics, DefaultSkills.Polearm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool SturgiaFarmerNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }

        public void SturgiaFarmerNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("farmer");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_father_sitting";
            string fatherAnimation = "act_character_creation_male_default_father_sitting";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetSturgiaArtisanNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.OneHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool SturgiaArtisanNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }

        public void SturgiaArtisanNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("artisan_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_side_to_side_2";
            string fatherAnimation = "act_character_creation_male_default_side_to_side_2";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetSturgiaHunterNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting, DefaultSkills.Bow };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }

        public bool SturgiaHunterNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }

        public void SturgiaHunterNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("hunter");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_side_to_side_3";
            string fatherAnimation = "act_character_creation_male_default_side_to_side_3";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetSturgiaVagabondNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Throwing };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }

        public bool SturgiaVagabondNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }

        public void SturgiaVagabondNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("vagabond_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_hugging";
            string fatherAnimation = "act_character_creation_male_default_hugging";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }


        public void GetAseraiKinsfolkNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.Throwing };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }

        public bool AseraiKinsfolkNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }

        public void AseraiKinsfolkNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("retainer");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_side_to_side_1";
            string fatherAnimation = "act_character_creation_male_default_side_to_side_1";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetAseraiSlaveNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.Polearm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }

        public bool AseraiSlaveNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }

        public void AseraiSlaveNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("mercenary_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_mother_front";
            string fatherAnimation = "act_character_creation_male_default_mother_front";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetAseraiPhysicianNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Medicine, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool AseraiPhysicianNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }

        public void AseraiPhysicianNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("physician_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_father_sitting";
            string fatherAnimation = "act_character_creation_male_default_father_sitting";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetAseraiFarmerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Athletics, DefaultSkills.OneHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool AseraiFarmerNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }

        public void AseraiFarmerNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("farmer");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_side_to_side_2";
            string fatherAnimation = "act_character_creation_male_default_side_to_side_2";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetAseraiHerderNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting, DefaultSkills.Bow };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }

        public bool AseraiHerderNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }

        public void AseraiHerderNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("herder");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_side_to_side_3";
            string fatherAnimation = "act_character_creation_male_default_side_to_side_3";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetAseraiArtisanNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Polearm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }

        public bool AseraiArtisanNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }

        public void AseraiArtisanNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("artisan_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_hugging";
            string fatherAnimation = "act_character_creation_male_default_hugging";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetBattaniaRetainerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.TwoHanded, DefaultSkills.Bow };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }

        public bool BattaniaRetainerNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }

        public void BattaniaRetainerNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("retainer_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_side_to_side_1";
            string fatherAnimation = "act_character_creation_male_default_side_to_side_1";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetBattaniaHealerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Medicine, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool BattaniaHealerNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }

        public void BattaniaHealerNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("healer");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_mother_front";
            string fatherAnimation = "act_character_creation_male_default_mother_front";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetBattaniaFarmerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Athletics, DefaultSkills.Throwing };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }

        public bool BattaniaFarmerNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }

        public void BattaniaFarmerNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("farmer");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_father_sitting";
            string fatherAnimation = "act_character_creation_male_default_father_sitting";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetBattaniaArtisanNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.TwoHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool BattaniaArtisanNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }

        public void BattaniaArtisanNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("artisan_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_side_to_side_2";
            string fatherAnimation = "act_character_creation_male_default_side_to_side_2";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetBattaniaHunterNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting, DefaultSkills.Tactics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }

        public bool BattaniaHunterNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }

        public void BattaniaHunterNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("hunter");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_side_to_side_3";
            string fatherAnimation = "act_character_creation_male_default_side_to_side_3";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetBattaniaBardNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }

        public bool BattaniaBardNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }

        public void BattaniaBardNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("bard_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_hugging";
            string fatherAnimation = "act_character_creation_male_default_hugging";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetKhuzaitRetainerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.Polearm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool KhuzaitRetainerNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }

        public void KhuzaitRetainerNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("retainer_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_side_to_side_1";
            string fatherAnimation = "act_character_creation_male_default_side_to_side_1";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetKhuzaitMerchantNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }

        public bool KhuzaitMerchantNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }

        public void KhuzaitMerchantNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("merchant_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_mother_front";
            string fatherAnimation = "act_character_creation_male_default_mother_front";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetKhuzaitHerderNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Bow, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }

        public bool KhuzaitHerderNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }

        public void KhuzaitHerderNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("herder");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_father_sitting";
            string fatherAnimation = "act_character_creation_male_default_father_sitting";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetKhuzaitFarmerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.Throwing };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }

        public bool KhuzaitFarmerNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }

        public void KhuzaitFarmerNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("farmer");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_side_to_side_2";
            string fatherAnimation = "act_character_creation_male_default_side_to_side_2";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetKhuzaitHealerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Medicine, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool KhuzaitHealerNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }

        public void KhuzaitHealerNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("healer_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_side_to_side_3";
            string fatherAnimation = "act_character_creation_male_default_side_to_side_3";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetKhuzaitNomadHerderNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }

        public bool KhuzaitNomadHerderNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }

        public void KhuzaitNomadHerderNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("herder");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_hugging";
            string fatherAnimation = "act_character_creation_male_default_hugging";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public List<NarrativeMenuCharacterArgs> AddEducationMenuCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            List<NarrativeMenuCharacterArgs> list = new List<NarrativeMenuCharacterArgs>();
            string playerChildhoodAgeEquipmentId = this.GetPlayerChildhoodAgeEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            list.Add(new NarrativeMenuCharacterArgs("player_childhood_character", 7, playerChildhoodAgeEquipmentId, "act_childhood_schooled", "spawnpoint_player_1", "", "", null, true, CharacterObject.PlayerCharacter.IsFemale));
            return list;
        }
        new public void AddEducationMenu(CharacterCreationManager characterCreationManager)
        {
            List<NarrativeMenuCharacter> list = new List<NarrativeMenuCharacter>();
            BodyProperties bodyProperties = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1);
            bodyProperties = FaceGen.GetBodyPropertiesWithAge(ref bodyProperties, 7f);
            list.Add(new NarrativeMenuCharacter("player_childhood_character", bodyProperties, CharacterObject.PlayerCharacter.Race, CharacterObject.PlayerCharacter.IsFemale));
            NarrativeMenu narrativeMenu = new NarrativeMenu("narrative_childhood_menu", "narrative_parent_menu", "narrative_education_menu", new TextObject("{=!}Received education", null), new TextObject("{=!}Your parents wanted you to...", null), list, new NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate(this.AddEducationMenuCharacterArgs));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_Faris", new TextObject("{=CCR_Education_Choice_Faris}become a faris.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.EducationFarisOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationFarisOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationFarisOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_Hearthguard", new TextObject("{=CCR_Education_Choice_Hearthguard}enter the hearthguard.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.EducationHearthguardOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationHearthguardOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationHearthguardOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_Cataphract", new TextObject("{=CCR_Education_Choice_Cataphract}become a cataphract.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.EducationCataphractOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationCataphractOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationCataphractOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_KhanGuard", new TextObject("{=CCR_Education_Choice_KhanGuard}enter a khan's guard.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.EducationKhanGuardOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationKhanGuardOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationKhanGuardOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_Druzhina", new TextObject("{=CCR_Education_Choice_Druzhina}join a druzhina.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.EducationDruzhinaOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationDruzhinaOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationDruzhinaOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_Knight", new TextObject("{=CCR_Education_Choice_Knight}become a knight.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.EducationKnightOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationKnightOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationKnightOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_Commander", new TextObject("{=CCR_Education_Choice_Commander}lead armies.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.EducationCommanderOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationCommanderOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationCommanderOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_Court", new TextObject("{=CCR_Education_Choice_Court}be part of the court.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.EducationCourtOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationCourtOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationCourtOptionOnSelect), null));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_trade", new TextObject("{=CCR_Education_Choice_trade}become a merchant.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.EducationMerchantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationMerchantOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationMerchantOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_crafting", new TextObject("{=CCR_Education_Choice_crafting}learn a trade.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.EducationCraftOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationCraftOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationCraftOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_scholar", new TextObject("{=CCR_Education_Choice_scholar}become a scholar.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.EducationScholarOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationScholarOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationScholarOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_religious", new TextObject("{=CCR_Education_Choice_religious}be a {?PLAYER.GENDER}lady{?}man{\\?} of faith.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.EducationReligiousOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationReligiousOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationReligiousOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_farmer", new TextObject("{=CCR_Education_Choice_farmer}tend to the fields.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.EducationFarmerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationFarmerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationFarmerOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_lady", new TextObject("{=CCR_Education_Choice_lady}become a lady in waiting.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.EducationLadyOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationLadyOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(EducationLadyOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_defense", new TextObject("{=CCR_Education_Choice_defense}learn how to defend yourself.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.EducationDefenseOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationDefenseOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationDefenseOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_trickery", new TextObject("{=CCR_Education_Choice_trickery}know how to trick others.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.EducationTrickOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationTrickOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationTrickOptionOnSelect), null));
            characterCreationManager.AddNewMenu(narrativeMenu);
        }

        public void EducationFarisOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.Athletics, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool EducationFarisOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Retainer && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }

        public void EducationFarisOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_childhood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_leader");
                }
            }
        }

        public void EducationHearthguardOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.TwoHanded, DefaultSkills.Bow, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }

        public bool EducationHearthguardOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Retainer && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }

        public void EducationHearthguardOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_childhood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_athlete");
                }
            }
        }

        public void EducationCataphractOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Polearm, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool EducationCataphractOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Retainer && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }

        public void EducationCataphractOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_childhood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_memory");
                }
            }
        }

        public void EducationKhanGuardOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Bow, DefaultSkills.Polearm, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }

        public bool EducationKhanGuardOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Retainer && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }

        public void EducationKhanGuardOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_childhood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_numbers");
                }
            }
        }

        public void EducationDruzhinaOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.TwoHanded, DefaultSkills.Throwing, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool EducationDruzhinaOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Retainer && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }

        public void EducationDruzhinaOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_childhood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_manners");
                }
            }
        }

        public void EducationKnightOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.Athletics, DefaultSkills.Polearm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool EducationKnightOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Retainer && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }

        public void EducationKnightOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_childhood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_animals");
                }
            }
        }
        public void EducationCommanderOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Leadership, DefaultSkills.Tactics, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }

        public bool EducationCommanderOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Retainer && !Hero.MainHero.IsFemale;
        }

        public void EducationCommanderOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_childhood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_leader");
                }
            }
        }

        public void EducationCourtOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Charm, DefaultSkills.Roguery, DefaultSkills.Tactics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }

        public bool EducationCourtOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Retainer && Hero.MainHero.IsFemale;
        }

        public void EducationCourtOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_childhood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_athlete");
                }
            }
        }

        public void EducationMerchantOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Charm, DefaultSkills.Steward };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }

        public bool EducationMerchantOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void EducationMerchantOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_childhood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_memory");
                }
            }
        }

        public void EducationCraftOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Engineering, DefaultSkills.Trade, DefaultSkills.Crafting };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool EducationCraftOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void EducationCraftOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_childhood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_numbers");
                }
            }
        }

        public void EducationScholarOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Medicine, DefaultSkills.Engineering, DefaultSkills.Tactics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool EducationScholarOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void EducationScholarOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_childhood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_manners");
                }
            }
        }

        public void EducationReligiousOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Steward, DefaultSkills.Medicine, DefaultSkills.Trade };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool EducationReligiousOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void EducationReligiousOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_childhood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_animals");
                }
            }
        }
        public void EducationFarmerOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Medicine, DefaultSkills.Athletics};
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool EducationFarmerOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void EducationFarmerOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_childhood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_leader");
                }
            }
        }

        public void EducationLadyOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Charm, DefaultSkills.Steward };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }

        public bool EducationLadyOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void EducationLadyOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_childhood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_athlete");
                }
            }
        }

        public void EducationDefenseOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Athletics, DefaultSkills.Roguery };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }

        public bool EducationDefenseOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void EducationDefenseOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_childhood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_memory");
                }
            }
        }

        public void EducationTrickOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Charm, DefaultSkills.OneHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }

        public bool EducationTrickOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void EducationTrickOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_childhood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_numbers");
                }
            }
        }



        /// <summary>
        /// Education menu
        /// </summary>
        public List<NarrativeMenuCharacterArgs> FavoriteIdiomMenuCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            List<NarrativeMenuCharacterArgs> list = new List<NarrativeMenuCharacterArgs>();
            string playerEducationAgeEquipmentId = this.GetPlayerEducationAgeEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            list.Add(new NarrativeMenuCharacterArgs("player_education_character", 12, playerEducationAgeEquipmentId, "act_childhood_schooled", "spawnpoint_player_1", "", "", null, true, CharacterObject.PlayerCharacter.IsFemale));
            return list;
        }

        public void FavoriteIdiomMenu(CharacterCreationManager characterCreationManager)
        {
            BodyProperties bodyProperties = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1);
            bodyProperties = FaceGen.GetBodyPropertiesWithAge(ref bodyProperties, 12f);
            List<NarrativeMenuCharacter> list = new List<NarrativeMenuCharacter>();
            list.Add(new NarrativeMenuCharacter("player_education_character", bodyProperties, CharacterObject.PlayerCharacter.Race, CharacterObject.PlayerCharacter.IsFemale));
            NarrativeMenu narrativeMenu = new NarrativeMenu("narrative_education_menu", "narrative_childhood_menu", "narrative_youth_menu", new TextObject("{=!}Idioms", null), new TextObject("{=!}Growing up, you were inculcated the saying...", null), list, new NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate(this.FavoriteIdiomMenuCharacterArgs));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_Afighter", new TextObject("{=CCR_Idiom_choice_fighter}Better to be a warrior in a garden than a gardener in a war.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomFighterAOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomFighterAOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomFighterAOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_Bfighter", new TextObject("{=CCR_Idiom_choice_fighter}Better to be a warrior in a garden than a gardener in a war.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomFighterBOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomFighterBOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomFighterBOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_Efighter", new TextObject("{=CCR_Idiom_choice_fighter}Better to be a warrior in a garden than a gardener in a war.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomFighterEOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomFighterEOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomFighterEOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_Kfighter", new TextObject("{=CCR_Idiom_choice_fighter}Better to be a warrior in a garden than a gardener in a war.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomFighterKOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomFighterKOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomFighterKOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_Sfighter", new TextObject("{=CCR_Idiom_choice_fighter}Better to be a warrior in a garden than a gardener in a war.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomFighterSOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomFighterSOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomFighterSOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_Vfighter", new TextObject("{=CCR_Idiom_choice_fighter}Better to be a warrior in a garden than a gardener in a war.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomFighterVOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomFighterVOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomFighterVOptionOnSelect), null));
            
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_Cfighter", new TextObject("{=CCR_Idiom_choice_fighter}Better to be a warrior in a garden than a gardener in a war.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomFighterCOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomFighterCOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomFighterCOptionOnSelect), null));
            
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_healthy", new TextObject("{=CCR_Idiom_choice_healthy}A healthy mind in a healthy body.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomHealthyOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomHealthyOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomHealthyOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_prevention", new TextObject("{=CCR_Idiom_choice_prevention}Prevention is better than cure.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomPreventionOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomPreventionOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomPreventionOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_wellbegun", new TextObject("{=CCR_Idiom_choice_wellbegun}Well begun is half done.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomWellBegunOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomWellBegunOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomWellBegunOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_bold", new TextObject("{=CCR_Idiom_choice_bold}Fortune favors the bold.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomBoldOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomBoldOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomBoldOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_forwarned", new TextObject("{=CCR_Idiom_choice_forwarned}Forewarned is forearmed.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomForwarnedOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomForwarnedOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomForwarnedOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_invention", new TextObject("{=CCR_Idiom_choice_invention}Necessity is the mother of invention.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomInventionOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomInventionOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomInventionOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_armed", new TextObject("{=CCR_Idiom_choice_armed}Men with weapons never starve.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomArmedOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomArmedOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomArmedOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_conquer", new TextObject("{=CCR_Idiom_choice_conquer}To conquer without risk is to triumph without glory.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomConquerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomConquerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomConquerOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_means", new TextObject("{=CCR_Idiom_choice_means}The end justifies the means.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomMeanOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomMeanOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomMeanOptionOnSelect), null));
            characterCreationManager.AddNewMenu(narrativeMenu);
        }

        public void IdiomFighterAOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }

        public bool IdiomFighterAOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Retainer && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }

        public void IdiomFighterAOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_streets");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("carry_bostaff_rogue1");
                    break;
                }
            }
        }

        public void IdiomFighterBOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Bow, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }

        public bool IdiomFighterBOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Retainer && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }

        public void IdiomFighterBOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_militia");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("peasant_hammer_1_t1");
                    break;
                }
            }
        }

        public void IdiomFighterEOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }

        public bool IdiomFighterEOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Retainer && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }

        public void IdiomFighterEOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_grit");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("carry_hammer");
                    break;
                }
            }
        }

        public void IdiomFighterKOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Bow, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }

        public bool IdiomFighterKOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Retainer && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }

        public void IdiomFighterKOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_peddlers");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("_to_carry_bd_basket_a");
                    break;
                }
            }
        }

        public void IdiomFighterSOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.TwoHanded, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }

        public bool IdiomFighterSOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Retainer && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }

        public void IdiomFighterSOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("composite_bow");
                    break;
                }
            }
        }

        public void IdiomFighterVOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }

        public bool IdiomFighterVOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Retainer && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }

        public void IdiomFighterVOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_peddlers_2");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("_to_carry_bd_fabric_c");
                    break;
                }
            }
        }

        public void IdiomFighterCOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool IdiomFighterCOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return !(characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Retainer);
        }

        public void IdiomFighterCOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_fox");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("");
                    break;
                }
            }
        }

        public void IdiomHealthyOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Athletics, DefaultSkills.Medicine };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool IdiomHealthyOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void IdiomHealthyOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_athlete");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("");
                    break;
                }
            }
        }

        public void IdiomPreventionOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Medicine, DefaultSkills.Steward };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool IdiomPreventionOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void IdiomPreventionOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_peddlers");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("_to_carry_bd_basket_a");
                    break;
                }
            }
        }

        public void IdiomWellBegunOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Steward, DefaultSkills.Engineering };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool IdiomWellBegunOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void IdiomWellBegunOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_manners");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("");
                    break;
                }
            }
        }

        public void IdiomBoldOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Tactics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool IdiomBoldOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void IdiomBoldOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_book");
                    narrativeMenuCharacter.SetLeftHandItem("character_creation_notebook");
                    narrativeMenuCharacter.SetRightHandItem("");
                    break;
                }
            }
        }

        public void IdiomForwarnedOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Tactics, DefaultSkills.Scouting };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool IdiomForwarnedOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void IdiomForwarnedOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_peddlers_2");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("_to_carry_bd_fabric_c");
                    break;
                }
            }
        }

        public void IdiomInventionOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Engineering };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool IdiomInventionOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void IdiomInventionOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_streets");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("carry_bostaff_rogue1");
                    break;
                }
            }
        }

        public void IdiomArmedOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Roguery };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }

        public bool IdiomArmedOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void IdiomArmedOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_militia");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("peasant_hammer_1_t1");
                    break;
                }
            }
        }

        public void IdiomConquerOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Leadership, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }

        public bool IdiomConquerOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void IdiomConquerOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_grit");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("carry_hammer");
                    break;
                }
            }
        }

        public void IdiomMeanOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Tactics, DefaultSkills.Roguery };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }

        public bool IdiomMeanOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void IdiomMeanOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_peddlers");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("_to_carry_bd_basket_a");
                    break;
                }
            }
        }

        public List<NarrativeMenuCharacterArgs> StartInLifeMenuCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            if (string.IsNullOrEmpty(characterCreationManager.CharacterCreationContent.SelectedTitleType))
            {
                characterCreationManager.CharacterCreationContent.SelectedTitleType = "guard";
            }
            List<NarrativeMenuCharacterArgs> list = new List<NarrativeMenuCharacterArgs>();
            string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            list.Add(new NarrativeMenuCharacterArgs("player_youth_character", 17, playerEquipmentId, "act_childhood_schooled", "spawnpoint_player_1", "", "", null, true, CharacterObject.PlayerCharacter.IsFemale));
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId);
            ItemObject item = @object.DefaultEquipment[EquipmentIndex.ArmorItemEndSlot].Item;
            list.Add(new NarrativeMenuCharacterArgs("narrative_character_horse", -1, "", "act_inventory_idle_start", "spawnpoint_mount_1", @object.DefaultEquipment[EquipmentIndex.ArmorItemEndSlot].Item.StringId, @object.DefaultEquipment[EquipmentIndex.HorseHarness].Item.StringId, MountCreationKey.GetRandomMountKey(item, CharacterObject.PlayerCharacter.GetMountKeySeed()), false, false));
            return list;
        }

        /// <summary>
        /// Youth menu
        /// </summary>
        public void StartInLifeMenu(CharacterCreationManager characterCreationManager)
        {
            TextObject description = CharacterObject.PlayerCharacter.IsFemale ? new TextObject("{=!}Start in life", null) : new TextObject("{=!}You started your life as...", null);
            BodyProperties bodyProperties = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1);
            bodyProperties = FaceGen.GetBodyPropertiesWithAge(ref bodyProperties, 17f);
            List<NarrativeMenuCharacter> list = new List<NarrativeMenuCharacter>();
            list.Add(new NarrativeMenuCharacter("player_youth_character", bodyProperties, CharacterObject.PlayerCharacter.Race, CharacterObject.PlayerCharacter.IsFemale));
            list.Add(new NarrativeMenuCharacter("narrative_character_horse"));
            NarrativeMenu narrativeMenu = new NarrativeMenu("narrative_youth_menu", "narrative_education_menu", "narrative_adulthood_menu", new TextObject("{=ok8lSW6M}Youth", null), description, list, new NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate(this.StartInLifeMenuCharacterArgs));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Afaris", new TextObject("{=CCR_Start_Choice_Afaris}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAFarisOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAFarisOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAFarisOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAFarisOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Acaravaner", new TextObject("{=CCR_Start_Choice_Acaravaner}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartACaravaneerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartACaravaneerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartACaravaneerOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartACaravaneerOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Amerchant", new TextObject("{=CCR_Start_Choice_Amerchant}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAMerchantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAMerchantOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAMerchantOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAMerchantOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Acraftman", new TextObject("{=CCR_Start_Choice_Acraftman}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartACraftmanOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartACraftmanOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartACraftmanOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartACraftmanOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Afarmer", new TextObject("{=CCR_Start_Choice_Afarmer}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAFarmerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAFarmerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAFarmerOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAFarmerOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Amamluke", new TextObject("{=CCR_Start_Choice_Amamluke}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAMamlukeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAMamlukeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAMamlukeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAMamlukeOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Ahorsearcher", new TextObject("{=CCR_Start_Choice_Ahorsearcher}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAHorseArcherOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAHorseArcherOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAHorseArcherOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAHorseArcherOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Aarcher", new TextObject("{=CCR_Start_Choice_Aarcher}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAArcherOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAArcherOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAArcherOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAArcherOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Adesertbandit", new TextObject("{=CCR_Start_Choice_Adesertbandit}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartADesertBanditOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartADesertBanditOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartADesertBanditOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartADesertBanditOptionOnConsequence)));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Afaris", new TextObject("{=CCR_Start_Choice_Afaris}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAFarisOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAFarisOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAFarisOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAFarisOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Acaravaner", new TextObject("{=CCR_Start_Choice_Acaravaner}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartACaravaneerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartACaravaneerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartACaravaneerOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartACaravaneerOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Amerchant", new TextObject("{=CCR_Start_Choice_Amerchant}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAMerchantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAMerchantOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAMerchantOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAMerchantOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Acraftman", new TextObject("{=CCR_Start_Choice_Acraftman}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartACraftmanOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartACraftmanOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartACraftmanOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartACraftmanOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Afarmer", new TextObject("{=CCR_Start_Choice_Afarmer}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAFarmerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAFarmerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAFarmerOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAFarmerOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Amamluke", new TextObject("{=CCR_Start_Choice_Amamluke}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAMamlukeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAMamlukeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAMamlukeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAMamlukeOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Ahorsearcher", new TextObject("{=CCR_Start_Choice_Ahorsearcher}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAHorseArcherOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAHorseArcherOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAHorseArcherOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAHorseArcherOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Aarcher", new TextObject("{=CCR_Start_Choice_Aarcher}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAArcherOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAArcherOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAArcherOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAArcherOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Adesertbandit", new TextObject("{=CCR_Start_Choice_Adesertbandit}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartADesertBanditOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartADesertBanditOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartADesertBanditOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartADesertBanditOptionOnConsequence)));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Afaris", new TextObject("{=CCR_Start_Choice_Afaris}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAFarisOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAFarisOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAFarisOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAFarisOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Acaravaner", new TextObject("{=CCR_Start_Choice_Acaravaner}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartACaravaneerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartACaravaneerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartACaravaneerOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartACaravaneerOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Amerchant", new TextObject("{=CCR_Start_Choice_Amerchant}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAMerchantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAMerchantOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAMerchantOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAMerchantOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Acraftman", new TextObject("{=CCR_Start_Choice_Acraftman}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartACraftmanOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartACraftmanOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartACraftmanOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartACraftmanOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Afarmer", new TextObject("{=CCR_Start_Choice_Afarmer}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAFarmerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAFarmerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAFarmerOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAFarmerOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Amamluke", new TextObject("{=CCR_Start_Choice_Amamluke}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAMamlukeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAMamlukeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAMamlukeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAMamlukeOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Ahorsearcher", new TextObject("{=CCR_Start_Choice_Ahorsearcher}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAHorseArcherOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAHorseArcherOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAHorseArcherOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAHorseArcherOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Aarcher", new TextObject("{=CCR_Start_Choice_Aarcher}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAArcherOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAArcherOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAArcherOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAArcherOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Adesertbandit", new TextObject("{=CCR_Start_Choice_Adesertbandit}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartADesertBanditOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartADesertBanditOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartADesertBanditOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartADesertBanditOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Adesertbandit", new TextObject("{=CCR_Start_Choice_Adesertbandit}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartADesertBanditOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartADesertBanditOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartADesertBanditOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartADesertBanditOptionOnConsequence)));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Afaris", new TextObject("{=CCR_Start_Choice_Afaris}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAFarisOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAFarisOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAFarisOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAFarisOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Acaravaner", new TextObject("{=CCR_Start_Choice_Acaravaner}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartACaravaneerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartACaravaneerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartACaravaneerOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartACaravaneerOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Amerchant", new TextObject("{=CCR_Start_Choice_Amerchant}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAMerchantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAMerchantOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAMerchantOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAMerchantOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Acraftman", new TextObject("{=CCR_Start_Choice_Acraftman}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartACraftmanOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartACraftmanOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartACraftmanOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartACraftmanOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Afarmer", new TextObject("{=CCR_Start_Choice_Afarmer}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAFarmerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAFarmerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAFarmerOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAFarmerOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Amamluke", new TextObject("{=CCR_Start_Choice_Amamluke}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAMamlukeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAMamlukeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAMamlukeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAMamlukeOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Ahorsearcher", new TextObject("{=CCR_Start_Choice_Ahorsearcher}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAHorseArcherOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAHorseArcherOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAHorseArcherOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAHorseArcherOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Aarcher", new TextObject("{=CCR_Start_Choice_Aarcher}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAArcherOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAArcherOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAArcherOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAArcherOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Adesertbandit", new TextObject("{=CCR_Start_Choice_Adesertbandit}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartADesertBanditOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartADesertBanditOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartADesertBanditOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartADesertBanditOptionOnConsequence)));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Afaris", new TextObject("{=CCR_Start_Choice_Afaris}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAFarisOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAFarisOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAFarisOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAFarisOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Acaravaner", new TextObject("{=CCR_Start_Choice_Acaravaner}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartACaravaneerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartACaravaneerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartACaravaneerOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartACaravaneerOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Amerchant", new TextObject("{=CCR_Start_Choice_Amerchant}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAMerchantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAMerchantOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAMerchantOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAMerchantOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Acraftman", new TextObject("{=CCR_Start_Choice_Acraftman}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartACraftmanOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartACraftmanOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartACraftmanOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartACraftmanOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Afarmer", new TextObject("{=CCR_Start_Choice_Afarmer}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAFarmerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAFarmerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAFarmerOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAFarmerOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Amamluke", new TextObject("{=CCR_Start_Choice_Amamluke}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAMamlukeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAMamlukeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAMamlukeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAMamlukeOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Ahorsearcher", new TextObject("{=CCR_Start_Choice_Ahorsearcher}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAHorseArcherOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAHorseArcherOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAHorseArcherOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAHorseArcherOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Aarcher", new TextObject("{=CCR_Start_Choice_Aarcher}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAArcherOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAArcherOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAArcherOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAArcherOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Adesertbandit", new TextObject("{=CCR_Start_Choice_Adesertbandit}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartADesertBanditOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartADesertBanditOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartADesertBanditOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartADesertBanditOptionOnConsequence)));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Afaris", new TextObject("{=CCR_Start_Choice_Afaris}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAFarisOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAFarisOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAFarisOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAFarisOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Acaravaner", new TextObject("{=CCR_Start_Choice_Acaravaner}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartACaravaneerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartACaravaneerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartACaravaneerOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartACaravaneerOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Amerchant", new TextObject("{=CCR_Start_Choice_Amerchant}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAMerchantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAMerchantOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAMerchantOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAMerchantOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Acraftman", new TextObject("{=CCR_Start_Choice_Acraftman}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartACraftmanOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartACraftmanOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartACraftmanOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartACraftmanOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Afarmer", new TextObject("{=CCR_Start_Choice_Afarmer}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAFarmerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAFarmerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAFarmerOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAFarmerOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Amamluke", new TextObject("{=CCR_Start_Choice_Amamluke}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAMamlukeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAMamlukeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAMamlukeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAMamlukeOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Ahorsearcher", new TextObject("{=CCR_Start_Choice_Ahorsearcher}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAHorseArcherOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAHorseArcherOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAHorseArcherOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAHorseArcherOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Aarcher", new TextObject("{=CCR_Start_Choice_Aarcher}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAArcherOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAArcherOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAArcherOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAArcherOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Adesertbandit", new TextObject("{=CCR_Start_Choice_Adesertbandit}", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartADesertBanditOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartADesertBanditOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartADesertBanditOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartADesertBanditOptionOnConsequence)));

            characterCreationManager.AddNewMenu(narrativeMenu);
        }

        public void LifeStartAFarisOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.OneHanded, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }

        public bool LifeStartAFarisOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Retainer && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }

        public void LifeStartAFarisOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "retainer";
            string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_youth_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_decisive");
                    narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
                }
            }
        }
        public void LifeStartAFarisOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
            Hero ruler = Hero.FindAll(hero => hero.Culture == Hero.MainHero.Culture && hero.IsAlive && hero.IsFactionLeader && !hero.MapFaction.IsMinorFaction).GetRandomElementInefficiently();
            ChangeKingdomAction.ApplyByJoinToKingdom(Hero.MainHero.Clan, ruler.Clan.Kingdom, default, false);
            CharacterObject wanderer = (from character in CharacterObject.All where character.Occupation == Occupation.Wanderer && character.Culture == Hero.MainHero.Culture select character).GetRandomElementInefficiently();
            Hero companion = HeroCreator.CreateSpecialHero(wanderer);
            AddCompanionAction.Apply(Clan.PlayerClan, companion);
            AddHeroToPartyAction.Apply(companion, Hero.MainHero.PartyBelongedTo);
        }

        public void LifeStartACaravaneerOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting, DefaultSkills.Trade, DefaultSkills.Leadership };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }

        public bool LifeStartACaravaneerOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }

        public void LifeStartACaravaneerOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "retainer";
            string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_youth_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
                    narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
                }
            }
        }

        public void LifeStartACaravaneerOptionOnConsequence(CharacterCreationManager characterCreationManager)
        { 
        }

        public void LifeStartAMerchantOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Steward, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }

        public bool LifeStartAMerchantOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }

        public void LifeStartAMerchantOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "retainer";
            string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_youth_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_ready");
                    narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
                }
            }
        }

        public void LifeStartAMerchantOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }

        public void LifeStartACraftmanOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Trade, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool LifeStartACraftmanOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }

        public void LifeStartACraftmanOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "mercenary";
            string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_youth_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_apprentice");
                    narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
                }
            }
        }

        public void LifeStartACraftmanOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }

        public void LifeStartAFarmerOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Steward, DefaultSkills.Medicine };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool LifeStartAFarmerOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }

        public void LifeStartAFarmerOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "mercenary";
            string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_youth_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_athlete");
                    narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
                }
            }
        }

        public void LifeStartAFarmerOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }

        public void LifeStartAMamlukeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Polearm, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool LifeStartAMamlukeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }

        public void LifeStartAMamlukeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "guard";
            string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_youth_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_vibrant");
                    narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
                }
            }
        }

        public void LifeStartAMamlukeOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }

        public void LifeStartAHorseArcherOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool LifeStartAHorseArcherOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }

        public void LifeStartAHorseArcherOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "guard";
            string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_youth_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
                    narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
                }
            }
        }

        public void LifeStartAHorseArcherOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }

        public void LifeStartAArcherOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool LifeStartAArcherOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }

        public void LifeStartAArcherOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "guard";
            string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_youth_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
                    narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
                }
            }
        }

        public void LifeStartAArcherOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }

        public void LifeStartADesertBanditOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Throwing, DefaultSkills.OneHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool LifeStartADesertBanditOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }

        public void LifeStartADesertBanditOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "guard";
            string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_youth_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
                    narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
                }
            }
        }
        public void LifeStartADesertBanditOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }


        /// <summary>
        /// Reason For Adventuring menu
        /// </summary>
        public List<NarrativeMenuCharacterArgs> GetReasonMenuCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            List<NarrativeMenuCharacterArgs> list = new List<NarrativeMenuCharacterArgs>();
            string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            list.Add(new NarrativeMenuCharacterArgs("player_adulthood_character", 20, playerEquipmentId, "act_childhood_schooled", "spawnpoint_player_1", "", "", null, true, CharacterObject.PlayerCharacter.IsFemale));
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId);
            ItemObject item = @object.DefaultEquipment[EquipmentIndex.ArmorItemEndSlot].Item;
            list.Add(new NarrativeMenuCharacterArgs("narrative_character_horse", -1, "", "act_horse_stand_1", "spawnpoint_mount_1", @object.DefaultEquipment[EquipmentIndex.ArmorItemEndSlot].Item.StringId, @object.DefaultEquipment[EquipmentIndex.HorseHarness].Item.StringId, MountCreationKey.GetRandomMountKey(item, CharacterObject.PlayerCharacter.GetMountKeySeed()), false, false));
            return list;
        }

        public void AddReasonMenu(CharacterCreationManager characterCreationManager)
        {
            BodyProperties bodyProperties = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1);
            bodyProperties = FaceGen.GetBodyPropertiesWithAge(ref bodyProperties, 20f);
            List<NarrativeMenuCharacter> list = new List<NarrativeMenuCharacter>();
            list.Add(new NarrativeMenuCharacter("player_adulthood_character", bodyProperties, CharacterObject.PlayerCharacter.Race, CharacterObject.PlayerCharacter.IsFemale));
            list.Add(new NarrativeMenuCharacter("narrative_character_horse"));
            MBTextManager.SetTextVariable("EXP_VALUE", 30);
            NarrativeMenu narrativeMenu = new NarrativeMenu("narrative_adulthood_menu", "narrative_youth_menu", "narrative_age_selection_menu", new TextObject("{=!}Reason for Adventuring", null), new TextObject("{=!}You started adventuring...", null), list, new NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate(this.GetReasonMenuCharacterArgs));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Reason_travel", new TextObject("{=CCR_Reason_travel}to discover the world.", null), new TextObject("{=CCR_Reason_desc_travel}The temptation of travel was to much for you, as you always dreamt of seeing the world.", null), new GetNarrativeMenuOptionArgsDelegate(this.ReasonTravelOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ReasonTravelOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ReasonTravelOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Reason_revenge", new TextObject("{=CCR_Reason_revenge}to take revenge.", null), new TextObject("{=CCR_Reason_desc_revenge}After being wronged, you felt the need for revenge. With that goal in mind, you wandered throughout Calradia to get reparations.", null), new GetNarrativeMenuOptionArgsDelegate(this.ReasonRevengeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ReasonRevengeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ReasonRevengeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Reason_forced_out", new TextObject("{=CCR_Reason_forced_out}after being forced out.", null), new TextObject("{=CCR_Reason_desc_forced_out}After one last disagreement with you, your parents forced you out. With nowhere to go, adventuring was all you could do.", null), new GetNarrativeMenuOptionArgsDelegate(this.ReasonForcedOutOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ReasonForcedOutOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ReasonForcedOutOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Reason_money", new TextObject("{=CCR_Reason_money}in search of money", null), new TextObject("{=CCR_Reason_desc_money}You always wanted to make riches, and it was obvious for you that staying at home would never allow you to do it.", null), new GetNarrativeMenuOptionArgsDelegate(this.ReasonMoneyOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ReasonMoneyOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ReasonMoneyOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Reason_power", new TextObject("{=CCR_Reason_power}to become one of the powerful", null), new TextObject("{=CCR_Reason_desc_power}Seeing how those in power had many advantages, you joined on an adventure to join them, or even replace them.", null), new GetNarrativeMenuOptionArgsDelegate(this.ReasonPowerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ReasonPowerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ReasonPowerOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Reason_history", new TextObject("{=CCR_Reason_history}to mark history", null), new TextObject("{=CCR_Reason_desc_history}With all the wars in Calradia, there are many way one could carve {?PLAYER.GENDER}her{?}his{\\?} name in history.", null), new GetNarrativeMenuOptionArgsDelegate(this.ReasonHistoryOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ReasonHistoryOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ReasonHistoryOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Reason_loss", new TextObject("{=CCR_Reason_loss}after the loss of a loved one", null), new TextObject("{=CCR_Reason_desc_loss}After losing some close to you, you left to see if you could fill that hole", null), new GetNarrativeMenuOptionArgsDelegate(this.ReasonLossOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ReasonLossOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ReasonLossOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Reason_crafting", new TextObject("{=CCR_Reason_crafting}to practice your trade", null), new TextObject("{=CCR_Reason_desc_crafting}Your craftmanship has been an important part of your life, but your skill wasn't enough. You thus decided to embark on a journey to improve at it.", null), new GetNarrativeMenuOptionArgsDelegate(this.ReasonCraftingOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ReasonCraftingOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ReasonCraftingOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Reason_helping", new TextObject("{=CCR_Reason_helping}to help those in need", null), new TextObject("{=CCR_Reason_desc_helping}Having been taught in medicine, you decide to set out and help those in need, as with the wars many have suffered.", null), new GetNarrativeMenuOptionArgsDelegate(this.ReasonHelpingOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ReasonHelpingOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ReasonHelpingOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Reason_prove_worth", new TextObject("{=CCR_Reason_prove_worth}to prove your fighting skills", null), new TextObject("{=CCR_Reason_desc_prove_worth}Attaching much importance to your fighting skill, you figured there was no better place than calradia to prove your might.", null), new GetNarrativeMenuOptionArgsDelegate(this.ReasonWorthOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ReasonWorthOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ReasonWorthOptionOnSelect), null));
            characterCreationManager.AddNewMenu(narrativeMenu);
        }

        public void ReasonTravelOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(50);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool ReasonTravelOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void ReasonTravelOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_adulthood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_athlete");
                }
            }
        }

        public void ReasonRevengeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Tactics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(50);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }

        public bool ReasonRevengeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void ReasonRevengeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_adulthood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_battania_mp_clan_warrior_shieldperk_idle");
                }
            }
        }

        public void ReasonForcedOutOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(50);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }

        public bool ReasonForcedOutOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void ReasonForcedOutOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_adulthood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_ready_handshield");
                }
            }
        }

        public void ReasonMoneyOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(50);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }

        public bool ReasonMoneyOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void ReasonMoneyOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_adulthood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_drafted_to_war_pose");
                }
            }
        }

        public void ReasonPowerOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Leadership };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(50);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }

        public bool ReasonPowerOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void ReasonPowerOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_adulthood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_vibrant");
                }
            }
        }

        public void ReasonHistoryOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(50);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }

        public bool ReasonHistoryOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void ReasonHistoryOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_adulthood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_decisive");
                }
            }
        }
        public void ReasonLossOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Steward };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(50);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }

        public bool ReasonLossOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void ReasonLossOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_adulthood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_decisive");
                }
            }
        }

        public void ReasonCraftingOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(50);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool ReasonCraftingOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void ReasonCraftingOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_adulthood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_tough");
                }
            }
        }

        public void ReasonHelpingOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Medicine };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(50);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool ReasonHelpingOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void ReasonHelpingOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_adulthood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_tough");
                }
            }
        }

        public void ReasonWorthOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.TwoHanded, DefaultSkills.Polearm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(20);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }

        public bool ReasonWorthOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void ReasonWorthOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_adulthood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_clever");
                }
            }
        }



        /// <summary>
        /// AgeSelection
        /// </summary>
        public List<NarrativeMenuCharacterArgs> GetAgeSelectionMenuNarrativeMenuCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            List<NarrativeMenuCharacterArgs> list = new List<NarrativeMenuCharacterArgs>();
            string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            list.Add(new NarrativeMenuCharacterArgs("player_age_selection_character", characterCreationManager.CharacterCreationContent.StartingAge, playerEquipmentId, "act_childhood_schooled", "spawnpoint_player_1", "", "", null, true, CharacterObject.PlayerCharacter.IsFemale));
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId);
            ItemObject item = @object.DefaultEquipment[EquipmentIndex.ArmorItemEndSlot].Item;
            list.Add(new NarrativeMenuCharacterArgs("narrative_character_horse", -1, "", "act_horse_stand_1", "spawnpoint_mount_1", @object.DefaultEquipment[EquipmentIndex.ArmorItemEndSlot].Item.StringId, @object.DefaultEquipment[EquipmentIndex.HorseHarness].Item.StringId, MountCreationKey.GetRandomMountKey(item, CharacterObject.PlayerCharacter.GetMountKeySeed()), false, false));
            return list;
        }

        public void AddAgeSelectionMenu(CharacterCreationManager characterCreationManager)
        {
            MBTextManager.SetTextVariable("EXP_VALUE", 30);
            BodyProperties bodyProperties = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1);
            bodyProperties = FaceGen.GetBodyPropertiesWithAge(ref bodyProperties, (float)characterCreationManager.CharacterCreationContent.StartingAge);
            List<NarrativeMenuCharacter> list = new List<NarrativeMenuCharacter>();
            list.Add(new NarrativeMenuCharacter("player_age_selection_character", bodyProperties, CharacterObject.PlayerCharacter.Race, CharacterObject.PlayerCharacter.IsFemale));
            list.Add(new NarrativeMenuCharacter("narrative_character_horse"));
            NarrativeMenu narrativeMenu = new NarrativeMenu("narrative_age_selection_menu", "narrative_adulthood_menu", "", new TextObject("{=HDFEAYDk}Starting Age", null), new TextObject("{=VlOGrGSn}Your character started off on the adventuring path at the age of...", null), list, new NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate(this.GetAgeSelectionMenuNarrativeMenuCharacterArgs));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("age_selection_young_adult_option", new TextObject("{=!}21", null), new TextObject("{=2k7adlh7}While lacking experience a bit, you are full with youthful energy, you are fully eager, for the long years of adventuring ahead.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAgeSelectionYoungAdultAgeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AgeSelectionYoungAdultAgeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AgeSelectionYoungAdultAgeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.AgeSelectionYoungAdultAgeOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("age_selection_adult_option", new TextObject("{=!}30", null), new TextObject("{=NUlVFRtK}You are at your prime, You still have some youthful energy but also have a substantial amount of experience under your belt. ", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAgeSelectionAdultOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AgeSelectionAdultOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AgeSelectionAdultOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.AgeSelectionAdultOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("age_selection_middle_age_option", new TextObject("{=!}40", null), new TextObject("{=5MxTYApM}This is the right age for starting off, you have years of experience, and you are old enough for people to respect you and gather under your banner.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAgeSelectionMiddleAgeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AgeSelectionMiddleAgeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AgeSelectionMiddleAgeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.AgeSelectionMiddleAgeOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("age_selection_elder_option", new TextObject("{=!}50", null), new TextObject("{=ePD5Afvy}While you are past your prime, there is still enough time to go on that last big adventure for you. And you have all the experience you need to overcome anything!", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAgeSelectionElderOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AgeSelectionElderOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AgeSelectionElderOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.AgeSelectionElderOptionOnConsequence)));
            characterCreationManager.AddNewMenu(narrativeMenu);
        }

        public void GetAgeSelectionYoungAdultAgeOptionArgs(NarrativeMenuOptionArgs args)
        {
            args.SetUnspentFocusToAdd(2);
            args.SetUnspentAttributeToAdd(1);
        }

        public bool AgeSelectionYoungAdultAgeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void AgeSelectionYoungAdultAgeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_age_selection_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_focus");
                    narrativeMenuCharacter.ChangeAge(21f);
                    MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId);
                    if (@object == null)
                    {
                        Debug.FailedAssert("character creation menu character equipment should not be null!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\CharacterCreationCampaignBehavior.cs", "AgeSelectionYoungAdultAgeOptionOnSelect", 4884);
                        @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>("player_char_creation_default");
                    }
                    narrativeMenuCharacter.SetEquipment(@object);
                    break;
                }
            }
            characterCreationManager.CharacterCreationContent.StartingAge = 21;
            Hero.MainHero.SetBirthDay(CampaignTime.YearsFromNow(-21f));
        }

        public void AgeSelectionYoungAdultAgeOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.StartingAge = 21;
            this.ApplyMainHeroEquipment(characterCreationManager);
        }

        public void GetAgeSelectionAdultOptionArgs(NarrativeMenuOptionArgs args)
        {
            args.SetUnspentFocusToAdd(4);
            args.SetUnspentAttributeToAdd(2);
        }

        public bool AgeSelectionAdultOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void AgeSelectionAdultOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_age_selection_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_athlete");
                    narrativeMenuCharacter.ChangeAge(30f);
                    MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId);
                    if (@object == null)
                    {
                        Debug.FailedAssert("character creation menu character equipment should not be null!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\CharacterCreationCampaignBehavior.cs", "AgeSelectionAdultOptionOnSelect", 4934);
                        @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>("player_char_creation_default");
                    }
                    narrativeMenuCharacter.SetEquipment(@object);
                    break;
                }
            }
            characterCreationManager.CharacterCreationContent.StartingAge = 30;
            Hero.MainHero.SetBirthDay(CampaignTime.YearsFromNow(-30f));
        }

        public void AgeSelectionAdultOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.StartingAge = 30;
            this.ApplyMainHeroEquipment(characterCreationManager);
        }

        public void GetAgeSelectionMiddleAgeOptionArgs(NarrativeMenuOptionArgs args)
        {
            args.SetUnspentFocusToAdd(6);
            args.SetUnspentAttributeToAdd(3);
        }

        public bool AgeSelectionMiddleAgeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void AgeSelectionMiddleAgeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_age_selection_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
                    narrativeMenuCharacter.ChangeAge(30f);
                    MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId);
                    if (@object == null)
                    {
                        Debug.FailedAssert("character creation menu character equipment should not be null!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\CharacterCreationCampaignBehavior.cs", "AgeSelectionMiddleAgeOptionOnSelect", 4984);
                        @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>("player_char_creation_default");
                    }
                    narrativeMenuCharacter.SetEquipment(@object);
                    break;
                }
            }
            characterCreationManager.CharacterCreationContent.StartingAge = 40;
            Hero.MainHero.SetBirthDay(CampaignTime.YearsFromNow(-40f));
        }

        public void AgeSelectionMiddleAgeOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.StartingAge = 40;
            this.ApplyMainHeroEquipment(characterCreationManager);
        }

        public void GetAgeSelectionElderOptionArgs(NarrativeMenuOptionArgs args)
        {
            args.SetUnspentFocusToAdd(8);
            args.SetUnspentAttributeToAdd(4);
        }

        public bool AgeSelectionElderOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void AgeSelectionElderOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_age_selection_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_tough");
                    narrativeMenuCharacter.ChangeAge(50f);
                    MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId);
                    if (@object == null)
                    {
                        Debug.FailedAssert("character creation menu character equipment should not be null!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\CharacterCreationCampaignBehavior.cs", "AgeSelectionElderOptionOnSelect", 5034);
                        @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>("player_char_creation_default");
                    }
                    narrativeMenuCharacter.SetEquipment(@object);
                    break;
                }
            }
            characterCreationManager.CharacterCreationContent.StartingAge = 50;
            Hero.MainHero.SetBirthDay(CampaignTime.YearsFromNow(-50f));
        }

        public void AgeSelectionElderOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.StartingAge = 50;
            this.ApplyMainHeroEquipment(characterCreationManager);
        }

        public void ApplyMainHeroEquipment(CharacterCreationManager characterCreationManager)
        {
            NarrativeMenu narrativeMenuWithId = characterCreationManager.GetNarrativeMenuWithId("narrative_age_selection_menu");
            NarrativeMenuCharacter narrativeMenuCharacter = null;
            foreach (NarrativeMenuCharacter narrativeMenuCharacter2 in narrativeMenuWithId.Characters)
            {
                if (narrativeMenuCharacter2.StringId.Equals("player_age_selection_character"))
                {
                    narrativeMenuCharacter = narrativeMenuCharacter2;
                    break;
                }
            }
            CharacterObject.PlayerCharacter.Equipment.FillFrom(narrativeMenuCharacter.Equipment.DefaultEquipment, true);
            CharacterObject.PlayerCharacter.FirstCivilianEquipment.FillFrom(narrativeMenuCharacter.Equipment.GetRandomCivilianEquipment(), true);
        }

        new public const string MotherNarrativeCharacterStringId = "mother_character";

        new public const string FatherNarrativeCharacterStringId = "father_character";

        new public const string PlayerChildhoodCharacterStringId = "player_childhood_character";

        new public const string PlayerEducationCharacterStringId = "player_education_character";

        new public const string PlayerYouthCharacterStringId = "player_youth_character";

        new public const string PlayerAdulthoodCharacterStringId = "player_adulthood_character";

        new public const string PlayerAgeSelectionCharacterStringId = "player_age_selection_character";

        new public const string HorseNarrativeCharacterStringId = "narrative_character_horse";

        public static class CharacterOccupationTypes
        {

            public const string Retainer = "retainer";

            public const string Bard = "bard";

            public const string Hunter = "hunter";

            public const string Farmer = "farmer";

            public const string Herder = "herder";

            public const string Healer = "healer";

            public const string Mercenary = "mercenary";

            public const string Infantry = "infantry";

            public const string Skirmisher = "skirmisher";

            public const string Kern = "kern";

            public const string Guard = "guard";

            public const string RetainerUrban = "retainer_urban";

            public const string MercenaryUrban = "mercenary_urban";

            public const string MerchantUrban = "merchant_urban";

            public const string VagabondUrban = "vagabond_urban";

            public const string ArtisanUrban = "artisan_urban";

            public const string PhysicianUrban = "physician_urban";

            public const string HealerUrban = "healer_urban";

            public const string BardUrban = "bard_urban";
        }
    }
}
