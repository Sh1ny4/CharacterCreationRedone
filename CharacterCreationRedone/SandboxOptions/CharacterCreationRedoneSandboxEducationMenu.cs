using SandBox.View.Map.Navigation.NavigationElements;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using static System.Net.Mime.MediaTypeNames;

namespace CharacterCreationRedone.SandboxOptions
{
    public class CharacterCreationRedoneSandboxEducationMenu
    {
        public string GetPlayerChildhoodAgeEquipmentId(CharacterCreationManager characterCreationManager, string parentOccupationType, string cultureId, bool isFemale)
        {
            characterCreationManager.CharacterCreationContent.TryGetEquipmentToUse(parentOccupationType, out string text);
            return string.Concat(new string[] { "player_char_creation_childhood_age_", cultureId, "_", text, "_", isFemale ? "f" : "m" });
        }

        /// <summary>
        /// Education Menu
        /// </summary>
        public List<NarrativeMenuCharacterArgs> AddEducationMenuCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            List<NarrativeMenuCharacterArgs> list = new List<NarrativeMenuCharacterArgs>();
            string playerChildhoodAgeEquipmentId = GetPlayerChildhoodAgeEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            list.Add(new NarrativeMenuCharacterArgs("player_childhood_character", 7, playerChildhoodAgeEquipmentId, "act_childhood_schooled", "spawnpoint_player_1", "", "", null, true, CharacterObject.PlayerCharacter.IsFemale));
            return list;
        }
        public void EducationMenu(CharacterCreationManager characterCreationManager)
        {
            List<NarrativeMenuCharacter> list = new List<NarrativeMenuCharacter>();
            BodyProperties bodyProperties = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1);
            bodyProperties = FaceGen.GetBodyPropertiesWithAge(ref bodyProperties, 7f);
            list.Add(new NarrativeMenuCharacter("player_childhood_character", bodyProperties, CharacterObject.PlayerCharacter.Race, CharacterObject.PlayerCharacter.IsFemale));
            NarrativeMenu narrativeMenu = new NarrativeMenu("narrative_childhood_menu", "narrative_parent_menu", "narrative_education_menu", new TextObject("{=!}Received education", null), new TextObject("{=!}Your parents wanted you to...", null), list, new NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate(AddEducationMenuCharacterArgs));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_Faris", new TextObject("{=CCR_Education_Choice_Faris}become a faris.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(EducationFarisOptionArgs), new NarrativeMenuOptionOnConditionDelegate(EducationFarisOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(EducationFarisOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_Hearthguard", new TextObject("{=CCR_Education_Choice_Hearthguard}enter the hearthguard.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(EducationHearthguardOptionArgs), new NarrativeMenuOptionOnConditionDelegate(EducationHearthguardOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(EducationHearthguardOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_Cataphract", new TextObject("{=CCR_Education_Choice_Cataphract}become a cataphract.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(EducationCataphractOptionArgs), new NarrativeMenuOptionOnConditionDelegate(EducationCataphractOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(EducationCataphractOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_KhanGuard", new TextObject("{=CCR_Education_Choice_KhanGuard}enter a khan's guard.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(EducationKhanGuardOptionArgs), new NarrativeMenuOptionOnConditionDelegate(EducationKhanGuardOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(EducationKhanGuardOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_Druzhina", new TextObject("{=CCR_Education_Choice_Druzhina}join a druzhina.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(EducationDruzhinaOptionArgs), new NarrativeMenuOptionOnConditionDelegate(EducationDruzhinaOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(EducationDruzhinaOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_Knight", new TextObject("{=CCR_Education_Choice_Knight}become a knight.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(EducationKnightOptionArgs), new NarrativeMenuOptionOnConditionDelegate(EducationKnightOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(EducationKnightOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_Commander", new TextObject("{=CCR_Education_Choice_Commander}lead armies.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(EducationCommanderOptionArgs), new NarrativeMenuOptionOnConditionDelegate(EducationCommanderOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(EducationCommanderOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_lady", new TextObject("{=CCR_Education_Choice_lady}become a lady in waiting.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(EducationLadyOptionArgs), new NarrativeMenuOptionOnConditionDelegate(EducationLadyOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(EducationLadyOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_Court", new TextObject("{=CCR_Education_Choice_Court}be part of the court.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(EducationCourtOptionArgs), new NarrativeMenuOptionOnConditionDelegate(EducationCourtOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(EducationCourtOptionOnSelect), null));
            
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_trade", new TextObject("{=CCR_Education_Choice_trade}become a merchant.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(EducationMerchantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(EducationMerchantOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(EducationMerchantOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_crafting", new TextObject("{=CCR_Education_Choice_crafting}learn a trade.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(EducationCraftOptionArgs), new NarrativeMenuOptionOnConditionDelegate(EducationCraftOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(EducationCraftOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_scholar", new TextObject("{=CCR_Education_Choice_scholar}become a scholar.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(EducationScholarOptionArgs), new NarrativeMenuOptionOnConditionDelegate(EducationScholarOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(EducationScholarOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_religious", new TextObject("{=CCR_Education_Choice_religious}be a {?PLAYER.GENDER}lady{?}man{\\?} of faith.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(EducationReligiousOptionArgs), new NarrativeMenuOptionOnConditionDelegate(EducationReligiousOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(EducationReligiousOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_farmer", new TextObject("{=CCR_Education_Choice_farmer}tend to the fields.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(EducationFarmerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(EducationFarmerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(EducationFarmerOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_defense", new TextObject("{=CCR_Education_Choice_defense}learn how to defend yourself.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(EducationDefenseOptionArgs), new NarrativeMenuOptionOnConditionDelegate(EducationDefenseOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(EducationDefenseOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Education_Choice_trickery", new TextObject("{=CCR_Education_Choice_trickery}know how to trick others.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(EducationTrickOptionArgs), new NarrativeMenuOptionOnConditionDelegate(EducationTrickOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(EducationTrickOptionOnSelect), null));
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
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Noble && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai" && !Hero.MainHero.IsFemale;
        }
        public void EducationFarisOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_childhood_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
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
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Noble && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania" && !Hero.MainHero.IsFemale;
        }
        public void EducationHearthguardOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_childhood_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
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
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Noble && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire" && !Hero.MainHero.IsFemale;
        }
        public void EducationCataphractOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_childhood_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
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
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Noble && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait" && !Hero.MainHero.IsFemale;
        }
        public void EducationKhanGuardOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_childhood_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
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
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Noble && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia" && !Hero.MainHero.IsFemale;
        }
        public void EducationDruzhinaOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_childhood_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
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
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Noble && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia" && !Hero.MainHero.IsFemale;
        }
        public void EducationKnightOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_childhood_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
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
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Noble && !Hero.MainHero.IsFemale;
        }
        public void EducationCommanderOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_childhood_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
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
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Noble && Hero.MainHero.IsFemale;
        }
        public void EducationCourtOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_childhood_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
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
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_childhood_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
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
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_childhood_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
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
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_childhood_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
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
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_childhood_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
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
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_childhood_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
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
            return Hero.MainHero.IsFemale;
        }
        public void EducationLadyOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_childhood_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
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
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_childhood_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
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
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_childhood_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
                if (narrativeMenuCharacter.StringId == "player_childhood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_numbers");
                }
            }
        }
        public static class CharacterOccupationTypes
        {

            public const string Noble = "noble";
            public const string Merchant = "merchant";
            public const string Craftman = "craftman";
            public const string Farmer = "farmer";
            public const string Mercenary = "mercenary";
            public const string Infantry = "infantry";
            public const string Shock = "shock";
            public const string Cavalry = "cavalry";
            public const string Skirmisher = "skirmisher";
            public const string HorseArcher = "horsearcher";
            public const string Bandit = "bandit";

            public const string Special = "special";
            public const string Healer = "healer";
            public const string Forester = "forester";
            public const string Bard = "bard";
            public const string Nomad = "nomad";
        }

    }
}
