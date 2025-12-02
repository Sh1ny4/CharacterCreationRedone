using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace CharacterCreationRedone.SandboxOptions
{
    public class CharacterCreationRedoneSandboxYouthMenu
    {
        public string GetPlayerEquipmentId(CharacterCreationManager characterCreationManager, string occupationType, string cultureId, bool isFemale)
        {
            characterCreationManager.CharacterCreationContent.TryGetEquipmentToUse(occupationType, out string text);
            return string.Concat(new string[] { "player_char_creation_", cultureId, "_", occupationType, "_", isFemale ? "f" : "m" });
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
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("CCR_Start_Choice_Bwildling", new TextObject("{=CCR_Start_Choice_Bwildling}wildling", null), new TextObject("{=!}", null), new GetNarrativeMenuOptionArgsDelegate(this.LifeStartBattaniaWildlingOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.LifeStartBattaniaWildlingOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.LifeStartBattaniaWildlingOptionOnSelect), new NarrativeMenuOptionOnConsequenceDelegate(this.LifeStartBattaniaWildlingOptionOnConsequence)));
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Throwing, DefaultSkills.OneHanded, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }
        public bool LifeStartAseraiFarisOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Noble && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }
        public void LifeStartAseraiFarisOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "noble";
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
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "special";
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
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "merchant";
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
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "craftman";
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
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "farmer";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Throwing, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }
        public bool LifeStartAseraiMamlukeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }
        public void LifeStartAseraiMamlukeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "infantry";
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
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }
        public bool LifeStartAseraiHorseArcherOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }
        public void LifeStartAseraiHorseArcherOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "horsearcher";
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
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }
        public bool LifeStartAseraiArcherOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }
        public void LifeStartAseraiArcherOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "skirmisher";
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
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public bool LifeStartAseraiDesertBanditOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }
        public void LifeStartAseraiDesertBanditOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "bandit";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.TwoHanded, DefaultSkills.Bow, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }
        public bool LifeStartBattaniaFiannaOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Noble && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }
        public void LifeStartBattaniaFiannaOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "noble";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Medicine, DefaultSkills.Steward, DefaultSkills.Scouting };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartBattaniaDruidOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }
        public void LifeStartBattaniaDruidOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "special";
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
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "merchant";
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
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "craftman";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Bow, DefaultSkills.Scouting, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }
        public bool LifeStartBattaniaForesterOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }
        public void LifeStartBattaniaForesterOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "farmer";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.TwoHanded, DefaultSkills.Throwing, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }
        public bool LifeStartBattaniaWildlingOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }
        public void LifeStartBattaniaWildlingOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "shock";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Athletics, DefaultSkills.Throwing, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }
        public bool LifeStartBattaniaScoutOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }
        public void LifeStartBattaniaScoutOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "horsearcher";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Throwing, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }
        public bool LifeStartBattaniaKernOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }
        public void LifeStartBattaniaKernOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "skirmisher";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Bow, DefaultSkills.OneHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public bool LifeStartBattaniaForestBanditOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }
        public void LifeStartBattaniaForestBanditOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "bandit";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Tactics, DefaultSkills.Leadership, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public bool LifeStartEmpireCommanderOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Noble && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }
        public void LifeStartEmpireCommanderOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "noble";
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
            CharacterObject wanderer = (from character in CharacterObject.All where character.Occupation == Occupation.Wanderer && character.Culture == Hero.MainHero.Culture select character).GetRandomElementInefficiently();
            Hero companion = HeroCreator.CreateSpecialHero(wanderer);
            AddCompanionAction.Apply(Clan.PlayerClan, companion);
            AddHeroToPartyAction.Apply(companion, Hero.MainHero.PartyBelongedTo);
        }
        public void LifeStartEmpireEngineerOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Engineering, DefaultSkills.Crafting, DefaultSkills.Steward };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }
        public bool LifeStartEmpireEngineerOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }
        public void LifeStartEmpireEngineerOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "special";
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
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "merchant";
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
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "craftman";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Steward, DefaultSkills.Polearm };
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
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "farmer";
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
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }
        public bool LifeStartEmpireLegionaryOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }
        public void LifeStartEmpireLegionaryOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "infantry";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }
        public bool LifeStartEmpireArcherOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }
        public void LifeStartEmpireArcherOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "skirmisher";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Polearm, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }
        public bool LifeStartEmpireCavalryOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }
        public void LifeStartEmpireCavalryOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "cavalry";
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
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }
        public bool LifeStartEmpireHorseArcherOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }
        public void LifeStartEmpireHorseArcherOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "horsearcher";
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
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public bool LifeStartEmpireLooterOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }
        public void LifeStartEmpireLooterOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "bandit";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.Bow, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }
        public bool LifeStartKhuzaitKhanGuardOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Noble && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }
        public void LifeStartKhuzaitKhanGuardOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "noble";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting, DefaultSkills.Bow, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }
        public bool LifeStartKhuzaitNomadOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }
        public void LifeStartKhuzaitNomadOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "special";
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
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "merchant";
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
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "craftman";
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
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "farmer";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Polearm, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }
        public bool LifeStartKhuzaitCavalryOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }
        public void LifeStartKhuzaitCavalryOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "cavalry";
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
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }
        public bool LifeStartKhuzaitHorseArcherOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }
        public void LifeStartKhuzaitHorseArcherOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "horsearcher";
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
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }
        public bool LifeStartKhuzaitInfantryOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }
        public void LifeStartKhuzaitInfantryOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "infantry";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Bow, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public bool LifeStartKhuzaitSteppeBanditOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }
        public void LifeStartKhuzaitSteppeBanditOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "bandit";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.TwoHanded, DefaultSkills.Throwing, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }
        public bool LifeStartSturgiaDruzhinaOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Noble && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }
        public void LifeStartSturgiaDruzhinaOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "noble";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting, DefaultSkills.Bow, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }
        public bool LifeStartSturgiaFurHunterOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }
        public void LifeStartSturgiaFurHunterOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "special";
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
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "merchant";
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
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "craftman";
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
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "farmer";
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
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }
        public bool LifeStartSturgiaInfantryOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }
        public void LifeStartSturgiaInfantryOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "infantry";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.TwoHanded, DefaultSkills.Throwing, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }
        public bool LifeStartSturgiaShockTroopOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }
        public void LifeStartSturgiaShockTroopOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "shock";
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
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }
        public bool LifeStartSturgiaArcherOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }
        public void LifeStartSturgiaArcherOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "skirmisher";
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
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public bool LifeStartSturgiaSeaRaiderBanditOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }
        public void LifeStartSturgiaSeaRaiderBanditOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "bandit";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.Charm, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }
        public bool LifeStartVlandiaKnightOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedParentOccupation == CharacterOccupationTypes.Noble && characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }
        public void LifeStartVlandiaKnightOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "noble";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Steward, DefaultSkills.Trade, DefaultSkills.Leadership };
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
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "special";
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
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "merchant";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Trade, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }
        public bool LifeStartVlandiaGuildMemberOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }
        public void LifeStartVlandiaGuildMemberOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "craftman";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Polearm, DefaultSkills.Medicine };
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
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "farmer";
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
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }
        public bool LifeStartVlandiaInfantryOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }
        public void LifeStartVlandiaInfantryOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "infantry";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.OneHanded, DefaultSkills.Polearm, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }
        public bool LifeStartVlandiaCavalryOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }
        public void LifeStartVlandiaCavalryOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "cavalry";
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crossbow, DefaultSkills.OneHanded, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }
        public bool LifeStartVlandiaRangedOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }
        public void LifeStartVlandiaRangedOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "skirmisher";
            string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, "skirmisher", characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
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
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Throwing, DefaultSkills.Riding };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }
        public bool LifeStartVlandiaMountainBanditOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }
        public void LifeStartVlandiaMountainBanditOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "bandit";
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
