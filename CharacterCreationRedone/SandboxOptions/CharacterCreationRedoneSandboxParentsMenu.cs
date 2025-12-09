using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace CharacterCreationRedone.SandboxOptions
{
    public class CharacterCreationRedoneSandboxParentsMenu
    {
        public string GetMotherEquipmentId(CharacterCreationManager characterCreationManager, string occupationType, string cultureId)
        {
            characterCreationManager.CharacterCreationContent.TryGetEquipmentToUse(occupationType, out string str);
            return "mother_char_creation_" + occupationType + "_" + cultureId;
        }
        public string GetFatherEquipmentId(CharacterCreationManager characterCreationManager, string occupationType, string cultureId)
        {
            characterCreationManager.CharacterCreationContent.TryGetEquipmentToUse(occupationType, out string str);
            return "father_char_creation_" + occupationType + "_" + cultureId;
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
        public void UpdateParentEquipment(CharacterCreationManager characterCreationManager, MBEquipmentRoster motherEquipment, MBEquipmentRoster fatherEquipment, string motherAnimation, string fatherAnimation)
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
            args.SetRenownToAdd(50);
        }
        public void FamilyChoiceARaisOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("noble");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }
        public void FamilyChoiceAMamluksOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("infantry");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public void FamilyChoiceAMerchantOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("merchant");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Polearm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public void FamilyChoiceAFarmerOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceAArtisansOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Trade };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public void FamilyChoiceAArtisansOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("craftman");
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
            characterCreationManager.CharacterCreationContent.SetParentOccupation("bandit");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Leadership, DefaultSkills.Steward };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
            args.SetRenownToAdd(50);
        }
        public void FamilyChoiceBchieftainsOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("noble");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Medicine, DefaultSkills.Steward };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public void FamilyChoiceBHealersOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceBFarmersOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.Crafting };
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Trade };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public void FamilyChoiceBArtisansOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("craftman");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting, DefaultSkills.Bow };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }
        public void FamilyChoiceBForestersOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("forester");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Charm, DefaultSkills.Roguery };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }
        public void FamilyChoiceBBardsOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("bard");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Charm, DefaultSkills.Leadership };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
            args.SetRenownToAdd(50);
        }
        public void FamilyChoiceEAristocratesOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("noble");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Steward };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public void FamilyChoiceEMerchantOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("merchant");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Trade };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public void FamilyChoiceEArtisansOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("craftman");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }
        public void FamilyChoiceESoldiersOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("infantry");
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
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public void FamilyChoiceEVagabonsOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("bandit");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.Leadership };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
            args.SetRenownToAdd(50);
        }
        public void FamilyChoiceKNoyansOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("noble");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.Bow };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }
        public void FamilyChoiceKNomadsOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("nomad");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Steward };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public void FamilyChoiceKMerchantOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("merchant");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Crafting };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public void FamilyChoiceKArtisansOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("craftman");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Bow };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public void FamilyChoiceKWarriorsOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("horsearcher");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Throwing };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public void FamilyChoiceKThugsOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("bandit");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.TwoHanded, DefaultSkills.Leadership };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
            args.SetRenownToAdd(50);
        }
        public void FamilyChoiceSBoyarsOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("noble");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public void FamilyChoiceSMerchantsOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("merchant");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.Crafting };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Trade };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public void FamilyChoiceSArtisansOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("craftman");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }
        public void FamilyChoiceSWarriorsOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("infantry");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Scouting };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public void FamilyChoiceSRaidersOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("bandit");
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
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
            args.SetRenownToAdd(50);
        }
        public void FamilyChoiceVBaronsOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("noble");
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
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public void FamilyChoiceVMerchantsOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("merchant");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Polearm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public void FamilyChoiceVYeomensOptionOnSelect(CharacterCreationManager characterCreationManager)
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
        public void FamilyChoiceVArtisansOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Trade };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public void FamilyChoiceVArtisansOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("craftman");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public void FamilyChoiceVSoldiersOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("infantry");
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.OneHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public void FamilyChoiceVMercenariesOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("bandit");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_hugging";
            string fatherAnimation = "act_character_creation_male_default_hugging";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

    }
}
