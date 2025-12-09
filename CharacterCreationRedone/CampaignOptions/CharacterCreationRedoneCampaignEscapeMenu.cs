using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.Extensions;
using StoryMode.GameComponents.CampaignBehaviors;
using StoryMode.StoryModeObjects;

namespace CharacterCreationRedone.CampaignOptions
{
    public class CharacterCreationRedoneCampaignEscapeMenu : StoryModeCharacterCreationCampaignBehavior, ICharacterCreationContentHandler
    {
        public List<NarrativeMenuCharacterArgs> GetEscapeMenuNarrativeMenuCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            List<NarrativeMenuCharacterArgs> list = new List<NarrativeMenuCharacterArgs>();
            string equipmentId = "brother_char_creation_" + characterCreationManager.CharacterCreationContent.SelectedCulture.StringId;
            list.Add(new NarrativeMenuCharacterArgs("brother_character", (int)StoryModeHeroes.ElderBrother.Age, equipmentId, "act_childhood_schooled", "spawnpoint_brother_brother_stage", "", "", null, true, false));
            string selectedTitleType = characterCreationManager.CharacterCreationContent.SelectedTitleType;
            string text = "player_char_creation_" + characterCreationManager.CharacterCreationContent.SelectedCulture.StringId + "_" + selectedTitleType.ToString().ToLower();
            text += (Hero.MainHero.IsFemale ? "_f" : "_m");
            list.Add(new NarrativeMenuCharacterArgs("player_escape_character", (int)CharacterObject.PlayerCharacter.Age, text, "act_childhood_schooled", "spawnpoint_player_brother_stage", "", "", null, true, CharacterObject.PlayerCharacter.IsFemale));
            return list;
        }
        public void AddEscapeMenu(CharacterCreationManager characterCreationManager)
        {
            MBTextManager.SetTextVariable("EXP_VALUE", 20);
            List<NarrativeMenuCharacter> list = new List<NarrativeMenuCharacter>();
            NarrativeMenu narrativeMenuWithId = characterCreationManager.GetNarrativeMenuWithId("narrative_parent_menu");
            BodyProperties bodyProperties = BodyProperties.Default;
            BodyProperties bodyProperties2 = BodyProperties.Default;
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in narrativeMenuWithId.Characters)
            {
                if (narrativeMenuCharacter.StringId == "mother_character")
                {
                    bodyProperties = narrativeMenuCharacter.BodyProperties;
                }
                if (narrativeMenuCharacter.StringId == "father_character")
                {
                    bodyProperties2 = narrativeMenuCharacter.BodyProperties;
                }
            }
            Hero elderBrother = StoryModeHeroes.ElderBrother;
            BodyProperties bodyProperties3 = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1);
            bodyProperties3 = FaceGen.GetBodyPropertiesWithAge(ref bodyProperties3, 23f);
            this.CreateSibling(StoryModeHeroes.LittleBrother, bodyProperties, bodyProperties2);
            this.CreateSibling(StoryModeHeroes.LittleSister, bodyProperties, bodyProperties2);
            BodyProperties randomBodyProperties = BodyProperties.GetRandomBodyProperties(elderBrother.CharacterObject.Race, elderBrother.IsFemale, bodyProperties, bodyProperties2, 1, Hero.MainHero.Mother.CharacterObject.GetDefaultFaceSeed(1), Hero.MainHero.Father.CharacterObject.BodyPropertyRange.HairTags, Hero.MainHero.Father.CharacterObject.BodyPropertyRange.BeardTags, Hero.MainHero.Father.CharacterObject.BodyPropertyRange.TattooTags, 0f);
            randomBodyProperties = new BodyProperties(new DynamicBodyProperties(elderBrother.Age, 0.5f, 0.5f), randomBodyProperties.StaticProperties);
            elderBrother.StaticBodyProperties = randomBodyProperties.StaticProperties;
            elderBrother.Weight = randomBodyProperties.Weight;
            elderBrother.Build = randomBodyProperties.Build;
            list.Add(new NarrativeMenuCharacter("brother_character", randomBodyProperties, elderBrother.CharacterObject.Race, elderBrother.CharacterObject.IsFemale));
            list.Add(new NarrativeMenuCharacter("player_escape_character", bodyProperties3, CharacterObject.PlayerCharacter.Race, CharacterObject.PlayerCharacter.IsFemale));
            NarrativeMenu narrativeMenu = new NarrativeMenu("narrative_escape_menu", "narrative_youth_menu", "", new TextObject("{=peNBA0WW}Story Background", null), new TextObject("{=jg3T5AyE}Like many families in Calradia, your life was upended by war. Your home was ravaged by the passage of army after army. Eventually, you sold your property and set off with your father, mother, brother, and your two younger siblings to a new town you'd heard was safer. But you did not make it. Along the way, the inn at which you were staying was attacked by raiders. Your parents were slain and your two youngest siblings seized, but you and your brother survived because...", null), list, new NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate(this.GetEscapeMenuNarrativeMenuCharacterArgs));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("escape_subdued_raider_option", new TextObject("{=6vCHovVH}you subdued a raider.", null), new TextObject("{=CvBoRaFv}You were able to grab a knife in the confusion of the attack. You stabbed a raider blocking your way.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEscapeSubduedRaiderNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.NarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EscapeSubduedRaiderNarrativeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.FinalizeMainHeroAndElderBrother)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("escape_arrow_option", new TextObject("{=2XhW49TX}you drove them off with arrows.", null), new TextObject("{=ccf67J3J}You grabbed a bow and sent a few arrows the raiders' way. They took cover, giving you the opportunity to flee with your brother.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEscapeArrowNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.NarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EscapeArrowNarrativeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.FinalizeMainHeroAndElderBrother)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("escape_horse_option", new TextObject("{=gOI8lKcl}you rode off on a fast horse.", null), new TextObject("{=cepWNzEA}Jumping on the two remaining horses in the inn's burning stable, you and your brother broke out of the encircling raiders and rode off.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEscapeHorseNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.NarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EscapeHorseNarrativeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.FinalizeMainHeroAndElderBrother)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("escape_tricked_option", new TextObject("{=EdUppdLZ}you tricked the raiders.", null), new TextObject("{=ZqOvtLBM}In the confusion of the attack you shouted that someone had found treasure in the back room. You then made your way out of the undefended entrance with your brother.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEscapeTrickedNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.NarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EscapeTrickedNarrativeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.FinalizeMainHeroAndElderBrother)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("escape_breakout_option", new TextObject("{=qhAhPWdp}you organized the travelers to break out.", null), new TextObject("{=Lmfi0cYk}You encouraged the few travellers in the inn to break out in a coordinated fashion. Raiders killed or captured most but you and your brother were able to escape.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEscapeBreakOutNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.NarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EscapeBreakOutNarrativeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.FinalizeMainHeroAndElderBrother)));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("escape_makeshift_fortification_option", new TextObject("{=7AEw4RbK}You threw up makeshift fortifications.", null), new TextObject("{=Lmfi0cYk}You encouraged the few travellers in the inn to break out in a coordinated fashion. Raiders killed or captured most but you and your brother were able to escape.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetMakeshiftFortificationNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.NarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.MakeshiftFortificationNarrativeOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.FinalizeMainHeroAndElderBrother)));
            characterCreationManager.AddNewMenu(narrativeMenu);
        }
        public bool NarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }
        public void GetEscapeSubduedRaiderNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Athletics
            };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(20);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }
        public void EscapeSubduedRaiderNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            string animationId = "act_childhood_fierce";
            string animationId2 = "act_childhood_athlete";
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId.Equals("player_escape_character"))
                {
                    narrativeMenuCharacter.SetAnimationId(animationId);
                }
                if (narrativeMenuCharacter.StringId.Equals("brother_character"))
                {
                    narrativeMenuCharacter.SetAnimationId(animationId2);
                }
            }
        }
        public void GetEscapeArrowNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Bow, DefaultSkills.Tactics
            };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(20);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }
        public void EscapeArrowNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            string animationId = "act_childhood_athlete";
            string animationId2 = "act_childhood_sharp";
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId.Equals("player_escape_character"))
                {
                    narrativeMenuCharacter.SetAnimationId(animationId);
                }
                if (narrativeMenuCharacter.StringId.Equals("brother_character"))
                {
                    narrativeMenuCharacter.SetAnimationId(animationId2);
                }
            }
        }
        public void GetEscapeHorseNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.Scouting
            };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(20);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public void EscapeHorseNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            string animationId = "act_childhood_tough";
            string animationId2 = "act_childhood_decisive";
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId.Equals("player_escape_character"))
                {
                    narrativeMenuCharacter.SetAnimationId(animationId);
                }
                if (narrativeMenuCharacter.StringId.Equals("brother_character"))
                {
                    narrativeMenuCharacter.SetAnimationId(animationId2);
                }
            }
        }
        public void GetEscapeTrickedNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Tactics
            };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(20);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public void EscapeTrickedNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            string animationId = "act_childhood_ready_handshield";
            string animationId2 = "act_aserai_aserai_mp_archer_idle";
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId.Equals("player_escape_character"))
                {
                    narrativeMenuCharacter.SetAnimationId(animationId);
                }
                if (narrativeMenuCharacter.StringId.Equals("brother_character"))
                {
                    narrativeMenuCharacter.SetAnimationId(animationId2);
                }
            }
        }
        public void GetEscapeBreakOutNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Leadership, DefaultSkills.Charm
            };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(20);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }
        public void EscapeBreakOutNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            string animationId = "act_childhood_manners";
            string animationId2 = "act_childhood_tough";
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId.Equals("player_escape_character"))
                {
                    narrativeMenuCharacter.SetAnimationId(animationId);
                }
                if (narrativeMenuCharacter.StringId.Equals("brother_character"))
                {
                    narrativeMenuCharacter.SetAnimationId(animationId2);
                }
            }
        }
        public void GetMakeshiftFortificationNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Engineering, DefaultSkills.TwoHanded
            };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(20);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public void MakeshiftFortificationNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            string animationId = "act_childhood_ready_handshield";
            string animationId2 = "act_khuzait_mp_rabble_idle";
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId.Equals("player_escape_character"))
                {
                    narrativeMenuCharacter.SetAnimationId(animationId);
                }
                if (narrativeMenuCharacter.StringId.Equals("brother_character"))
                {
                    narrativeMenuCharacter.SetAnimationId(animationId2);
                }
            }
        }
        public void FinalizeMainHeroAndElderBrother(CharacterCreationManager characterCreationManager)
        {
            NarrativeMenu narrativeMenuWithId = characterCreationManager.GetNarrativeMenuWithId("narrative_escape_menu");
            NarrativeMenuCharacter narrativeMenuCharacter = null;
            NarrativeMenuCharacter narrativeMenuCharacter2 = null;
            foreach (NarrativeMenuCharacter narrativeMenuCharacter3 in narrativeMenuWithId.Characters)
            {
                if (narrativeMenuCharacter3.StringId.Equals("player_escape_character"))
                {
                    narrativeMenuCharacter = narrativeMenuCharacter3;
                }
                if (narrativeMenuCharacter3.StringId.Equals("brother_character"))
                {
                    narrativeMenuCharacter2 = narrativeMenuCharacter3;
                }
            }
            CharacterObject.PlayerCharacter.Equipment.FillFrom(narrativeMenuCharacter.Equipment.DefaultEquipment, true);
            CharacterObject.PlayerCharacter.FirstCivilianEquipment.FillFrom(narrativeMenuCharacter.Equipment.GetRandomCivilianEquipment(), true);
            Hero elderBrother = StoryModeHeroes.ElderBrother;
            elderBrother.CharacterObject.Equipment.FillFrom(narrativeMenuCharacter2.Equipment.DefaultEquipment, true);
            elderBrother.CharacterObject.FirstCivilianEquipment.FillFrom(narrativeMenuCharacter2.Equipment.GetRandomCivilianEquipment(), true);
        }
        new public void CreateSibling(Hero hero, BodyProperties motherBodyProperties, BodyProperties fatherBodyProperties)
        {
            BodyProperties randomBodyProperties = BodyProperties.GetRandomBodyProperties(hero.CharacterObject.Race, hero.IsFemale, motherBodyProperties, fatherBodyProperties, 1, Hero.MainHero.Mother.CharacterObject.GetDefaultFaceSeed(1), hero.IsFemale ? Hero.MainHero.Mother.CharacterObject.BodyPropertyRange.HairTags : Hero.MainHero.Father.CharacterObject.BodyPropertyRange.HairTags, hero.IsFemale ? Hero.MainHero.Mother.CharacterObject.BodyPropertyRange.BeardTags : Hero.MainHero.Father.CharacterObject.BodyPropertyRange.BeardTags, hero.IsFemale ? Hero.MainHero.Mother.CharacterObject.BodyPropertyRange.TattooTags : Hero.MainHero.Father.CharacterObject.BodyPropertyRange.TattooTags, 0f);
            randomBodyProperties = new BodyProperties(new DynamicBodyProperties(hero.Age, 0.5f, 0.5f), randomBodyProperties.StaticProperties);
            hero.StaticBodyProperties = randomBodyProperties.StaticProperties;
            hero.Weight = randomBodyProperties.Weight;
            hero.Build = randomBodyProperties.Build;
        }
    }
}
