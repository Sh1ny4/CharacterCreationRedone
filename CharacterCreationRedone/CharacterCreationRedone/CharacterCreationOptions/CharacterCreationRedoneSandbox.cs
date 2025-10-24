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
            __instance.ParentsMenu(characterCreationManager);
            __instance.EducationMenu(characterCreationManager);
            __instance.FavoriteIdiomMenu(characterCreationManager);
            __instance.StartInLifeMenu(characterCreationManager);
            __instance.ReasonMenu(characterCreationManager);
            __instance.AgeSelectionMenu(characterCreationManager);
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
        public void ParentsMenu(CharacterCreationManager characterCreationManager)
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

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_ARais", new TextObject("{=CCR_Family_Choice_ARais}rais", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceARaisOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceARaisOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_AMamluks", new TextObject("{=CCR_Family_Choice_AMamluks}mamluks", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceAMamluksOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceAMamluksOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_AMerchant", new TextObject("{=CCR_Family_Choice_AMerchant}merchants", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceAMerchantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceAMerchantOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_AFarmers", new TextObject("{=CCR_Family_Choice_AFarmers}farmers", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceAFarmerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceAFarmerOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_AArtisans", new TextObject("{=CCR_Family_Choice_AArtisans}artisans", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceAArtisansOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceAArtisansOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_AThugs", new TextObject("{=CCR_Family_Choice_AThugs}thugs", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceAThugsOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceAThugsOptionOnSelect), null));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Bchieftains", new TextObject("{=CCR_Family_Choice_Bchieftains}chieftains", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceBchieftainsOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattanianParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceBchieftainsOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Bhealers", new TextObject("{=CCR_Family_Choice_Bhealers}healers", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceBHealersOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattanianParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceBHealersOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Bfarmers", new TextObject("{=CCR_Family_Choice_Bfarmers}farmers", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceBFarmersOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattanianParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceBFarmersOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Bartisans", new TextObject("{=CCR_Family_Choice_Bartisans}artisans", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceBArtisansOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattanianParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceBArtisansOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Bforesters", new TextObject("{=CCR_Family_Choice_Bforesters}foresters", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceBForestersOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattanianParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceBForestersOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Bbards", new TextObject("{=CCR_Family_Choice_Bbards}bards", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceBBardsOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattanianParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceBBardsOptionOnSelect), null));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Earistocrates", new TextObject("{=CCR_Family_Choice_Earistocrates}aristocrates", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceEAristocratesOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceEAristocratesOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Emerchants", new TextObject("{=CCR_Family_Choice_Emerchants}merchants", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceEMerchantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceEMerchantOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Efreeholders", new TextObject("{=CCR_Family_Choice_Efreeholders}freeholders", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceEFreeholderOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceEFreeholderOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Eartisans", new TextObject("{=CCR_Family_Choice_Eartisans}artisans", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceEArtisansOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceEArtisansOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Esoldiers", new TextObject("{=CCR_Family_Choice_Esoldiers}soldiers", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceESoldiersOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceESoldiersOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Evagabonds", new TextObject("{=CCR_Family_Choice_Evagabonds}vagabonds", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceEVagabonsOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceEVagabonsOptionOnSelect), null));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Knoyans", new TextObject("{=CCR_Family_Choice_Knoyans}noyans", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceKNoyansOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceKNoyansOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Knomads", new TextObject("{=CCR_Family_Choice_Knomads}nomads", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceKNomadsOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceKNomadsOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Kmerchants", new TextObject("{=CCR_Family_Choice_Kmerchants}merchants", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceKMerchantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceKMerchantOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Kartisans", new TextObject("{=CCR_Family_Choice_Kartisans}artisans", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceKArtisansOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceKArtisansOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Kwarriors", new TextObject("{=CCR_Family_Choice_Kwarriors}warriors", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceKWarriorsOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceKWarriorsOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Kthugs", new TextObject("{=CCR_Family_Choice_Kthugs}thugs", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceKThugsOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceKThugsOptionOnSelect), null));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Sboyars", new TextObject("{=CCR_Family_Choice_Sboyars}boyars", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceSBoyarsOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgianParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceSBoyarsOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Smerchants", new TextObject("{=CCR_Family_Choice_Smerchants}merchants", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceSMerchantsOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgianParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceSMerchantsOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Sfarmers", new TextObject("{=CCR_Family_Choice_Sfarmers}farmers", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceSFarmersOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgianParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceSFarmersOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Sartisans", new TextObject("{=CCR_Family_Choice_Sartisans}artisans", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceSArtisansOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgianParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceSArtisansOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Swarriors", new TextObject("{=CCR_Family_Choice_Swarriors}warriors", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceSWarriorsOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgianParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceSWarriorsOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Sraiders", new TextObject("{=CCR_Family_Choice_Sraiders}raiders", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceSRaidersOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgianParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceSRaidersOptionOnSelect), null));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Vbarons", new TextObject("{=CCR_Family_Choice_Vbarons}barons", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceVBaronsOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandianParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceVBaronsOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Vmerchants", new TextObject("{=CCR_Family_Choice_Vmerchants}merchants", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceVMerchantsOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandianParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceVMerchantsOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Vyeomens", new TextObject("{=CCR_Family_Choice_Vyeomens}yeomen", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceVYeomensOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandianParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceVYeomensOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Vartisans", new TextObject("{=CCR_Family_Choice_Vartisans}artisans", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceVArtisansOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandianParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceVArtisansOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Vsoldiers", new TextObject("{=CCR_Family_Choice_Vsoldiers}soldiers", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceVSoldiersOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandianParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceVSoldiersOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Family_Choice_Vmercenaries", new TextObject("{=CCR_Family_Choice_Vmercenaries}mercenaries", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.FamilyChoiceVMercenariesOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandianParentsOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.FamilyChoiceVMercenariesOptionOnSelect), null));

            characterCreationManager.AddNewMenu(narrativeMenu);
        }
        public bool AseraiParentsOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }
        public void FamilyChoiceARaisOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.Polearm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }
        public void FamilyChoiceARaisOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceAMamluksOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }
        public void FamilyChoiceAMamluksOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceAMerchantOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Athletics, DefaultSkills.Polearm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public void FamilyChoiceAMerchantOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceAFarmerOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Crossbow };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public void FamilyChoiceAFarmerOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceAArtisansOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting, DefaultSkills.Bow };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }
        public void FamilyChoiceAArtisansOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceAThugsOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Throwing };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public void FamilyChoiceAThugsOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public bool BattanianParentsOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }
        public void FamilyChoiceBchieftainsOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.Polearm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }
        public void FamilyChoiceBchieftainsOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceBHealersOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public void FamilyChoiceBHealersOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceBFarmersOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.Crossbow };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public void FamilyChoiceBFarmersOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceBArtisansOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.TwoHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }
        public void FamilyChoiceBArtisansOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceBForestersOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting, DefaultSkills.Crossbow };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }
        public void FamilyChoiceBForestersOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceBBardsOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Crossbow };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public void FamilyChoiceBBardsOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public bool EmpireParentsOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }
        public void FamilyChoiceEAristocratesOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.TwoHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }
        public void FamilyChoiceEAristocratesOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceEMerchantOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Tactics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public void FamilyChoiceEMerchantOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceEFreeholderOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Athletics, DefaultSkills.Polearm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public void FamilyChoiceEFreeholderOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceEArtisansOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.OneHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public void FamilyChoiceEArtisansOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceESoldiersOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting, DefaultSkills.Bow };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }
        public void FamilyChoiceESoldiersOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceEVagabonsOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Throwing };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }
        public void FamilyChoiceEVagabonsOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public bool KhuzaitParentsOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }
        public void FamilyChoiceKNoyansOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.Throwing };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }
        public void FamilyChoiceKNoyansOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceKNomadsOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.Polearm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }
        public void FamilyChoiceKNomadsOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceKMerchantOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Medicine, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public void FamilyChoiceKMerchantOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceKArtisansOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Athletics, DefaultSkills.OneHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public void FamilyChoiceKArtisansOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceKWarriorsOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting, DefaultSkills.Bow };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public void FamilyChoiceKWarriorsOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceKThugsOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Polearm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }
        public void FamilyChoiceKThugsOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public bool SturgianParentsOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }
        public void FamilyChoiceSBoyarsOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.TwoHanded, DefaultSkills.Bow };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }
        public void FamilyChoiceSBoyarsOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceSMerchantsOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Medicine, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public void FamilyChoiceSMerchantsOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceSFarmersOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Athletics, DefaultSkills.Throwing };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }
        public void FamilyChoiceSFarmersOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceSArtisansOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.TwoHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public void FamilyChoiceSArtisansOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceSWarriorsOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting, DefaultSkills.Tactics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public void FamilyChoiceSWarriorsOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceSRaidersOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }
        public void FamilyChoiceSRaidersOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public bool VlandianParentsOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }
        public void FamilyChoiceVBaronsOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.Polearm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public void FamilyChoiceVBaronsOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceVMerchantsOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }
        public void FamilyChoiceVMerchantsOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceVYeomensOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Bow, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }
        public void FamilyChoiceVYeomensOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceVArtisansOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.Throwing };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }
        public void FamilyChoiceVArtisansOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceVSoldiersOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Medicine, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public void FamilyChoiceVSoldiersOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceVMercenariesOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public void FamilyChoiceVMercenariesOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        /// <summary>
        /// Education Menu
        /// </summary>
        public List<NarrativeMenuCharacterArgs> AddEducationMenuCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            List<NarrativeMenuCharacterArgs> list = new List<NarrativeMenuCharacterArgs>();
            string playerChildhoodAgeEquipmentId = this.GetPlayerChildhoodAgeEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            list.Add(new NarrativeMenuCharacterArgs("player_childhood_character", 7, playerChildhoodAgeEquipmentId, "act_childhood_schooled", "spawnpoint_player_1", "", "", null, true, CharacterObject.PlayerCharacter.IsFemale));
            return list;
        }
        new public void EducationMenu(CharacterCreationManager characterCreationManager)
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Medicine, DefaultSkills.Athletics };
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

        /// <summary>
        /// Youth menu
        /// </summary>
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
        public void StartInLifeMenu(CharacterCreationManager characterCreationManager)
        {
            BodyProperties bodyProperties = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1);
            bodyProperties = FaceGen.GetBodyPropertiesWithAge(ref bodyProperties, 17f);
            List<NarrativeMenuCharacter> list = new List<NarrativeMenuCharacter>();
            list.Add(new NarrativeMenuCharacter("player_youth_character", bodyProperties, CharacterObject.PlayerCharacter.Race, CharacterObject.PlayerCharacter.IsFemale));
            NarrativeMenu narrativeMenu = new NarrativeMenu("narrative_youth_menu", "narrative_education_menu", "narrative_adulthood_menu", new TextObject("{=!}Start in life", null), new TextObject("{=!}You started your life as...", null), list, new NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate(this.StartInLifeMenuCharacterArgs));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Afaris", new TextObject("{=CCR_Start_Choice_Afaris}a faris", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAseraiFarisOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAseraiFarisOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAseraiFarisOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAseraiFarisOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Acaravaner", new TextObject("{=CCR_Start_Choice_Acaravaner}a caravaner", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAseraiCaravaneerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAseraiCaravaneerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAseraiCaravaneerOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAseraiCaravaneerOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Amerchant", new TextObject("{=CCR_Start_Choice_Amerchant}a merchant", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAseraiMerchantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAseraiMerchantOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAseraiMerchantOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAseraiMerchantOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Acraftman", new TextObject("{=CCR_Start_Choice_Acraftman}a craftman", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAseraiCraftmanOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAseraiCraftmanOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAseraiCraftmanOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAseraiCraftmanOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Afarmer", new TextObject("{=CCR_Start_Choice_Afarmer}a farmer", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAseraiFarmerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAseraiFarmerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAseraiFarmerOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAseraiFarmerOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Amamluke", new TextObject("{=CCR_Start_Choice_Amamluke}mamluke", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAseraiMamlukeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAseraiMamlukeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAseraiMamlukeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAseraiMamlukeOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Ahorsearcher", new TextObject("{=CCR_Start_Choice_Ahorsearcher}mounted archer", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAseraiHorseArcherOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAseraiHorseArcherOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAseraiHorseArcherOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAseraiHorseArcherOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Aarcher", new TextObject("{=CCR_Start_Choice_Aarcher}archer", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAseraiArcherOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAseraiArcherOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAseraiArcherOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAseraiArcherOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Adesertbandit", new TextObject("{=CCR_Start_Choice_Adesertbandit}a desert bandit", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartAseraiDesertBanditOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartAseraiDesertBanditOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartAseraiDesertBanditOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartAseraiDesertBanditOptionOnConsequence)));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Bfian", new TextObject("{=CCR_Start_Choice_Bfian}a member of a fianna", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartBattaniaFiannaOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartBattaniaFiannaOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartBattaniaFiannaOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartBattaniaFiannaOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Bdruid", new TextObject("{=CCR_Start_Choice_Bdruid}a druid", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartBattaniaDruidOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartBattaniaDruidOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartBattaniaDruidOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartBattaniaDruidOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Bmerchant", new TextObject("{=CCR_Start_Choice_Bmerchant}a merchant", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartBattaniaMerchantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartBattaniaMerchantOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartBattaniaMerchantOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartBattaniaMerchantOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Bcraftman", new TextObject("{=CCR_Start_Choice_Bcraftman}a craftman", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartBattaniaCraftmanOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartBattaniaCraftmanOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartBattaniaCraftmanOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartBattaniaCraftmanOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Bforester", new TextObject("{=CCR_Start_Choice_Bforester}a forester", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartBattaniaForesterOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartBattaniaForesterOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartBattaniaForesterOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartBattaniaForesterOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Bwildling", new TextObject("{=CCR_Start_Choice_Bwildling}wildling", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartBattaniaForesterOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartBattaniaWildlingOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartBattaniaWildlingOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartBattaniaWildlingOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Bscout", new TextObject("{=CCR_Start_Choice_Bscout}scout", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartBattaniaScoutOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartBattaniaScoutOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartBattaniaScoutOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartBattaniaScoutOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Bkern", new TextObject("{=CCR_Start_Choice_Bkern}part of the kern", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartBattaniaKernOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartBattaniaKernOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartBattaniaKernOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartBattaniaKernOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Bforestbandit", new TextObject("{=CCR_Start_Choice_Bforestbandit}a forest bandit", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartBattaniaForestBanditOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartBattaniaForestBanditOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartBattaniaForestBanditOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartBattaniaForestBanditOptionOnConsequence)));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Ecommander", new TextObject("{=CCR_Start_Choice_Ecommander}a centurion", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartEmpireCommanderOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartEmpireCommanderOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartEmpireCommanderOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartEmpireCommanderOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Eengineer", new TextObject("{=CCR_Start_Choice_Eengineer}an engineer's apprentice", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartEmpireEngineerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartEmpireEngineerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartEmpireEngineerOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartEmpireEngineerOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Emerchant", new TextObject("{=CCR_Start_Choice_Emerchant}a merchant", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartEmpireMerchantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartEmpireMerchantOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartEmpireMerchantOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartEmpireMerchantOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Ecraftman", new TextObject("{=CCR_Start_Choice_Ecraftman}a craftman", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartEmpireCraftmanOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartEmpireCraftmanOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartEmpireCraftmanOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartEmpireCraftmanOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Epeasant", new TextObject("{=CCR_Start_Choice_Epeasant}a peasant", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartEmpirePeasantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartEmpirePeasantOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartEmpirePeasantOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartEmpirePeasantOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Elegionary", new TextObject("{=CCR_Start_Choice_Elegionary}legionary", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartEmpireLegionaryOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartEmpireLegionaryOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartEmpireLegionaryOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartEmpireLegionaryOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Earcher", new TextObject("{=CCR_Start_Choice_Earcher}archer", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartEmpireArcherOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartEmpireArcherOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartEmpireArcherOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartEmpireArcherOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Ecavalry", new TextObject("{=CCR_Start_Choice_Ecavalry}light cavalry", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartEmpireCavalryOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartEmpireCavalryOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartEmpireCavalryOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartEmpireCavalryOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Ehorsearcher", new TextObject("{=CCR_Start_Choice_Ehorsearcher}mounted archer", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartEmpireHorseArcherOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartEmpireHorseArcherOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartEmpireHorseArcherOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartEmpireHorseArcherOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Elooter", new TextObject("{=CCR_Start_Choice_Elooter}a looter", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartEmpireLooterOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartEmpireLooterOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartEmpireLooterOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartEmpireLooterOptionOnConsequence)));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Kkhanguard", new TextObject("{=CCR_Start_Choice_Kkhanguard}part of a khan's guard", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartKhuzaitKhanGuardOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartKhuzaitKhanGuardOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartKhuzaitKhanGuardOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartKhuzaitKhanGuardOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Knomad", new TextObject("{=CCR_Start_Choice_Knomad}a nomad", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartKhuzaitNomadOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartKhuzaitNomadOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartKhuzaitNomadOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartKhuzaitNomadOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Kmerchant", new TextObject("{=CCR_Start_Choice_Kmerchant}a merchant", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartKhuzaitMerchantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartKhuzaitMerchantOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartKhuzaitMerchantOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartKhuzaitMerchantOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Kcraftman", new TextObject("{=CCR_Start_Choice_Kcraftman}a craftman", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartKhuzaitCraftmanOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartKhuzaitCraftmanOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartKhuzaitCraftmanOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartKhuzaitCraftmanOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Kfarmer", new TextObject("{=CCR_Start_Choice_Kfarmer}a farmer", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartKhuzaitFarmerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartKhuzaitFarmerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartKhuzaitFarmerOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartKhuzaitFarmerOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Kcavalry", new TextObject("{=CCR_Start_Choice_Kcavalry}part of the cavalry", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartKhuzaitCavalryOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartKhuzaitCavalryOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartKhuzaitCavalryOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartKhuzaitCavalryOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Khorsearcher", new TextObject("{=CCR_Start_Choice_Khorsearcher}mounted archer", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartKhuzaitHorseArcherOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartKhuzaitHorseArcherOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartKhuzaitHorseArcherOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartKhuzaitHorseArcherOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Kinfantry", new TextObject("{=CCR_Start_Choice_Kinfantry}infantry", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartKhuzaitInfantryOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartKhuzaitInfantryOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartKhuzaitInfantryOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartKhuzaitInfantryOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Ksteppebandit", new TextObject("{=CCR_Start_Choice_Ksteppebandit}a steppe bandit", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartKhuzaitSteppeBanditOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartKhuzaitSteppeBanditOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartKhuzaitSteppeBanditOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartKhuzaitSteppeBanditOptionOnConsequence)));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Sdruzhina", new TextObject("{=CCR_Start_Choice_Sdruzhina}a member of a druzhina", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartSturgiaDruzhinaOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartSturgiaDruzhinaOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartSturgiaDruzhinaOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartSturgiaDruzhinaOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Sfur_hunter", new TextObject("{=CCR_Start_Choice_Sfur_hunter}fur hunter", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartSturgiaFurHunterOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartSturgiaFurHunterOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartSturgiaFurHunterOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartSturgiaFurHunterOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Smerchant", new TextObject("{=CCR_Start_Choice_Smerchant}a merchant", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartSturgiaMerchantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartSturgiaMerchantOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartSturgiaMerchantOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartSturgiaMerchantOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Scraftman", new TextObject("{=CCR_Start_Choice_Scraftman}a craftman", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartSturgiaCraftmanOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartSturgiaCraftmanOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartSturgiaCraftmanOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartSturgiaCraftmanOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Speasant", new TextObject("{=CCR_Start_Choice_Speasant}a peasant", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartSturgiaPeasantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartSturgiaPeasantOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartSturgiaPeasantOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartSturgiaPeasantOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Sinfantry", new TextObject("{=CCR_Start_Choice_Sinfantry}part of the infantry", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartSturgiaInfantryOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartSturgiaInfantryOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartSturgiaInfantryOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartSturgiaInfantryOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Sshocktroop", new TextObject("{=CCR_Start_Choice_Sshocktroop}shock troop", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartSturgiaShockTroopOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartSturgiaShockTroopOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartSturgiaShockTroopOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartSturgiaShockTroopOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Sarcher", new TextObject("{=CCR_Start_Choice_Sarcher}bowman", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartSturgiaArcherOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartSturgiaArcherOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartSturgiaArcherOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartSturgiaArcherOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Ssearaider", new TextObject("{=CCR_Start_Choice_Ssearaider}a raider", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartSturgiaSeaRaiderBanditOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartSturgiaSeaRaiderBanditOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartSturgiaSeaRaiderBanditOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartSturgiaSeaRaiderBanditOptionOnConsequence)));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Vknight", new TextObject("{=CCR_Start_Choice_Vknight}a knight", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartVlandiaKnightOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartVlandiaKnightOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartVlandiaKnightOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartVlandiaKnightOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Vchamberlain", new TextObject("{=CCR_Start_Choice_Vchamberlain}chamberlain", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartVlandiaChamberlainOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartVlandiaChamberlainOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartVlandiaChamberlainOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartVlandiaChamberlainOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Vmerchant", new TextObject("{=CCR_Start_Choice_Vmerchant}a merchant", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartVlandiaMerchantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartVlandiaMerchantOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartVlandiaMerchantOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartVlandiaMerchantOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Vguild", new TextObject("{=CCR_Start_Choice_Vguild}a member of a guild", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartVlandiaGuildMemberOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartVlandiaGuildMemberOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartVlandiaGuildMemberOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartVlandiaGuildMemberOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Vserf", new TextObject("{=CCR_Start_Choice_Vserf}a serf", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartVlandiaSerfOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartVlandiaSerfOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartVlandiaSerfOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartVlandiaSerfOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Vinfantry", new TextObject("{=CCR_Start_Choice_Vinfantry}levied footman", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartVlandiaInfantryOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartVlandiaInfantryOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartVlandiaInfantryOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartVlandiaInfantryOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Vcavalry", new TextObject("{=CCR_Start_Choice_Vcavalry}light cavalry", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartVlandiaCavalryOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartVlandiaCavalryOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartVlandiaCavalryOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartVlandiaCavalryOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Vcrossbowman", new TextObject("{=CCR_Start_Choice_Vcrossbowman}levied crossbowman", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartVlandiaRangedOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartVlandiaRangedOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartVlandiaRangedOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartVlandiaRangedOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Vbandit", new TextObject("{=CCR_Start_Choice_Vbandit}a highwayman", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartVlandiaMountainBanditOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartVlandiaMountainBanditOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartVlandiaMountainBanditOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartVlandiaMountainBanditOptionOnConsequence)));

            characterCreationManager.AddNewMenu(narrativeMenu);
        }
        public void LifeStartAseraiFarisOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.OneHanded, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public bool LifeStartAseraiFarisOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Retainer && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }
        public void LifeStartAseraiFarisOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartAseraiFarisOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
            Hero ruler = Hero.FindAll(hero => hero.Culture == Hero.MainHero.Culture && hero.IsAlive && hero.IsFactionLeader && !hero.MapFaction.IsMinorFaction).GetRandomElementInefficiently();
            ChangeKingdomAction.ApplyByJoinToKingdom(Hero.MainHero.Clan, ruler.Clan.Kingdom, default, false);
            CharacterObject wanderer = (from character in CharacterObject.All where character.Occupation == Occupation.Wanderer && character.Culture == Hero.MainHero.Culture select character).GetRandomElementInefficiently();
            Hero companion = HeroCreator.CreateSpecialHero(wanderer);
            AddCompanionAction.Apply(Clan.PlayerClan, companion);
            AddHeroToPartyAction.Apply(companion, Hero.MainHero.PartyBelongedTo);
        }
        public void LifeStartAseraiCaravaneerOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting, DefaultSkills.Trade, DefaultSkills.Leadership };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }
        public bool LifeStartAseraiCaravaneerOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }
        public void LifeStartAseraiCaravaneerOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartAseraiCaravaneerOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartAseraiMerchantOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Steward, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public bool LifeStartAseraiMerchantOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }
        public void LifeStartAseraiMerchantOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartAseraiMerchantOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartAseraiCraftmanOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Trade, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public bool LifeStartAseraiCraftmanOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }
        public void LifeStartAseraiCraftmanOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartAseraiCraftmanOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartAseraiFarmerOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Steward, DefaultSkills.Medicine };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public bool LifeStartAseraiFarmerOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }
        public void LifeStartAseraiFarmerOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartAseraiFarmerOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartAseraiMamlukeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Polearm, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartAseraiMamlukeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }
        public void LifeStartAseraiMamlukeOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartAseraiMamlukeOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartAseraiHorseArcherOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartAseraiHorseArcherOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }
        public void LifeStartAseraiHorseArcherOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartAseraiHorseArcherOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartAseraiArcherOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartAseraiArcherOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }
        public void LifeStartAseraiArcherOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartAseraiArcherOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartAseraiDesertBanditOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Throwing, DefaultSkills.OneHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartAseraiDesertBanditOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }
        public void LifeStartAseraiDesertBanditOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartAseraiDesertBanditOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        
        public void LifeStartBattaniaFiannaOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.OneHanded, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public bool LifeStartBattaniaFiannaOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Retainer && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }
        public void LifeStartBattaniaFiannaOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartBattaniaFiannaOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
            Hero ruler = Hero.FindAll(hero => hero.Culture == Hero.MainHero.Culture && hero.IsAlive && hero.IsFactionLeader && !hero.MapFaction.IsMinorFaction).GetRandomElementInefficiently();
            ChangeKingdomAction.ApplyByJoinToKingdom(Hero.MainHero.Clan, ruler.Clan.Kingdom, default, false);
            CharacterObject wanderer = (from character in CharacterObject.All where character.Occupation == Occupation.Wanderer && character.Culture == Hero.MainHero.Culture select character).GetRandomElementInefficiently();
            Hero companion = HeroCreator.CreateSpecialHero(wanderer);
            AddCompanionAction.Apply(Clan.PlayerClan, companion);
            AddHeroToPartyAction.Apply(companion, Hero.MainHero.PartyBelongedTo);
        }
        public void LifeStartBattaniaDruidOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting, DefaultSkills.Trade, DefaultSkills.Leadership };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }
        public bool LifeStartBattaniaDruidOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }
        public void LifeStartBattaniaDruidOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartBattaniaDruidOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartBattaniaMerchantOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Steward, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public bool LifeStartBattaniaMerchantOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }
        public void LifeStartBattaniaMerchantOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartBattaniaMerchantOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartBattaniaCraftmanOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Trade, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public bool LifeStartBattaniaCraftmanOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }
        public void LifeStartBattaniaCraftmanOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartBattaniaCraftmanOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartBattaniaForesterOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Steward, DefaultSkills.Medicine };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public bool LifeStartBattaniaForesterOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }
        public void LifeStartBattaniaForesterOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartBattaniaForesterOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartBattaniaWildlingOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Polearm, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartBattaniaWildlingOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }
        public void LifeStartBattaniaWildlingOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartBattaniaWildlingOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartBattaniaScoutOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartBattaniaScoutOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }
        public void LifeStartBattaniaScoutOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartBattaniaScoutOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartBattaniaKernOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartBattaniaKernOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }
        public void LifeStartBattaniaKernOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartBattaniaKernOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartBattaniaForestBanditOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Throwing, DefaultSkills.OneHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartBattaniaForestBanditOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }
        public void LifeStartBattaniaForestBanditOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartBattaniaForestBanditOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        
        public void LifeStartEmpireCommanderOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.OneHanded, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public bool LifeStartEmpireCommanderOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Retainer && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }
        public void LifeStartEmpireCommanderOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartEmpireCommanderOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
            Hero ruler = Hero.FindAll(hero => hero.Culture == Hero.MainHero.Culture && hero.IsAlive && hero.IsFactionLeader && !hero.MapFaction.IsMinorFaction).GetRandomElementInefficiently();
            ChangeKingdomAction.ApplyByJoinToKingdom(Hero.MainHero.Clan, ruler.Clan.Kingdom, default, false);
            CharacterObject wanderer = (from character in CharacterObject.All where character.Occupation == Occupation.Wanderer && character.Culture == Hero.MainHero.Culture select character).GetRandomElementInefficiently();
            Hero companion = HeroCreator.CreateSpecialHero(wanderer);
            AddCompanionAction.Apply(Clan.PlayerClan, companion);
            AddHeroToPartyAction.Apply(companion, Hero.MainHero.PartyBelongedTo);
        }
        public void LifeStartEmpireEngineerOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting, DefaultSkills.Trade, DefaultSkills.Leadership };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }
        public bool LifeStartEmpireEngineerOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }
        public void LifeStartEmpireEngineerOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartEmpireEngineerOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartEmpireMerchantOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Steward, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public bool LifeStartEmpireMerchantOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }
        public void LifeStartEmpireMerchantOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartEmpireMerchantOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartEmpireCraftmanOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Trade, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public bool LifeStartEmpireCraftmanOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }
        public void LifeStartEmpireCraftmanOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartEmpireCraftmanOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartEmpirePeasantOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Steward, DefaultSkills.Medicine };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public bool LifeStartEmpirePeasantOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }
        public void LifeStartEmpirePeasantOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartEmpirePeasantOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartEmpireLegionaryOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Polearm, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartEmpireLegionaryOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }
        public void LifeStartEmpireLegionaryOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartEmpireLegionaryOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartEmpireArcherOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartEmpireArcherOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }
        public void LifeStartEmpireArcherOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartEmpireArcherOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartEmpireCavalryOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartEmpireCavalryOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }
        public void LifeStartEmpireCavalryOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartEmpireCavalryOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartEmpireHorseArcherOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartEmpireHorseArcherOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }
        public void LifeStartEmpireHorseArcherOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartEmpireHorseArcherOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartEmpireLooterOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Throwing, DefaultSkills.OneHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartEmpireLooterOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }
        public void LifeStartEmpireLooterOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartEmpireLooterOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }

        public void LifeStartKhuzaitKhanGuardOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.OneHanded, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public bool LifeStartKhuzaitKhanGuardOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Retainer && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }
        public void LifeStartKhuzaitKhanGuardOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartKhuzaitKhanGuardOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
            Hero ruler = Hero.FindAll(hero => hero.Culture == Hero.MainHero.Culture && hero.IsAlive && hero.IsFactionLeader && !hero.MapFaction.IsMinorFaction).GetRandomElementInefficiently();
            ChangeKingdomAction.ApplyByJoinToKingdom(Hero.MainHero.Clan, ruler.Clan.Kingdom, default, false);
            CharacterObject wanderer = (from character in CharacterObject.All where character.Occupation == Occupation.Wanderer && character.Culture == Hero.MainHero.Culture select character).GetRandomElementInefficiently();
            Hero companion = HeroCreator.CreateSpecialHero(wanderer);
            AddCompanionAction.Apply(Clan.PlayerClan, companion);
            AddHeroToPartyAction.Apply(companion, Hero.MainHero.PartyBelongedTo);
        }
        public void LifeStartKhuzaitNomadOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting, DefaultSkills.Trade, DefaultSkills.Leadership };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }
        public bool LifeStartKhuzaitNomadOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }
        public void LifeStartKhuzaitNomadOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartKhuzaitNomadOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartKhuzaitMerchantOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Steward, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public bool LifeStartKhuzaitMerchantOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }
        public void LifeStartKhuzaitMerchantOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartKhuzaitMerchantOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartKhuzaitCraftmanOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Trade, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public bool LifeStartKhuzaitCraftmanOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }
        public void LifeStartKhuzaitCraftmanOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartKhuzaitCraftmanOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartKhuzaitFarmerOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Steward, DefaultSkills.Medicine };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public bool LifeStartKhuzaitFarmerOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }
        public void LifeStartKhuzaitFarmerOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartKhuzaitFarmerOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartKhuzaitCavalryOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Polearm, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartKhuzaitCavalryOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }
        public void LifeStartKhuzaitCavalryOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartKhuzaitCavalryOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartKhuzaitHorseArcherOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartKhuzaitHorseArcherOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }
        public void LifeStartKhuzaitHorseArcherOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartKhuzaitHorseArcherOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartKhuzaitInfantryOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartKhuzaitInfantryOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }
        public void LifeStartKhuzaitInfantryOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartKhuzaitInfantryOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartKhuzaitSteppeBanditOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Throwing, DefaultSkills.OneHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartKhuzaitSteppeBanditOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }
        public void LifeStartKhuzaitSteppeBanditOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartKhuzaitSteppeBanditOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }

        public void LifeStartSturgiaDruzhinaOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.OneHanded, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public bool LifeStartSturgiaDruzhinaOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Retainer && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }
        public void LifeStartSturgiaDruzhinaOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartSturgiaDruzhinaOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
            Hero ruler = Hero.FindAll(hero => hero.Culture == Hero.MainHero.Culture && hero.IsAlive && hero.IsFactionLeader && !hero.MapFaction.IsMinorFaction).GetRandomElementInefficiently();
            ChangeKingdomAction.ApplyByJoinToKingdom(Hero.MainHero.Clan, ruler.Clan.Kingdom, default, false);
            CharacterObject wanderer = (from character in CharacterObject.All where character.Occupation == Occupation.Wanderer && character.Culture == Hero.MainHero.Culture select character).GetRandomElementInefficiently();
            Hero companion = HeroCreator.CreateSpecialHero(wanderer);
            AddCompanionAction.Apply(Clan.PlayerClan, companion);
            AddHeroToPartyAction.Apply(companion, Hero.MainHero.PartyBelongedTo);
        }
        public void LifeStartSturgiaFurHunterOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting, DefaultSkills.Trade, DefaultSkills.Leadership };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }
        public bool LifeStartSturgiaFurHunterOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }
        public void LifeStartSturgiaFurHunterOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartSturgiaFurHunterOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartSturgiaMerchantOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Steward, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public bool LifeStartSturgiaMerchantOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }
        public void LifeStartSturgiaMerchantOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartSturgiaMerchantOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartSturgiaCraftmanOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Trade, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public bool LifeStartSturgiaCraftmanOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }
        public void LifeStartSturgiaCraftmanOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartSturgiaCraftmanOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartSturgiaPeasantOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Steward, DefaultSkills.Medicine };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public bool LifeStartSturgiaPeasantOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }
        public void LifeStartSturgiaPeasantOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartSturgiaPeasantOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartSturgiaInfantryOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Polearm, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartSturgiaInfantryOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }
        public void LifeStartSturgiaInfantryOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartSturgiaInfantryOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartSturgiaShockTroopOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartSturgiaShockTroopOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }
        public void LifeStartSturgiaShockTroopOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartSturgiaShockTroopOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartSturgiaArcherOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartSturgiaArcherOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }
        public void LifeStartSturgiaArcherOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartSturgiaArcherOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartSturgiaSeaRaiderBanditOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Throwing, DefaultSkills.OneHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartSturgiaSeaRaiderBanditOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }
        public void LifeStartSturgiaSeaRaiderBanditOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartSturgiaSeaRaiderBanditOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }

        public void LifeStartVlandiaKnightOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.OneHanded, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public bool LifeStartVlandiaKnightOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Retainer && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }
        public void LifeStartVlandiaKnightOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartVlandiaKnightOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
            Hero ruler = Hero.FindAll(hero => hero.Culture == Hero.MainHero.Culture && hero.IsAlive && hero.IsFactionLeader && !hero.MapFaction.IsMinorFaction).GetRandomElementInefficiently();
            ChangeKingdomAction.ApplyByJoinToKingdom(Hero.MainHero.Clan, ruler.Clan.Kingdom, default, false);
            CharacterObject wanderer = (from character in CharacterObject.All where character.Occupation == Occupation.Wanderer && character.Culture == Hero.MainHero.Culture select character).GetRandomElementInefficiently();
            Hero companion = HeroCreator.CreateSpecialHero(wanderer);
            AddCompanionAction.Apply(Clan.PlayerClan, companion);
            AddHeroToPartyAction.Apply(companion, Hero.MainHero.PartyBelongedTo);
        }
        public void LifeStartVlandiaChamberlainOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting, DefaultSkills.Trade, DefaultSkills.Leadership };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }
        public bool LifeStartVlandiaChamberlainOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }
        public void LifeStartVlandiaChamberlainOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartVlandiaChamberlainOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartVlandiaMerchantOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Steward, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public bool LifeStartVlandiaMerchantOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }
        public void LifeStartVlandiaMerchantOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartVlandiaMerchantOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartVlandiaGuildMemberOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Trade, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public bool LifeStartVlandiaGuildMemberOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }
        public void LifeStartVlandiaGuildMemberOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartVlandiaGuildMemberOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartVlandiaSerfOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Steward, DefaultSkills.Medicine };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public bool LifeStartVlandiaSerfOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }
        public void LifeStartVlandiaSerfOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartVlandiaSerfOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartVlandiaInfantryOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Polearm, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartVlandiaInfantryOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }
        public void LifeStartVlandiaInfantryOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartVlandiaInfantryOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartVlandiaCavalryOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartVlandiaCavalryOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }
        public void LifeStartVlandiaCavalryOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartVlandiaCavalryOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartVlandiaRangedOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartVlandiaRangedOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }
        public void LifeStartVlandiaRangedOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartVlandiaRangedOptionOnConsequence(CharacterCreationManager characterCreationManager)
        {
        }
        public void LifeStartVlandiaMountainBanditOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Throwing, DefaultSkills.OneHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartVlandiaMountainBanditOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }
        public void LifeStartVlandiaMountainBanditOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void LifeStartVlandiaMountainBanditOptionOnConsequence(CharacterCreationManager characterCreationManager)
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
        public void ReasonMenu(CharacterCreationManager characterCreationManager)
        {
            BodyProperties bodyProperties = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1);
            bodyProperties = FaceGen.GetBodyPropertiesWithAge(ref bodyProperties, 20f);
            List<NarrativeMenuCharacter> list = new List<NarrativeMenuCharacter>();
            list.Add(new NarrativeMenuCharacter("player_adulthood_character", bodyProperties, CharacterObject.PlayerCharacter.Race, CharacterObject.PlayerCharacter.IsFemale));
            MBTextManager.SetTextVariable("EXP_VALUE", 30);
            NarrativeMenu narrativeMenu = new NarrativeMenu("narrative_adulthood_menu", "narrative_youth_menu", "narrative_age_selection_menu", new TextObject("{=!}Reason for Adventuring", null), new TextObject("{=!}You started adventuring...", null), list, new NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate(this.GetReasonMenuCharacterArgs));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Reason_travel", new TextObject("{=CCR_Reason_travel}to discover the world.", null), new TextObject("{=CCR_Reason_desc_travel}The temptation of travel was to much for you, as you always dreamt of seeing the world.", null), new GetNarrativeMenuOptionArgsDelegate(this.ReasonTravelOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ReasonOptionsOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ReasonTravelOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Reason_revenge", new TextObject("{=CCR_Reason_revenge}to take revenge.", null), new TextObject("{=CCR_Reason_desc_revenge}After being wronged, you felt the need for revenge. With that goal in mind, you wandered throughout Calradia to get reparations.", null), new GetNarrativeMenuOptionArgsDelegate(this.ReasonRevengeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ReasonOptionsOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ReasonRevengeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Reason_forced_out", new TextObject("{=CCR_Reason_forced_out}after being forced out.", null), new TextObject("{=CCR_Reason_desc_forced_out}After one last disagreement with you, your parents forced you out. With nowhere to go, adventuring was all you could do.", null), new GetNarrativeMenuOptionArgsDelegate(this.ReasonForcedOutOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ReasonOptionsOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ReasonForcedOutOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Reason_money", new TextObject("{=CCR_Reason_money}in search of money", null), new TextObject("{=CCR_Reason_desc_money}You always wanted to make riches, and it was obvious for you that staying at home would never allow you to do it.", null), new GetNarrativeMenuOptionArgsDelegate(this.ReasonMoneyOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ReasonOptionsOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ReasonMoneyOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Reason_power", new TextObject("{=CCR_Reason_power}to become one of the powerful", null), new TextObject("{=CCR_Reason_desc_power}Seeing how those in power had many advantages, you joined on an adventure to join them, or even replace them.", null), new GetNarrativeMenuOptionArgsDelegate(this.ReasonPowerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ReasonOptionsOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ReasonPowerOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Reason_history", new TextObject("{=CCR_Reason_history}to mark history", null), new TextObject("{=CCR_Reason_desc_history}With all the wars in Calradia, there are many way one could carve {?PLAYER.GENDER}her{?}his{\\?} name in history.", null), new GetNarrativeMenuOptionArgsDelegate(this.ReasonHistoryOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ReasonOptionsOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ReasonHistoryOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Reason_loss", new TextObject("{=CCR_Reason_loss}after the loss of a loved one", null), new TextObject("{=CCR_Reason_desc_loss}After losing some close to you, you left to see if you could fill that hole", null), new GetNarrativeMenuOptionArgsDelegate(this.ReasonLossOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ReasonOptionsOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ReasonLossOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Reason_crafting", new TextObject("{=CCR_Reason_crafting}to practice your trade", null), new TextObject("{=CCR_Reason_desc_crafting}Your craftmanship has been an important part of your life, but your skill wasn't enough. You thus decided to embark on a journey to improve at it.", null), new GetNarrativeMenuOptionArgsDelegate(this.ReasonCraftingOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ReasonOptionsOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ReasonCraftingOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Reason_helping", new TextObject("{=CCR_Reason_helping}to help those in need", null), new TextObject("{=CCR_Reason_desc_helping}Having been taught in medicine, you decide to set out and help those in need, as with the wars many have suffered.", null), new GetNarrativeMenuOptionArgsDelegate(this.ReasonHelpingOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ReasonOptionsOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ReasonHelpingOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Reason_prove_worth", new TextObject("{=CCR_Reason_prove_worth}to prove your fighting skills", null), new TextObject("{=CCR_Reason_desc_prove_worth}Attaching much importance to your fighting skill, you figured there was no better place than calradia to prove your might.", null), new GetNarrativeMenuOptionArgsDelegate(this.ReasonWorthOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ReasonOptionsOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ReasonWorthOptionOnSelect), null));
            characterCreationManager.AddNewMenu(narrativeMenu);
        }
        public bool ReasonOptionsOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }
        public void ReasonTravelOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(50);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
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
        public void AgeSelectionMenu(CharacterCreationManager characterCreationManager)
        {
            MBTextManager.SetTextVariable("EXP_VALUE", 30);
            BodyProperties bodyProperties = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1);
            bodyProperties = FaceGen.GetBodyPropertiesWithAge(ref bodyProperties, (float)characterCreationManager.CharacterCreationContent.StartingAge);
            List<NarrativeMenuCharacter> list = new List<NarrativeMenuCharacter>();
            list.Add(new NarrativeMenuCharacter("player_age_selection_character", bodyProperties, CharacterObject.PlayerCharacter.Race, CharacterObject.PlayerCharacter.IsFemale));
            NarrativeMenu narrativeMenu = new NarrativeMenu("narrative_age_selection_menu", "narrative_adulthood_menu", "", new TextObject("{=HDFEAYDk}Starting Age", null), new TextObject("{=VlOGrGSn}Your character started off on the adventuring path at the age of...", null), list, new NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate(this.GetAgeSelectionMenuNarrativeMenuCharacterArgs));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("age_selection_young_adult_option", new TextObject("{=!}21", null), new TextObject("{=2k7adlh7}While lacking experience a bit, you are full with youthful energy, you are fully eager, for the long years of adventuring ahead.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAgeSelectionYoungAdultAgeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AgeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AgeSelectionYoungAdultAgeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.AgeSelectionYoungAdultAgeOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("age_selection_adult_option", new TextObject("{=!}30", null), new TextObject("{=NUlVFRtK}You are at your prime, You still have some youthful energy but also have a substantial amount of experience under your belt. ", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAgeSelectionAdultOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AgeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AgeSelectionAdultOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.AgeSelectionAdultOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("age_selection_middle_age_option", new TextObject("{=!}40", null), new TextObject("{=5MxTYApM}This is the right age for starting off, you have years of experience, and you are old enough for people to respect you and gather under your banner.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAgeSelectionMiddleAgeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AgeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AgeSelectionMiddleAgeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.AgeSelectionMiddleAgeOptionOnConsequence)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("age_selection_elder_option", new TextObject("{=!}50", null), new TextObject("{=ePD5Afvy}While you are past your prime, there is still enough time to go on that last big adventure for you. And you have all the experience you need to overcome anything!", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAgeSelectionElderOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AgeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AgeSelectionElderOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.AgeSelectionElderOptionOnConsequence)));
            characterCreationManager.AddNewMenu(narrativeMenu);
        }
        public bool AgeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }
        public void GetAgeSelectionYoungAdultAgeOptionArgs(NarrativeMenuOptionArgs args)
        {
            args.SetUnspentFocusToAdd(2);
            args.SetUnspentAttributeToAdd(1);
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
