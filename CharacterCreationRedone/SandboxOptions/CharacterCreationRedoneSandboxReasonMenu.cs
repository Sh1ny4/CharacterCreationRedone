using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace CharacterCreationRedone.SandboxOptions
{
    public class CharacterCreationRedoneSandboxReasonMenu
    {
        public string GetPlayerEquipmentId(CharacterCreationManager characterCreationManager, string occupationType, string cultureId, bool isFemale)
        {
            characterCreationManager.CharacterCreationContent.TryGetEquipmentToUse(occupationType, out string text);
            return string.Concat(new string[] { "player_char_creation_", cultureId, "_", occupationType, "_", isFemale ? "f" : "m" });
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
                    narrativeMenuCharacter.SetAnimationId("act_childhood_vibrant");
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
                    narrativeMenuCharacter.SetAnimationId("act_childhood_decisive");
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
                    narrativeMenuCharacter.SetAnimationId("act_childhood_clever");
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
                    narrativeMenuCharacter.SetAnimationId("act_childhood_decisive");
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
                    narrativeMenuCharacter.SetAnimationId("act_childhood_vibrant");
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
                    narrativeMenuCharacter.SetAnimationId("act_childhood_vibrant");
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
                    narrativeMenuCharacter.SetAnimationId("act_childhood_tough");
                }
            }
        }
    }
}
