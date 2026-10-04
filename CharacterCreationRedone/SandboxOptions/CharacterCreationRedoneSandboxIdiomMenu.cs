using NavalDLC.CharacterDevelopment;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace CharacterCreationRedone.SandboxOptions
{
    public class CharacterCreationRedoneSandboxIdiomMenu
    {
        public string GetPlayerEducationAgeEquipmentId(CharacterCreationManager characterCreationManager, string parentOccupationType, string cultureId, bool isFemale)
        {
            characterCreationManager.CharacterCreationContent.TryGetEquipmentToUse(parentOccupationType, out string text);
            return string.Concat(new string[] { "player_char_creation_education_age_", cultureId, "_", text, "_", isFemale ? "f" : "m" });
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
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_Noblefighter", new TextObject("{=CCR_Idiom_choice_fighter}Better to be a warrior in a garden than a gardener in a war.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomFighterNobleOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomFighterNobleOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomFighterNobleOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_Commonerfighter", new TextObject("{=CCR_Idiom_choice_fighter}Better to be a warrior in a garden than a gardener in a war.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomFighterCommonerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomFighterCommonerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomFighterCommonerOptionOnSelect), null));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_healthy", new TextObject("{=CCR_Idiom_choice_healthy}A healthy mind in a healthy body.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomHealthyOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomHealthyOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomHealthyOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_prevention", new TextObject("{=CCR_Idiom_choice_prevention}Prevention is better than cure.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomPreventionOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomPreventionOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomPreventionOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_wellbegun", new TextObject("{=CCR_Idiom_choice_wellbegun}Well begun is half done.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomWellBegunOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomWellBegunOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomWellBegunOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_bold", new TextObject("{=CCR_Idiom_choice_bold}Fortune favors the bold.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomBoldOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomBoldOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomBoldOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_forwarned", new TextObject("{=CCR_Idiom_choice_forwarned}Forewarned is forearmed.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomForwarnedOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomForwarnedOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomForwarnedOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_invention", new TextObject("{=CCR_Idiom_choice_invention}Necessity is the mother of invention.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomInventionOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomInventionOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomInventionOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_armed", new TextObject("{=CCR_Idiom_choice_armed}Men with weapons never starve.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomArmedOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomArmedOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomArmedOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_conquer", new TextObject("{=CCR_Idiom_choice_conquer}To conquer without risk is to triumph without glory.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomConquerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomConquerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomConquerOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_means", new TextObject("{=CCR_Idiom_choice_means}The end justifies the means.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomMeanOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomMeanOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomMeanOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Idiom_choice_means", new TextObject("{=CCR_Idiom_choice_turbulent}A smooth sea never made a skillful sailor.", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.IdiomTurbulantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.IdiomTurbulantOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.IdiomTurbulantOptionOnSelect), null));
            characterCreationManager.AddNewMenu(narrativeMenu);
        }

        public void IdiomFighterNobleOptionArgs(NarrativeMenuOptionArgs args)
        {
            if (Hero.MainHero.Culture.StringId == "aserai")
            {
                args.SetAffectedSkills(new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.Riding });
                args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
                args.SetAffectedTraits(new TraitObject[] { DefaultTraits.Valor });
            }
            else if (Hero.MainHero.Culture.StringId == "battania")
            {
                args.SetAffectedSkills(new SkillObject[] { DefaultSkills.Bow, DefaultSkills.Athletics });
                args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
                args.SetAffectedTraits(new TraitObject[] { DefaultTraits.Valor });
            }
            else if (Hero.MainHero.Culture.StringId == "empire")
            {
                args.SetAffectedSkills(new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.Riding });
                args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
                args.SetAffectedTraits(new TraitObject[] { DefaultTraits.Valor });
            }
            else if (Hero.MainHero.Culture.StringId == "khuzait")
            {
                args.SetAffectedSkills(new SkillObject[] { DefaultSkills.Bow, DefaultSkills.Riding });
                args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
                args.SetAffectedTraits(new TraitObject[] { DefaultTraits.Valor });
            }
            else if (Hero.MainHero.Culture.StringId == "nord")
            {
                args.SetAffectedSkills(new SkillObject[] { DefaultSkills.TwoHanded, DefaultSkills.Athletics });
                args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
                args.SetAffectedTraits(new TraitObject[] { DefaultTraits.Valor });
            }
            else if (Hero.MainHero.Culture.StringId == "sturgia")
            {
                args.SetAffectedSkills(new SkillObject[] { DefaultSkills.TwoHanded, DefaultSkills.Riding });
                args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
                args.SetAffectedTraits(new TraitObject[] { DefaultTraits.Valor });
            }
            else if (Hero.MainHero.Culture.StringId == "vlandia")
            {
                args.SetAffectedSkills(new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.Riding });
                args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
                args.SetAffectedTraits(new TraitObject[] { DefaultTraits.Valor });
            }
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToTraits(1);
        }
        public bool IdiomFighterNobleOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Noble;
        }
        public void IdiomFighterNobleOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_education_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("");
                    break;
                }
            }
        }
        public void IdiomFighterCommonerOptionArgs(NarrativeMenuOptionArgs args)
        {
            args.SetAffectedSkills(new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Athletics });
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
            args.SetAffectedTraits(new TraitObject[] { DefaultTraits.Valor });
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToTraits(1);
        }
        public bool IdiomFighterCommonerOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return !(characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Noble);
        }
        public void IdiomFighterCommonerOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_education_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("");
                    break;
                }
            }
        }
        public void IdiomHealthyOptionArgs(NarrativeMenuOptionArgs args)
        {
            args.SetAffectedSkills(new SkillObject[] { DefaultSkills.Athletics, DefaultSkills.Medicine });
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
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_education_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("");
                    break;
                }
            }
        }
        public void IdiomPreventionOptionArgs(NarrativeMenuOptionArgs args)
        {
            args.SetAffectedSkills(new SkillObject[] { DefaultSkills.Medicine, DefaultSkills.Steward });
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
            args.SetAffectedTraits(new TraitObject[] { DefaultTraits.Calculating });
            args.SetLevelToTraits(1);
        }
        public bool IdiomPreventionOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }
        public void IdiomPreventionOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_education_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("");
                    break;
                }
            }
        }
        public void IdiomWellBegunOptionArgs(NarrativeMenuOptionArgs args)
        {
            args.SetAffectedSkills(new SkillObject[] { DefaultSkills.Steward, DefaultSkills.Engineering });
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
            args.SetAffectedTraits(new TraitObject[] { DefaultTraits.Calculating });
            args.SetLevelToTraits(1);
        }
        public bool IdiomWellBegunOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }
        public void IdiomWellBegunOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_education_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("");
                    break;
                }
            }
        }
        public void IdiomBoldOptionArgs(NarrativeMenuOptionArgs args)
        {
            args.SetAffectedSkills(new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Tactics });
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
            args.SetAffectedTraits(new TraitObject[] { DefaultTraits.Calculating });
            args.SetLevelToTraits(-1);
        }
        public bool IdiomBoldOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }
        public void IdiomBoldOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_education_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("");
                    break;
                }
            }
        }
        public void IdiomForwarnedOptionArgs(NarrativeMenuOptionArgs args)
        {
            args.SetAffectedSkills(new SkillObject[] { DefaultSkills.Tactics, DefaultSkills.Scouting });
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
            args.SetAffectedTraits(new TraitObject[] { DefaultTraits.Calculating });
            args.SetLevelToTraits(1);
        }
        public bool IdiomForwarnedOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }
        public void IdiomForwarnedOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_education_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("");
                    break;
                }
            }
        }
        public void IdiomInventionOptionArgs(NarrativeMenuOptionArgs args)
        {
            args.SetAffectedSkills(new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Engineering });
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
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_education_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("");
                    break;
                }
            }
        }
        public void IdiomArmedOptionArgs(NarrativeMenuOptionArgs args)
        {
            args.SetAffectedSkills(new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Roguery });
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
            args.SetAffectedTraits(new TraitObject[] { DefaultTraits.Honor });
            args.SetLevelToTraits(-1);
        }
        public bool IdiomArmedOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }
        public void IdiomArmedOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_education_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("");
                    break;
                }
            }
        }
        public void IdiomConquerOptionArgs(NarrativeMenuOptionArgs args)
        {
            args.SetAffectedSkills(new SkillObject[] { DefaultSkills.Leadership, DefaultSkills.Charm });
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
            args.SetAffectedTraits(new TraitObject[] { DefaultTraits.Valor });
            args.SetLevelToTraits(1);
        }
        public bool IdiomConquerOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }
        public void IdiomConquerOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_education_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("");
                    break;
                }
            }
        }
        public void IdiomMeanOptionArgs(NarrativeMenuOptionArgs args)
        {
            args.SetAffectedSkills(new SkillObject[] { DefaultSkills.Tactics, DefaultSkills.Roguery });
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
            args.SetAffectedTraits(new TraitObject[] { DefaultTraits.Honor });
            args.SetLevelToTraits(-1);
        }
        public bool IdiomMeanOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }
        public void IdiomMeanOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_education_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("");
                    break;
                }
            }
        }
        public void IdiomTurbulantOptionArgs(NarrativeMenuOptionArgs args)
        {
            args.SetAffectedSkills(new SkillObject[] { NavalSkills.Mariner, NavalSkills.Shipmaster });
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
            args.SetAffectedTraits(new TraitObject[] { DefaultTraits.Valor });
            args.SetLevelToTraits(1);
        }
        public bool IdiomTurbulantOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }
        public void IdiomTurbulantOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                narrativeMenuCharacter.SetEquipment(Campaign.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new string[] { "player_char_creation_education_age_", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, "_", characterCreationManager.CharacterCreationContent.SelectedParentOccupation, "_", Hero.MainHero.IsFemale ? "f" : "m" })));
                if (narrativeMenuCharacter.StringId == "player_education_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_sharp");
                    narrativeMenuCharacter.SetLeftHandItem("");
                    narrativeMenuCharacter.SetRightHandItem("");
                    break;
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
