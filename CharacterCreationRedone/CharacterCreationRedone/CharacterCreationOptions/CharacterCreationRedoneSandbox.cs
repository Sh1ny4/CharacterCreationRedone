using HarmonyLib;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
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
            __instance.AddChildhoodMenu(characterCreationManager);
            __instance.AddEducationMenu(characterCreationManager);
            __instance.AddYouthMenu(characterCreationManager);
            __instance.AddReasonMenu(characterCreationManager);
            __instance.AddAgeSelectionMenu(characterCreationManager);
            return false;
        }

        public string GetMotherEquipmentId(CharacterCreationManager characterCreationManager, string occupationType, string cultureId)
        {
            string str;
            characterCreationManager.CharacterCreationContent.TryGetEquipmentToUse(occupationType, out str);
            return "mother_char_creation_" + str + "_" + cultureId;
        }

        public string GetFatherEquipmentId(CharacterCreationManager characterCreationManager, string occupationType, string cultureId)
        {
            string str;
            characterCreationManager.CharacterCreationContent.TryGetEquipmentToUse(occupationType, out str);
            return "father_char_creation_" + str + "_" + cultureId;
        }

        public string GetPlayerChildhoodAgeEquipmentId(CharacterCreationManager characterCreationManager, string parentOccupationType, string cultureId, bool isFemale)
        {
            string text;
            characterCreationManager.CharacterCreationContent.TryGetEquipmentToUse(parentOccupationType, out text);
            return string.Concat(new string[] { "player_char_creation_childhood_age_", cultureId, "_", text, "_", isFemale ? "f" : "m" });
        }

        public string GetPlayerEducationAgeEquipmentId(CharacterCreationManager characterCreationManager, string parentOccupationType, string cultureId, bool isFemale)
        {
            string text;
            characterCreationManager.CharacterCreationContent.TryGetEquipmentToUse(parentOccupationType, out text);
            return string.Concat(new string[] { "player_char_creation_education_age_", cultureId, "_", text, "_", isFemale ? "f" : "m" });
        }

        public string GetPlayerEquipmentId(CharacterCreationManager characterCreationManager, string occupationType, string cultureId, bool isFemale)
        {
            string text;
            characterCreationManager.CharacterCreationContent.TryGetEquipmentToUse(occupationType, out text);
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
            NarrativeMenu narrativeMenu = new NarrativeMenu("narrative_parent_menu", "start", "narrative_childhood_menu", new TextObject("{=b4lDDcli}Family", null), new TextObject("{=XgFU1pCx}You were born into a family of...", null), list, new NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate(this.GetParentMenuNarrativeMenuCharacterArgs));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("aserai_kinsfolk_option", new TextObject("{=Sw8OxnNr}Kinsfolk of an emir", null), new TextObject("{=MFrIHJZM}Your family was from a smaller offshoot of an emir's tribe. Your father's land gave him enough income to afford a horse but he was not quite wealthy enough to buy the armor needed to join the heavier cavalry. He fought as one of the light horsemen for which the desert is famous.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAseraiKinsfolkNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiKinsfolkNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AseraiKinsfolkNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("aserai_slave_option", new TextObject("{=ngFVgwDD}Warrior-slaves", null), new TextObject("{=GsPC2MgU}Your father was part of one of the slave-bodyguards maintained by the Aserai emirs. He fought by his master's side with tribe's armored cavalry, and was freed - perhaps for an act of valor, or perhaps he paid for his freedom with his share of the spoils of battle. He then married your mother.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAseraiSlaveNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiSlaveNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AseraiSlaveNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("aserai_physician_option", new TextObject("{=bgy8LVvY}Physician", null), new TextObject("{=BhQlmQoj}Your family were respected physicians in an oasis town. They set bones and cured the sick, and their skills were in much demand. They were respected in the higher echelons of society too.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAseraiPhysicianNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiPhysicianNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AseraiPhysicianNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("aserai_farmer_option", new TextObject("{=g31pXuqi}Oasis farmers", null), new TextObject("{=5P0KqBAw}Your family tilled the soil in one of the oases of the Nahasa and tended the palm orchards that produced the desert's famous dates. Your father was a member of the main foot levy of his tribe, fighting with his kinsmen under the emir's banner.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAseraiFarmerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiFarmerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AseraiFarmerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("aserai_herder_option", new TextObject("{=EEedqolz}Bedouin", null), new TextObject("{=PKhcPbBX}Your family were part of a nomadic clan, crisscrossing the wastes between wadi beds and wells to feed their herds of goats and camels on the scraggly scrubs of the Nahasa.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAseraiHerderNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiHerderNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AseraiHerderNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("aserai_artisan_option", new TextObject("{=tRIrbTvv}Urban back-alley thugs", null), new TextObject("{=6bUSbsKC}Your father worked for a fitiwi, one of the strongmen who keep order in the poorer quarters of the oasis towns. He resolved disputes over land, dice and insults, imposing his authority with the fitiwi's traditional staff.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAseraiArtisanNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiArtisanNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.AseraiArtisanNarrativeOptionOnSelect), null));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("battania_retainer_option", new TextObject("{=GeNKQlHR}Members of the chieftain's hearthguard", null), new TextObject("{=LpH8SYFL}Your family were the trusted kinfolk of a Battanian chieftain, and sat at his table in his great hall. Your father assisted his chief in running the affairs of the clan and trained with the traditional weapons of the Battanian elite, the two-handed sword or falx and the bow.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaRetainerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaRetainerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.BattaniaRetainerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("battania_healer_option", new TextObject("{=AeBzTj6w}Healers", null), new TextObject("{=j6py5Rv5}Your parents were healers who gathered herbs and treated the sick. As a living reservoir of Battanian tradition, they were also asked to adjudicate many disputes between the clans.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaHealerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaHealerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.BattaniaHealerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("battania_farmer_option", new TextObject("{=tGEStbxb}Tribespeople", null), new TextObject("{=WchH8bS2}Your family were middle-ranking members of a Battanian clan, who tilled their own land. Your father fought with the kern, the main body of his people's warriors, joining in the screaming charges for which the Battanians were famous.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaFarmerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaFarmerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.BattaniaFarmerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("battania_artisan_option", new TextObject("{=BCU6RezA}Smiths", null), new TextObject("{=kg9YtrOg}Your family were smiths, a revered profession among the Battanians. They crafted everything from fine filigree jewelry in geometric designs to the well-balanced longswords favored by the Battanian aristocracy.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaArtisanNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaArtisanNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.BattaniaArtisanNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("battania_hunter_option", new TextObject("{=7eWmU2mF}Foresters", null), new TextObject("{=7jBroUUQ}Your family had little land of their own, so they earned their living from the woods, hunting and trapping. They taught you from an early age that skills like finding game trails and killing an animal with one shot could make the difference between eating and starvation.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaHunterNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaHunterNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.BattaniaHunterNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("battania_bard_option", new TextObject("{=SpJqhEEh}Bards", null), new TextObject("{=aVzcyhhy}Your father was a bard, drifting from chieftain's hall to chieftain's hall making his living singing the praises of one Battanian aristocrat and mocking his enemies, then going to his enemy's hall and doing the reverse. You learned from him that a clever tongue could spare you  from a life toiling in the fields, if you kept your wits about you.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaBardNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaBardNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.BattaniaBardNarrativeOptionOnSelect), null));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("empire_lanlord_option", new TextObject("{=InN5ZZt3}A landlord's retainers", null), new TextObject("{=ivKl4mV2}Your father was a trusted lieutenant of the local landowning aristocrat. He rode with the lord's cavalry, fighting as an armored lancer.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireLandlordNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireLandlordNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EmpireLandlordNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("empire_merchant_option", new TextObject("{=651FhzdR}Urban merchants", null), new TextObject("{=FQntPChs}Your family were merchants in one of the main cities of the Empire. They sometimes organized caravans to nearby towns, and discussed issues in the town council.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireUrbanNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireUrbanNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EmpireUrbanNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("empire_farmer_option", new TextObject("{=sb4gg8Ak}Freeholders", null), new TextObject("{=09z8Q08f}Your family were small farmers with just enough land to feed themselves and make a small profit. People like them were the pillars of the imperial rural economy, as well as the backbone of the levy.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireFarmerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireFarmerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EmpireFarmerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("empire_artisan_option", new TextObject("{=v48N6h1t}Urban artisans", null), new TextObject("{=ueCm5y1C}Your family owned their own workshop in a city, making goods from raw materials brought in from the countryside. Your father played an active if minor role in the town council, and also served in the militia.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireArtisanNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireArtisanNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EmpireArtisanNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("empire_hunter_option", new TextObject("{=7eWmU2mF}Foresters", null), new TextObject("{=yRFSzSDZ}Your family lived in a village, but did not own their own land. Instead, your father supplemented paid jobs with long trips in the woods, hunting and trapping, always keeping a wary eye for the lord's game wardens.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireHunterNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireHunterNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EmpireHunterNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("empire_vagabond_option", new TextObject("{=aEke8dSb}Urban vagabonds", null), new TextObject("{=Jvf6K7TZ}Your family numbered among the many poor migrants living in the slums that grow up outside the walls of imperial cities, making whatever money they could from a variety of odd jobs. Sometimes they did service for one of the Empire's many criminal gangs, and you had an early look at the dark side of life.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireVagabondNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireVagabondNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EmpireVagabondNarrativeOptionOnSelect), null));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("khuzait_retainer_option", new TextObject("{=FVaRDe2a}A noyan's kinsfolk", null), new TextObject("{=jAs3kDXh}Your family were the trusted kinsfolk of a Khuzait noyan, and shared his meals in the chieftain's yurt. Your father assisted his chief in running the affairs of the clan and fought in the core of armored lancers in the center of the Khuzait battle line.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitRetainerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitRetainerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.KhuzaitRetainerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("khuzait_merhant_option", new TextObject("{=TkgLEDRM}Merchants", null), new TextObject("{=qPg3IDiq}Your family came from one of the merchant clans that dominated the cities in eastern Calradia before the Khuzait conquest. They adjusted quickly to their new masters, keeping the caravan routes running and ensuring that the tariff revenues that once went into imperial coffers now flowed to the khanate.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitMerchantNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitMerchantNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.KhuzaitMerchantNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("khuzait_mercenary_option", new TextObject("{=tGEStbxb}Tribespeople", null), new TextObject("{=URgZ4ai4}Your family were middle-ranking members of one of the Khuzait clans. He had some herds of his own, but was not rich. When the Khuzait horde was summoned to battle, he fought with the horse archers, shooting and wheeling and wearing down the enemy before the lancers delivered the final punch.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitHerderNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitHerderNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.KhuzaitHerderNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("khuzait_farmer_option", new TextObject("{=gQ2tAvCz}Farmers", null), new TextObject("{=5QSGoRFj}Your family tilled one of the small patches of arable land in the steppes for generations. When the Khuzaits came, they ceased paying taxes to the emperor and providing conscripts for his army, and served the khan instead.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitFarmerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitFarmerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.KhuzaitFarmerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("khuzait_healer_option", new TextObject("{=vfhVveLW}Shamans", null), new TextObject("{=WOKNhaG2}Your family were guardians of the sacred traditions of the Khuzaits, channelling the spirits of the wilderness and of the ancestors. They tended the sick and dispensed wisdom, resolving disputes and providing practical advice.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitHealerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitHealerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.KhuzaitHealerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("khuzait_herder_option", new TextObject("{=Xqba1Obq}Nomads", null), new TextObject("{=9aoQYpZs}Your family's clan never pledged its loyalty to the khan and never settled down, preferring to live out in the deep steppe away from his authority. They remain some of the finest trackers and scouts in the grasslands, as the ability to spot an enemy coming and move quickly is often all that protects their herds from their neighbors' predations.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitNomadHerderNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitNomadHerderNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.KhuzaitNomadHerderNarrativeOptionOnSelect), null));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("sturgia_companion_option", new TextObject("{=mc78FEbA}A boyar's companions", null), new TextObject("{=hob3WVkU}Your father was a member of a boyar's druzhina, the 'companions' that make up his retinue. He sat at his lord's table in the great hall, oversaw the boyar's estates, and stood by his side in the center of the shield wall in battle.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaCompanionNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaCompanionNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.SturgiaCompanionNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("sturgia_trader_option", new TextObject("{=HqzVBfpl}Urban traders", null), new TextObject("{=bjVMtW3W}Your family were merchants who lived in one of Sturgia's great river ports, organizing the shipment of the north's bounty of furs, honey and other goods to faraway lands.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaTraderNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaTraderNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.SturgiaTraderNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("sturgia_farmer_option", new TextObject("{=zrpqSWSh}Free farmers", null), new TextObject("{=Mcd3ZyKq}Your family had just enough land to feed themselves and make a small profit. People like them were the pillars of the kingdom's economy, as well as the backbone of the levy.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaFarmerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaFarmerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.SturgiaFarmerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("sturgia_artisan_option", new TextObject("{=v48N6h1t}Urban artisans", null), new TextObject("{=ueCm5y1C}Your family owned their own workshop in a city, making goods from raw materials brought in from the countryside. Your father played an active if minor role in the town council, and also served in the militia.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaArtisanNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaArtisanNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.SturgiaArtisanNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("sturgia_hunter_option", new TextObject("{=YcnK0Thk}Hunters", null), new TextObject("{=WyZ2UtFF}Your family had no taste for the authority of the boyars. They made their living deep in the woods, slashing and burning fields which they tended for a year or two before moving on. They hunted and trapped fox, hare, ermine, and other fur-bearing animals.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaHunterNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaHunterNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.SturgiaHunterNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("sturgia_vagabond_option", new TextObject("{=TPoK3GSj}Vagabonds", null), new TextObject("{=2SDWhGmQ}Your family numbered among the poor migrants living in the slums that grow up outside the walls of the river cities, making whatever money they could from a variety of odd jobs. Sometimes they did services for one of the region's many criminal gangs.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaVagabondNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaVagabondNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.SturgiaVagabondNarrativeOptionOnSelect), null));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("vlandia_retainer_option", new TextObject("{=2TptWc4m}A baron's retainers", null), new TextObject("{=0Suu1Q9q}Your father was a bailiff for a local feudal magnate. He looked after his liege's estates, resolved disputes in the village, and helped train the village levy. He rode with the lord's cavalry, fighting as an armored knight.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaRetainerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaRetainerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaRetainerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("vlandia_merchant_option", new TextObject("{=651FhzdR}Urban merchants", null), new TextObject("{=qNZFkxJb}Your family were merchants in one of the main cities of the kingdom. They organized caravans to nearby towns and were active in the local merchant's guild.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaMerchantNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaMerchantNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaMerchantNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("vlandia_farmer_option", new TextObject("{=RDfXuVxT}Yeomen", null), new TextObject("{=BLZ4mdhb}Your family were small farmers with just enough land to feed themselves and make a small profit. People like them were the pillars of the kingdom's economy, as well as the backbone of the levy.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaFarmerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaFarmerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaFarmerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("vlandia_blacksmith_option", new TextObject("{=p2KIhGbE}Urban blacksmith", null), new TextObject("{=btsMpRcA}Your family owned a smithy in a city. Your father played an active if minor role in the town council, and also served in the militia.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaBlacksmithNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaBlacksmithNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaBlacksmithNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("vlandia_hunter_option", new TextObject("{=YcnK0Thk}Hunters", null), new TextObject("{=yRFSzSDZ}Your family lived in a village, but did not own their own land. Instead, your father supplemented paid jobs with long trips in the woods, hunting and trapping, always keeping a wary eye for the lord's game wardens.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaHunterNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaHunterNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaHunterNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("vlandia_mercenary_option", new TextObject("{=ipQP6aVi}Mercenaries", null), new TextObject("{=yYhX6JQC}Your father joined one of Vlandia's many mercenary companies, composed of men who got such a taste for war in their lord's service that they never took well to peace. Their crossbowmen were much valued across Calradia. Your mother was a camp follower, taking you along in the wake of bloody campaigns.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaMercenaryNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaMercenaryNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaMercenaryNarrativeOptionOnSelect), null));

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

        public List<NarrativeMenuCharacterArgs> GetChildhoodMenuNarrativeMenuCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            List<NarrativeMenuCharacterArgs> list = new List<NarrativeMenuCharacterArgs>();
            string playerChildhoodAgeEquipmentId = this.GetPlayerChildhoodAgeEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            list.Add(new NarrativeMenuCharacterArgs("player_childhood_character", 7, playerChildhoodAgeEquipmentId, "act_childhood_schooled", "spawnpoint_player_1", "", "", null, true, CharacterObject.PlayerCharacter.IsFemale));
            return list;
        }
        public void AddChildhoodMenu(CharacterCreationManager characterCreationManager)
        {
            List<NarrativeMenuCharacter> list = new List<NarrativeMenuCharacter>();
            BodyProperties bodyProperties = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1);
            bodyProperties = FaceGen.GetBodyPropertiesWithAge(ref bodyProperties, 7f);
            list.Add(new NarrativeMenuCharacter("player_childhood_character", bodyProperties, CharacterObject.PlayerCharacter.Race, CharacterObject.PlayerCharacter.IsFemale));
            NarrativeMenu narrativeMenu = new NarrativeMenu("narrative_childhood_menu", "narrative_parent_menu", "narrative_education_menu", new TextObject("{=8Yiwt1z6}Early Childhood", null), new TextObject("{=character_creation_content_16}As a child you were noted for...", null), list, new NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate(this.GetChildhoodMenuNarrativeMenuCharacterArgs));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("childhood_leadership_option", new TextObject("{=kmM68Qx4}your leadership skills.", null), new TextObject("{=FfNwXtii}If the wolf pup gang of your early childhood had an alpha, it was definitely you. All the other kids followed your lead as you decided what to play and where to play, and led them in games and mischief.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetChildhoodLeadershipOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ChildhoodLeadershipOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ChildhoodLeadershipOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("childhood_brawn_option", new TextObject("{=5HXS8HEY}your brawn.", null), new TextObject("{=YKzuGc54}You were big, and other children looked to have you around in any scrap with children from a neighboring village. You pushed a plough and threw an axe like an adult.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetChildhoodBrawnOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ChildhoodBrawnOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ChildhoodBrawnOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("childhood_detail_option", new TextObject("{=QrYjPUEf}your attention to detail.", null), new TextObject("{=JUSHAPnu}You were quick on your feet and attentive to what was going on around you. Usually you could run away from trouble, though you could give a good account of yourself in a fight with other children if cornered.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetChildhoodDetailOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ChildhoodDetailOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ChildhoodDetailOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("childhood_smart_option", new TextObject("{=Y3UcaX74}your aptitude for numbers.", null), new TextObject("{=DFidSjIf}Most children around you had only the most rudimentary education, but you lingered after class to study letters and mathematics. You were fascinated by the marketplace - weights and measures, tallies and accounts, the chatter about profits and losses.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetChildhoodSmartOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ChildhoodSmartOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ChildhoodSmartOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("childhood_leader_option", new TextObject("{=GEYzLuwb}your way with people.", null), new TextObject("{=w2TEQq26}You were always attentive to other people, good at guessing their motivations. You studied how individuals were swayed, and tried out what you learned from adults on your friends.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetChildhoodLeaderOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ChildhoodLeaderOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ChildhoodLeaderOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("childhood_horse_option", new TextObject("{=MEgLE2kj}your skill with horses.", null), new TextObject("{=ngazFofr}You were always drawn to animals, and spent as much time as possible hanging out in the village stables. You could calm horses, and were sometimes called upon to break in new colts. You learned the basics of veterinary arts, much of which is applicable to humans as well.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetChildhoodHorseOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.ChildhoodHorseOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.ChildhoodHorseOptionOnSelect), null));
            characterCreationManager.AddNewMenu(narrativeMenu);
        }

        public void GetChildhoodLeadershipOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Leadership, DefaultSkills.Tactics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }

        public bool ChildhoodLeadershipOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void ChildhoodLeadershipOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_childhood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_leader");
                }
            }
        }

        public void GetChildhoodBrawnOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.TwoHanded, DefaultSkills.Throwing };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }

        public bool ChildhoodBrawnOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void ChildhoodBrawnOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_childhood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_athlete");
                }
            }
        }

        public void GetChildhoodDetailOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Athletics, DefaultSkills.Bow };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }

        public bool ChildhoodDetailOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void ChildhoodDetailOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_childhood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_memory");
                }
            }
        }

        public void GetChildhoodSmartOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Engineering, DefaultSkills.Trade };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool ChildhoodSmartOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void ChildhoodSmartOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_childhood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_numbers");
                }
            }
        }

        public void GetChildhoodLeaderOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Charm, DefaultSkills.Leadership };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }

        public bool ChildhoodLeaderOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void ChildhoodLeaderOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_childhood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_manners");
                }
            }
        }

        public void GetChildhoodHorseOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.Medicine };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool ChildhoodHorseOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        public void ChildhoodHorseOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_childhood_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_animals");
                }
            }
        }

        /// <summary>
        /// Education menu
        /// </summary>
        public List<NarrativeMenuCharacterArgs> GetEducationMenuNarrativeMenuCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            List<NarrativeMenuCharacterArgs> list = new List<NarrativeMenuCharacterArgs>();
            string playerEducationAgeEquipmentId = this.GetPlayerEducationAgeEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            list.Add(new NarrativeMenuCharacterArgs("player_education_character", 12, playerEducationAgeEquipmentId, "act_childhood_schooled", "spawnpoint_player_1", "", "", null, true, CharacterObject.PlayerCharacter.IsFemale));
            return list;
        }

        new public void AddEducationMenu(CharacterCreationManager characterCreationManager)
        {
            BodyProperties bodyProperties = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1);
            bodyProperties = FaceGen.GetBodyPropertiesWithAge(ref bodyProperties, 12f);
            List<NarrativeMenuCharacter> list = new List<NarrativeMenuCharacter>();
            list.Add(new NarrativeMenuCharacter("player_education_character", bodyProperties, CharacterObject.PlayerCharacter.Race, CharacterObject.PlayerCharacter.IsFemale));
            NarrativeMenu narrativeMenu = new NarrativeMenu("narrative_education_menu", "narrative_childhood_menu", "narrative_youth_menu", new TextObject("{=rcoueCmk}Adolescence", null), new TextObject("{=WYvnWcXQ}Like all village children you helped out in the fields. You also...", null), list, new NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate(this.GetEducationMenuNarrativeMenuCharacterArgs));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("education_herder_option", new TextObject("{=RKVNvimC}herded the sheep.", null), new TextObject("{=KfaqPpbK}You went with other fleet-footed youths to take the villages' sheep, goats or cattle to graze in pastures near the village. You were in charge of chasing down stray beasts, and always kept a big stone on hand to be hurled at lurking predators if necessary.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEducationHerderOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationHerderOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationHerderOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("education_smith_option", new TextObject("{=bTKiN0hr}worked in the village smithy.", null), new TextObject("{=y6j1bJTH}You were apprenticed to the local smith. You learned how to heat and forge metal, hammering for hours at a time until your muscles ached.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEducationSmithOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationSmithOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationSmithOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("education_engineer_option", new TextObject("{=tI8ZLtoA}repaired projects.", null), new TextObject("{=6LFj919J}You helped dig wells, rethatch houses, and fix broken plows. You learned about the basics of construction, as well as what it takes to keep a farming community prosperous.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEducationEngineerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationEngineerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationEngineerOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("education_doctor_option", new TextObject("{=TRwgSLD2}gathered herbs in the wild.", null), new TextObject("{=9ks4u5cH}You were sent by the village healer up into the hills to look for useful medicinal plants. You learned which herbs healed wounds or brought down a fever, and how to find them.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEducationDoctorOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationDoctorOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationDoctorOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("education_hunter_option", new TextObject("{=T7m7ReTq}hunted small game.", null), new TextObject("{=RuvSk3QT}You accompanied a local hunter as he went into the wilderness, helping him set up traps and catch small animals.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEducationHunterOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationHunterOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationHunterOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("education_merchant_option", new TextObject("{=qAbMagWq}sold product at the market.", null), new TextObject("{=DIgsfYfz}You took your family's goods to the nearest town to sell your produce and buy supplies. It was hard work, but you enjoyed the hubbub of the marketplace.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEducationMerchantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationMerchantOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationMerchantOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("education_watcher_option", new TextObject("{=go7Yu7KS}watched the militia training.", null), new TextObject("{=qnqdEJOv}You watched the town's watch practice shooting and perfect their plans to defend the walls in case of a siege.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEducationWatcherOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationWatcherOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationWatcherOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("education_ganger_option", new TextObject("{=gAjvAGTa}hung out with the gangs in the alleys.", null), new TextObject("{=1SUTcF0J}The gang leaders who kept watch over the slums of Calradian cities were always in need of poor youth to run messages and back them up in turf wars, while thrill-seeking merchants' sons and daughters sometimes slummed it in their company as well.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEducationGangerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationGangerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationGangerOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("education_docker_option", new TextObject("{=QVVCgajg}helped at building sites.", null), new TextObject("{=bhdkegZ4}All towns had their share of projects that were constantly in need of both skilled and unskilled labor. You learned how hoists and scaffolds were constructed, how planks and stones were hewn and fitted, and other skills.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEducationDockerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationDockerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationDockerOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("education_marketer_option", new TextObject("{=JTsv6PFe}worked in the markets and caravanserais.", null), new TextObject("{=rmMcwSn8}You helped your family handle their business affairs, going down to the marketplace to make purchases and oversee the arrival of caravans.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEducationMarketerOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationMarketerOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationMarketerOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("education_tutor_option", new TextObject("{=EMVojYzW}studied with your public tutor.", null), new TextObject("{=hXl25avg}Your family arranged for a public tutor and you took full advantage, reading voraciously on history, mathematics, and philosophy and discussing what you read with your tutor and classmates.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEducationTutorOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationTutorOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationTutorOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("education_horser_option", new TextObject("{=hin3iA2D}cared for the horses.", null), new TextObject("{=Ghz90npw}Your family owned a few horses at the town stables and you took charge of their care. Many evenings you would take them out beyond the walls and gallup through the fields, racing other youth.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEducationPoorHorserOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EducationPoorHorserOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EducationPoorHorserOptionOnSelect), null));
            characterCreationManager.AddNewMenu(narrativeMenu);
        }

        public void GetEducationHerderOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Athletics, DefaultSkills.Throwing };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }

        public bool EducationHerderOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return !CharacterCreationRedoneSandbox.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
        }

        public void EducationHerderOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public void GetEducationSmithOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.TwoHanded, DefaultSkills.Crafting };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }

        public bool EducationSmithOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return !CharacterCreationRedoneSandbox.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
        }

        public void EducationSmithOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public void GetEducationEngineerOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Engineering };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool EducationEngineerOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return !CharacterCreationRedoneSandbox.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
        }

        public void EducationEngineerOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public void GetEducationDoctorOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Medicine, DefaultSkills.Scouting };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool EducationDoctorOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return !CharacterCreationRedoneSandbox.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
        }

        public void EducationDoctorOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public void GetEducationHunterOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Bow, DefaultSkills.Tactics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }

        public bool EducationHunterOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return !CharacterCreationRedoneSandbox.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
        }

        public void EducationHunterOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public void GetEducationMerchantOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }

        public bool EducationMerchantOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return !CharacterCreationRedoneSandbox.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
        }

        public void EducationMerchantOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public void GetEducationWatcherOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.Tactics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }

        public bool EducationWatcherOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return CharacterCreationRedoneSandbox.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
        }

        public void EducationWatcherOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public void GetEducationGangerOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.OneHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }

        public bool EducationGangerOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return CharacterCreationRedoneSandbox.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
        }

        public void EducationGangerOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public void GetEducationDockerOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Athletics, DefaultSkills.Crafting };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }

        public bool EducationDockerOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return CharacterCreationRedoneSandbox.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
        }

        public void EducationDockerOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public void GetEducationMarketerOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }

        public bool EducationMarketerOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return CharacterCreationRedoneSandbox.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
        }

        public void EducationMarketerOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public void GetEducationTutorOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Engineering, DefaultSkills.Leadership };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool EducationTutorOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return CharacterCreationRedoneSandbox.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
        }

        public void EducationTutorOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public void GetEducationPoorHorserOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.Steward };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool EducationPoorHorserOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return CharacterCreationRedoneSandbox.CharacterOccupationTypes.IsUrbanOccupation(characterCreationManager.CharacterCreationContent.SelectedParentOccupation);
        }

        public void EducationPoorHorserOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public List<NarrativeMenuCharacterArgs> GetYouthMenuNarrativeMenuCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
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
        public void AddYouthMenu(CharacterCreationManager characterCreationManager)
        {
            TextObject description = CharacterObject.PlayerCharacter.IsFemale ? new TextObject("{=5kbeAC7k}In wartorn Calradia, especially in frontier or tribal areas, some women as well as men learn to fight from an early age. You...", null) : new TextObject("{=F7OO5SAa}As a youngster growing up in Calradia, war was never too far away. You...", null);
            BodyProperties bodyProperties = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1);
            bodyProperties = FaceGen.GetBodyPropertiesWithAge(ref bodyProperties, 17f);
            List<NarrativeMenuCharacter> list = new List<NarrativeMenuCharacter>();
            list.Add(new NarrativeMenuCharacter("player_youth_character", bodyProperties, CharacterObject.PlayerCharacter.Race, CharacterObject.PlayerCharacter.IsFemale));
            list.Add(new NarrativeMenuCharacter("narrative_character_horse"));
            NarrativeMenu narrativeMenu = new NarrativeMenu("narrative_youth_menu", "narrative_education_menu", "narrative_adulthood_menu", new TextObject("{=ok8lSW6M}Youth", null), description, list, new NarrativeMenu.GetNarrativeMenuCharacterArgsDelegate(this.GetYouthMenuNarrativeMenuCharacterArgs));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("youth_staff_first_option", new TextObject("{=CITG915d}joined a commander's staff.", null), new TextObject("{=wNHqFlDL}You were chosen by your superior officer to serve an imperial strategos as a courier. You were not given major responsibilities - mostly carrying messages and tending to his horse - but it did give you a chance to see how campaigns were planned and men were deployed in battle.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthStaffOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthStaffOneOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthStaffOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("youth_staff_second_option", new TextObject("{=CITG915d}joined a commander's staff.", null), new TextObject("{=ANbNblaH}You were picked as the courier of the commander of the local forces. You were not given major responsibilities - mostly carrying messages and tending to his horse - but it did give you a chance to see how campaigns were planned and men were deployed in battle.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthStaffOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthStaffTwoOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthStaffOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("youth_groom_option", new TextObject("{=bhE2i6OU}served as a baron's groom.", null), new TextObject("{=i3k7YtA8}You were chosen by a knight to accompany a minor baron of the Vlandian kingdom. You were not given major responsibilities - mostly carrying messages and tending to his horse - but it did give you a chance to see how campaigns were planned and men were deployed in battle.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthGroomOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthGroomOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthGroomOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("youth_servant_first_option", new TextObject("{=F2bgujPo}were a chieftain's servant.", null), new TextObject("{=AXWO4C69}Your were choosen among others to accompany a chieftain of your people. You were not given major responsibilities - mostly carrying messages and tending to his horse - but it did give you a chance to see how campaigns were planned and men were deployed in battle.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthServantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthServantOneOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthServantOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("youth_servant_second_option", new TextObject("{=F2bgujPo}were a chieftain's servant.", null), new TextObject("{=neMCgMZM}Local wise man picked you to become the messenger of a chieftain of your people. You were not given major responsibilities - mostly carrying messages and tending to his horse - but it did give you a chance to see how campaigns were planned and men were deployed in battle.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthServantOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthServantTwoOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthServantOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("youth_cavalry_option", new TextObject("{=h2KnarLL}trained with the cavalry.", null), new TextObject("{=7cHsIMLP}You could never have bought the equipment on your own, but you were a good enough rider so that the local lord lent you a horse and equipment. You joined the armored cavalry, training with the lance.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthCavalryOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthCavalryOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthCavalryOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("youth_hearth_option", new TextObject("{=zsC2t5Hb}trained with the hearth guard.", null), new TextObject("{=RmbWW6Bm}You were a big and imposing enough youth that the chief's guard allowed you to train alongside them, in preparation to join them some day.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthHearthOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthHearthOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthHearthOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("youth_guard_high_register_option", new TextObject("{=aTncHUfL}stood guard with the garrisons.", null), new TextObject("{=63TAYbkx}Urban troops spend much of their time guarding the town walls. Most of their training was in missile weapons, especially useful during sieges.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthGuardHighRegisterOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthGuardHighRegisterOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthGuardHighRegisterOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("youth_guard_low_register_option", new TextObject("{=aTncHUfL}stood guard with the garrisons.", null), new TextObject("{=oR58iNDz}Urban troops spend much of their time guarding the town walls. Most of their training was in missile weapons.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthGuardLowRegisterOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthGuardLowRegisterOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthGuardLowRegisterOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("youth_guard_garrisons_register_option", new TextObject("{=aTncHUfL}stood guard with the garrisons.", null), new TextObject("{=e6lINjFg}The garrisons spent most of their time guarding the town walls, and their training focused largely on missile weapons.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthGuardGarrisonRegisterOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthGuardGarrisonRegisterOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthGuardGarrisonRegisterOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("youth_guard_empire_register_option", new TextObject("{=aTncHUfL}stood guard with the garrisons.", null), new TextObject("{=oR58iNDz}Urban troops spend much of their time guarding the town walls. Most of their training was in missile weapons.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthGuardEmpireRegisterOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthGuardEmpireRegisterOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthGuardEmpireRegisterOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("youth_rider_high_register_option", new TextObject("{=VlXOgIX6}rode with the scouts.", null), new TextObject("{=888lmJqs}All of Calradia's kingdoms recognize the value of good light cavalry and horse archers, and are sure to recruit nomads and borderers with the skills to fulfill those duties. You were a good enough rider that your neighbors pitched in to buy you a small pony and a good bow so that you could fulfill their levy obligations.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthRiderHighRegisterOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthRiderHighRegisterOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthRiderHighRegisterOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("youth_rider_low_register_option", new TextObject("{=VlXOgIX6}rode with the scouts.", null), new TextObject("{=sYuN6hPD}All of Calradia's kingdoms recognize the value of good light cavalry, and are sure to recruit nomads and borderers with the skills to fulfill those duties. You were a good enough rider that your neighbors pitched in to buy you a small pony and a sheaf of javelins so that you could fulfill their levy obligations.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthRiderLowRegisterOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthRiderLowRegisterOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthRiderLowRegisterOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("youth_infantry_option", new TextObject("{=a8arFSra}trained with the infantry.", null), new TextObject("{=afH90aNs}Levy armed with spear and shield, drawn from smallholding farmers, have always been the backbone of most armies of Calradia.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthInfantryOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthInfantryOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthInfantryOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("youth_skirmisher_option", new TextObject("{=oMbOIPc9}joined the skirmishers.", null), new TextObject("{=bXAg5w19}Younger recruits, or those of a slighter build, or those too poor to buy shield and armor tend to join the skirmishers. Fighting with bow and javelin, they try to stay out of reach of the main enemy forces.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthSkirmisherOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthSkirmisherOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthSkirmisherOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("youth_kern_option", new TextObject("{=cDWbwBwI}joined the kern.", null), new TextObject("{=tTb28jyU}Many Battanians fight as kern, versatile troops who could both harass the enemy line with their javelins or join in the final screaming charge once it weakened.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthKernOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthKernOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthKernOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("youth_camp_option", new TextObject("{=GFUggps8}marched with the camp followers.", null), new TextObject("{=64rWqBLN}You avoided service with one of the main forces of your realm's armies, but followed instead in the train - the troops' wives, lovers and servants, and those who make their living by caring for, entertaining, or cheating the soldiery.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetYouthCampOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.YouthCampOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.YouthCampOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("youth_envoys_guard_first_option", new TextObject("{=YmPlLGXb}served as an envoy's guard", null), new TextObject("{=qPamcCkA}Your family arranged for you to accompany an envoy. You were not given major responsibilities - mostly carrying arms and trying to look imposing. - but it did give you a chance to travel a lot and socialise and see the world.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEnvoysGuardFirstOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EnvoysGuardFirstOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EnvoysGuardFirstOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("youth_envoys_guard_second_option", new TextObject("{=YmPlLGXb}served as an envoy's guard", null), new TextObject("{=VYU1nEHP}Your family arranged for you to accompany an envoy. You were not given major responsibilities but it did give you a chance to travel and socialise and see a bit of the world.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEnvoysGuardSecondOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EnvoysGuardSecondOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EnvoysGuardSecondOptionOnSelect), null));
            characterCreationManager.AddNewMenu(narrativeMenu);
        }

        public void GetYouthStaffOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Steward, DefaultSkills.Tactics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }

        public bool YouthStaffOneOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }

        public bool YouthStaffTwoOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }

        public void YouthStaffOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public void GetYouthGroomOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Charm, DefaultSkills.Tactics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }

        public bool YouthGroomOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }

        public void YouthGroomOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public void GetYouthServantOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Steward, DefaultSkills.Tactics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }

        public bool YouthServantOneOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }

        public bool YouthServantTwoOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }

        public void YouthServantOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public void GetYouthCavalryOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.Polearm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool YouthCavalryOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }

        public void YouthCavalryOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public void GetYouthHearthOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.Polearm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool YouthHearthOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }

        public void YouthHearthOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public void GetYouthGuardHighRegisterOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crossbow, DefaultSkills.Engineering };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool YouthGuardHighRegisterOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }

        public void YouthGuardHighRegisterOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public void GetYouthGuardLowRegisterOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Bow, DefaultSkills.Engineering };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool YouthGuardLowRegisterOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }

        public void YouthGuardLowRegisterOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public void GetYouthGuardGarrisonRegisterOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Bow, DefaultSkills.Engineering };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool YouthGuardGarrisonRegisterOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }

        public void YouthGuardGarrisonRegisterOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public void GetYouthGuardEmpireRegisterOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crossbow, DefaultSkills.Engineering };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 2);
        }

        public bool YouthGuardEmpireRegisterOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }

        public void YouthGuardEmpireRegisterOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public void GetYouthRiderHighRegisterOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.Bow };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool YouthRiderHighRegisterOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }

        public void YouthRiderHighRegisterOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "hunter";
            string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_youth_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_sturgia_mp_warrior_axe");
                    narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
                }
            }
        }

        public void GetYouthRiderLowRegisterOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.Bow };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 2);
        }

        public bool YouthRiderLowRegisterOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }

        public void YouthRiderLowRegisterOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "hunter";
            string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_youth_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_sturgia_mp_huskarl_idle");
                    narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
                }
            }
        }

        public void GetYouthInfantryOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Polearm, DefaultSkills.OneHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 2);
        }

        public bool YouthInfantryOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }

        public void YouthInfantryOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "infantry";
            string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_youth_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_fierce");
                    narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
                }
            }
        }

        public void GetYouthSkirmisherOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Throwing, DefaultSkills.OneHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }

        public bool YouthSkirmisherOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }

        public void YouthSkirmisherOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "skirmisher";
            string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_youth_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_fox");
                    narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
                }
            }
        }

        public void GetYouthKernOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Throwing, DefaultSkills.OneHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 2);
        }

        public bool YouthKernOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }

        public void YouthKernOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "kern";
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

        public void GetYouthCampOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Roguery, DefaultSkills.Throwing };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 2);
        }

        public bool YouthCampOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }

        public void YouthCampOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SelectedTitleType = "bard";
            string playerEquipmentId = this.GetPlayerEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedTitleType, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId, Hero.MainHero.IsFemale);
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "player_youth_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_childhood_militia");
                    narrativeMenuCharacter.SetEquipment(Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(playerEquipmentId));
                }
            }
        }

        public void GetEnvoysGuardFirstOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Charm, DefaultSkills.Scouting };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }

        public void GetEnvoysGuardSecondOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Charm, DefaultSkills.Scouting };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(30);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 2);
        }

        public bool EnvoysGuardFirstOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }

        public bool EnvoysGuardSecondOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania" || characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }

        public void EnvoysGuardFirstOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public void EnvoysGuardSecondOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        public readonly IReadOnlyDictionary<string, string> _occupationToEquipmentMapping = new Dictionary<string, string>
        {
            {
                "retainer",
                "retainer"
            },
            {
                "bard",
                "bard"
            },
            {
                "hunter",
                "hunter"
            },
            {
                "farmer",
                "farmer"
            },
            {
                "herder",
                "herder"
            },
            {
                "healer",
                "healer"
            },
            {
                "mercenary",
                "mercenary"
            },
            {
                "infantry",
                "infantry"
            },
            {
                "skirmisher",
                "skirmisher"
            },
            {
                "kern",
                "kern"
            },
            {
                "guard",
                "guard"
            },
            {
                "retainer_urban",
                "retainer"
            },
            {
                "mercenary_urban",
                "mercenary"
            },
            {
                "merchant_urban",
                "merchant"
            },
            {
                "vagabond_urban",
                "vagabond"
            },
            {
                "artisan_urban",
                "artisan"
            },
            {
                "physician_urban",
                "physician"
            },
            {
                "healer_urban",
                "healer"
            },
            {
                "bard_urban",
                "bard"
            }
        };

        new public const int FocusToAddYouthStart = 2;

        new public const int FocusToAddAdultStart = 4;

        new public const int FocusToAddMiddleAgedStart = 6;

        new public const int FocusToAddElderlyStart = 8;

        new public const int AttributeToAddYouthStart = 1;

        new public const int AttributeToAddAdultStart = 2;

        new public const int AttributeToAddMiddleAgedStart = 3;

        new public const int AttributeToAddElderlyStart = 4;

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
            public static bool IsUrbanOccupation(string occupation)
            {
                return occupation == "retainer_urban" || occupation == "mercenary_urban" || occupation == "merchant_urban" || occupation == "vagabond_urban" || occupation == "artisan_urban" || occupation == "physician_urban" || occupation == "healer_urban" || occupation == "bard_urban";
            }

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
