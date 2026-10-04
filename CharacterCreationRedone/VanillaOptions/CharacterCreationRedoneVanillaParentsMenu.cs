using NavalDLC.CharacterDevelopment;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace CharacterCreationRedone.VanillaOptions
{
    public class CharacterCreationRedoneVanillaParentsMenu : CharacterCreationCampaignBehavior, ICharacterCreationContentHandler
    {
        /// <summary>
        /// I've split the different steps of the backstory options to make it easier to work with, if you want it to be ina single file : https://github.com/Sh1ny4/CharacterCreationRedone/blob/bc3fd947cef49e8275fa729928d9743b5ea64abe/CharacterCreationRedone/CharacterCreationOptions/CharacterCreationRedoneVanilla.cs
        /// a lot has changed in the 1.3 update so here is what I found :
        /// it is now possible to add options without this patching, as seen in the war sails DLC. this is allowed by having a narrative menu ID. Check the DLC code to see how to implement it since it is not the idea behind this mod
        /// 
        /// Each menu option has 4 inputs : Condition, Args, OnSelect and Consequences
        ///     Condition input : allows you to limit which options are available, can be the cultures, for the parents to be noble, having a specifc trait, being a woman, etc
        ///     Args  : contains what will be affected by your choice like focus, skill level, attributes, traits, etc
        ///     OnSelect : mostly used to change what is displayed like the equipement and the animation
        ///     Consequences : is optional, it can be used to change what isn't available in args. I have used it to have the player be part of a kingdom, increase the clan level, change the gold, have a companion or give the player a criminal rating
        /// 
        /// A lot more can be done with this, like having a section that is purely a starting gear choice and each option costing a certain amount or having a menu option that allow you to select in which place to spawn
        /// 
        /// </summary>

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
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("aserai_ferryman_option", new TextObject("{=PaXaNLrb}Ferryman", null), new TextObject("{=LtOCnEC8}Your family are from a small rural community along a river bank where they operated a small ferry to transport goods and people across the river, connecting rural communities. You learned about boats and ebbs and flows of the river navigation.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAseraiFerrymanNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiFerrymanNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.GetAseraiFerrymanNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("aserai_corsair_traders_option", new TextObject("{=V0IGaFFn}Corsair Traders", null), new TextObject("{=Gl5CFpEM}Raised on Aserai dhows, your father thought you about the trade winds and routes. The ship you were raised in made long and tedious voyages, smuggling silks and spices or ambushing Vlandian ships when profits dwindled.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetAseraiCorsairTradersNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.AseraiCorsairTradersNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.GetAseraiCorsairTradersNarrativeOptionOnSelect), null));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("battania_retainer_option", new TextObject("{=GeNKQlHR}Members of the chieftain's hearthguard", null), new TextObject("{=LpH8SYFL}Your family were the trusted kinfolk of a Battanian chieftain, and sat at his table in his great hall. Your father assisted his chief in running the affairs of the clan and trained with the traditional weapons of the Battanian elite, the two-handed sword or falx and the bow.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaRetainerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaRetainerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.BattaniaRetainerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("battania_healer_option", new TextObject("{=AeBzTj6w}Healers", null), new TextObject("{=j6py5Rv5}Your parents were healers who gathered herbs and treated the sick. As a living reservoir of Battanian tradition, they were also asked to adjudicate many disputes between the clans.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaHealerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaHealerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.BattaniaHealerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("battania_farmer_option", new TextObject("{=tGEStbxb}Tribespeople", null), new TextObject("{=WchH8bS2}Your family were middle-ranking members of a Battanian clan, who tilled their own land. Your father fought with the kern, the main body of his people's warriors, joining in the screaming charges for which the Battanians were famous.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaFarmerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaFarmerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.BattaniaFarmerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("battania_artisan_option", new TextObject("{=BCU6RezA}Smiths", null), new TextObject("{=kg9YtrOg}Your family were smiths, a revered profession among the Battanians. They crafted everything from fine filigree jewelry in geometric designs to the well-balanced longswords favored by the Battanian aristocracy.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaArtisanNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaArtisanNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.BattaniaArtisanNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("battania_hunter_option", new TextObject("{=7eWmU2mF}Foresters", null), new TextObject("{=7jBroUUQ}Your family had little land of their own, so they earned their living from the woods, hunting and trapping. They taught you from an early age that skills like finding game trails and killing an animal with one shot could make the difference between eating and starvation.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaHunterNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaHunterNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.BattaniaHunterNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("battania_bard_option", new TextObject("{=SpJqhEEh}Bards", null), new TextObject("{=aVzcyhhy}Your father was a bard, drifting from chieftain's hall to chieftain's hall making his living singing the praises of one Battanian aristocrat and mocking his enemies, then going to his enemy's hall and doing the reverse. You learned from him that a clever tongue could spare you  from a life toiling in the fields, if you kept your wits about you.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaBardNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaBardNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.BattaniaBardNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("battania_currach_sailors_option", new TextObject("{=4zNU0J1S}Currach Sailors", null), new TextObject("{=bnrmJHc6}Your kin braved the lakes and rivers in hide-covered currachs, fishing icy waters and facing the dangers of strong currents and occasional banditry. You grew up learning to navigate the treacherous waters and to defend yourself from those who would prey on the river traffic.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaCurrachSailorsNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaCurrachSailorsNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.GetBattaniaCurrachSailorsNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("battania_guardian_of_the_lake_option", new TextObject("{=o7BFw2WW}Guardian of the Lake", null), new TextObject("{=ydyaMa6E}Your kin were part of a group of warriors tasked with maintaining small boats for defense or patrol of vital waterways, protecting it from raiders or invaders. While they weren't around much while you were growing up, you still earned some riverine navigation and combat skills.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetBattaniaGuardianOfTheLakeNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.BattaniaGuardianOfTheLakeNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.GetBattaniaGuardianOfTheLakeNarrativeOptionOnSelect), null));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("empire_lanlord_option", new TextObject("{=InN5ZZt3}A landlord's retainers", null), new TextObject("{=ivKl4mV2}Your father was a trusted lieutenant of the local landowning aristocrat. He rode with the lord's cavalry, fighting as an armored lancer.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireLandlordNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireLandlordNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EmpireLandlordNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("empire_merchant_option", new TextObject("{=651FhzdR}Urban merchants", null), new TextObject("{=FQntPChs}Your family were merchants in one of the main cities of the Empire. They sometimes organized caravans to nearby towns, and discussed issues in the town council.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireUrbanNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireUrbanNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EmpireUrbanNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("empire_farmer_option", new TextObject("{=sb4gg8Ak}Freeholders", null), new TextObject("{=09z8Q08f}Your family were small farmers with just enough land to feed themselves and make a small profit. People like them were the pillars of the imperial rural economy, as well as the backbone of the levy.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireFarmerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireFarmerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EmpireFarmerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("empire_artisan_option", new TextObject("{=v48N6h1t}Urban artisans", null), new TextObject("{=ueCm5y1C}Your family owned their own workshop in a city, making goods from raw materials brought in from the countryside. Your father played an active if minor role in the town council, and also served in the militia.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireArtisanNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireArtisanNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EmpireArtisanNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("empire_hunter_option", new TextObject("{=7eWmU2mF}Foresters", null), new TextObject("{=yRFSzSDZ}Your family lived in a village, but did not own their own land. Instead, your father supplemented paid jobs with long trips in the woods, hunting and trapping, always keeping a wary eye for the lord's game wardens.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireHunterNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireHunterNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EmpireHunterNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("empire_vagabond_option", new TextObject("{=aEke8dSb}Urban vagabonds", null), new TextObject("{=Jvf6K7TZ}Your family numbered among the many poor migrants living in the slums that grow up outside the walls of imperial cities, making whatever money they could from a variety of odd jobs. Sometimes they did service for one of the Empire's many criminal gangs, and you had an early look at the dark side of life.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireVagabondNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireVagabondNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.EmpireVagabondNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("empire_small_boat_fisherman_option", new TextObject("{=e1aebAAL}Small Boat Fisherman", null), new TextObject("{=nBr0jL3X}Your family inhabited a small, relatively isolated coastal village within the Empire. They foraged along the shoreline for fish using small boats. You grew up with the smell of salt and the rhythm of the tides, learning to navigate close to shore and brave the smaller waves in your sturdy little vessel.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireSmallBoatFishermanNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireSmallBoatFishermanNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.GetEmpireSmallBoatFishermanNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("empire_imperial_fleet_option", new TextObject("{=LdCQfaUi}Imperial Fleet", null), new TextObject("{=N6o7Gnpz}Your father served in one the Imperial Navy's liburna as a quartermaster. He bought supplies for the crew and basically kept the ship running. He wanted the same path for you so you were schooled in trading and ship maintenance.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetEmpireImperialFleetNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.EmpireImperialFleetNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.GetEmpireImperialFleetNarrativeOptionOnSelect), null));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("khuzait_retainer_option", new TextObject("{=FVaRDe2a}A noyan's kinsfolk", null), new TextObject("{=jAs3kDXh}Your family were the trusted kinsfolk of a Khuzait noyan, and shared his meals in the chieftain's yurt. Your father assisted his chief in running the affairs of the clan and fought in the core of armored lancers in the center of the Khuzait battle line.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitRetainerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitRetainerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.KhuzaitRetainerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("khuzait_merhant_option", new TextObject("{=TkgLEDRM}Merchants", null), new TextObject("{=qPg3IDiq}Your family came from one of the merchant clans that dominated the cities in eastern Calradia before the Khuzait conquest. They adjusted quickly to their new masters, keeping the caravan routes running and ensuring that the tariff revenues that once went into imperial coffers now flowed to the khanate.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitMerchantNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitMerchantNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.KhuzaitMerchantNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("khuzait_mercenary_option", new TextObject("{=tGEStbxb}Tribespeople", null), new TextObject("{=URgZ4ai4}Your family were middle-ranking members of one of the Khuzait clans. He had some herds of his own, but was not rich. When the Khuzait horde was summoned to battle, he fought with the horse archers, shooting and wheeling and wearing down the enemy before the lancers delivered the final punch.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitHerderNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitHerderNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.KhuzaitHerderNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("khuzait_farmer_option", new TextObject("{=gQ2tAvCz}Farmers", null), new TextObject("{=5QSGoRFj}Your family tilled one of the small patches of arable land in the steppes for generations. When the Khuzaits came, they ceased paying taxes to the emperor and providing conscripts for his army, and served the khan instead.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitFarmerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitFarmerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.KhuzaitFarmerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("khuzait_healer_option", new TextObject("{=vfhVveLW}Shamans", null), new TextObject("{=WOKNhaG2}Your family were guardians of the sacred traditions of the Khuzaits, channelling the spirits of the wilderness and of the ancestors. They tended the sick and dispensed wisdom, resolving disputes and providing practical advice.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitHealerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitHealerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.KhuzaitHealerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("khuzait_herder_option", new TextObject("{=Xqba1Obq}Nomads", null), new TextObject("{=9aoQYpZs}Your family's clan never pledged its loyalty to the khan and never settled down, preferring to live out in the deep steppe away from his authority. They remain some of the finest trackers and scouts in the grasslands, as the ability to spot an enemy coming and move quickly is often all that protects their herds from their neighbors' predations.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitNomadHerderNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitNomadHerderNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.KhuzaitNomadHerderNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("khuzait_river_foragers_option", new TextObject("{=fEIzJtSF}River Foragers", null), new TextObject("{=2rNqqZnm}Along the winding veins of a major river that cuts through the steppe, your family carved a life from the water's edge. Using small, makeshift rafts and boats, they developed a keen eye for the river's bounty, gathering specific plants from its banks and fishing in its shallows. From your humble parents, you inherited a deep well of knowledge.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitRiverForagersNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitRiverForagersNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.GetKhuzaitRiverForagersNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("khuzait_river_traders_option", new TextObject("{=DQQogYtq}River Traders", null), new TextObject("{=enS8isiB}Your family transports goods and people along the river, facing the dangers of strong currents and occasional banditry. You grew up learning to navigate the treacherous waters and to defend yourself from those who would prey on the river traffic.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetKhuzaitRiverTradersNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.KhuzaitRiverTradersNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.GetKhuzaitRiverTradersNarrativeOptionOnSelect), null));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("nord_hersir_option", new TextObject("{=DRC5bTE5}Hersir", null), new TextObject("{=w3AI4lwM}Your family's loyalty ran deep, not in sprawling lands or grand titles, but in service. For generations, they'd served as hersirs, the trusted retainers, for a minor Jarl who kept watch over a windswept corner of the Nord territory.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetNordHersirNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.NordHersirNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.NordHersirNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("nord_market_trader_option", new TextObject("{=uqpHfuZV}Peddler", null), new TextObject("{=DvgmjoCE}You grew up amidst the bustling chaos of a Norse market town, a hub of trade where goods from across the known world exchanged hands. Your family were established traders, perhaps dealing in furs, amber, crafted goods, or even imported luxuries. You learned the art of negotiation, the value of different commodities, and the diverse languages and customs of the merchants who passed through. The market was your school, and shrewd dealing your lesson.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetNordMarketTraderNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.NordMarketTraderNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.NordMarketTraderNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("nord_skald_option", new TextObject("{=1lX8eks5}Travelling skalds", null), new TextObject("{=KtucaHqd}Your family's voices carried the tales of the North. Not grand courtly Skalds, but traveling storytellers with weathered cloaks and worn lutes. They wandered from village to village, weaving tales of heroes and hearth into songs and sagas. You grew up surrounded by the rhythmic strum of their instruments and the flickering firelight reflecting off their eyes as they spun fantastical yarns. These weren't just stories - they were the beating heart of Nord culture, passed down from generation to generation by your family's calloused hands and booming voices.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetNordSkaldNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.NordSkaldNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.NordSkaldNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("nord_blacksmith_option", new TextObject("{=v48N6h1t}Urban artisans", null), new TextObject("{=AAHhp1ly}The clang of hammer on hot iron was the defining sound of your upbringing. Your family were more than mere smiths; they were artisans who coaxed wonders from limited resources, shaping valuable iron into formidable weapons and treasured tools. From the forge, you learned to work with what little you had, understanding the unique properties of each piece and the almost magical skill required to transform it.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetNordBlacksmithNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.NordBlacksmithNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.NordBlacksmithNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("nord_hunter_option", new TextObject("{=izTHRXo5}Hunters", null), new TextObject("{=rdRamFhv}You were born into a family of foresters living off the land. You learned to track prey, hunt for sustenance and gathering herbs and mushrooms from a young age. The forest provided, but it also demanded respect. You learned the medicinal properties of plants and mushrooms for the inevitable scrapes and ailments that came with life in the wild. The harsh environment became your teacher, and survival your greatest lesson.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetNordHunterNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.NordHunterNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.NordHunterNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("nord_vagabonds_option", new TextObject("{=TPoK3GSj}Vagabonds", null), new TextObject("{=nrtrMbLx}You were part of a tight-knit family scraping by on the fringes of a bustling Nord port. Hard work wasn't always an option, and your kin did what they had to - unloading ships one day, \"borrowing\" a stray coin the next. Life was rough, lessons learned on cobblestone streets, but the fierce loyalty that bound your family together was stronger than any harbor wall.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetNordVagabondNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.NordVagabondNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.NordVagabondNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("nord_sailors_option", new TextObject("{=6aKaV4ua}Sailors", null), new TextObject("{=BbOM3F8H}Your family was a tight-knit crew on a sturdy fishing vessel. They weren't charting uncharted seas, but venturing just beyond the familiar fjords, bartering with coastal settlements for smoked fish and bragging rights about the biggest catch. Tales of faraway lands might have been spun under flickering lanterns, but the reality was weathered sails, calloused hands, and a knack for reading the temperamental sea.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetNordSailorsNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.NordSailorsNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.GetNordSailorsNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("nord_shipwrights_option", new TextObject("{=WYS68dRq}Shipwrights", null), new TextObject("{=qUwVnncn}Your kin weren't grand shipwrights building mighty drakkars, but a family of skilled boatbuilders crafting sturdy vessels. Their longships weren't feared in battle, but prized for braving the treacherous coasts. Each plank and sail held the legacy of generations, passed down through calloused hands and the rhythmic tap of the hammer.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetNordShipwrightsNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.NordShipwrightsNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.GetNordShipwrightsNarrativeOptionOnSelect), null));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("sturgia_companion_option", new TextObject("{=mc78FEbA}A boyar's companions", null), new TextObject("{=hob3WVkU}Your father was a member of a boyar's druzhina, the 'companions' that make up his retinue. He sat at his lord's table in the great hall, oversaw the boyar's estates, and stood by his side in the center of the shield wall in battle.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaCompanionNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaCompanionNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.SturgiaCompanionNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("sturgia_trader_option", new TextObject("{=HqzVBfpl}Urban traders", null), new TextObject("{=bjVMtW3W}Your family were merchants who lived in one of Sturgia's great river ports, organizing the shipment of the north's bounty of furs, honey and other goods to faraway lands.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaTraderNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaTraderNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.SturgiaTraderNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("sturgia_farmer_option", new TextObject("{=zrpqSWSh}Free farmers", null), new TextObject("{=Mcd3ZyKq}Your family had just enough land to feed themselves and make a small profit. People like them were the pillars of the kingdom's economy, as well as the backbone of the levy.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaFarmerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaFarmerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.SturgiaFarmerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("sturgia_artisan_option", new TextObject("{=v48N6h1t}Urban artisans", null), new TextObject("{=ueCm5y1C}Your family owned their own workshop in a city, making goods from raw materials brought in from the countryside. Your father played an active if minor role in the town council, and also served in the militia.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaArtisanNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaArtisanNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.SturgiaArtisanNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("sturgia_hunter_option", new TextObject("{=YcnK0Thk}Hunters", null), new TextObject("{=WyZ2UtFF}Your family had no taste for the authority of the boyars. They made their living deep in the woods, slashing and burning fields which they tended for a year or two before moving on. They hunted and trapped fox, hare, ermine, and other fur-bearing animals.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaHunterNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaHunterNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.SturgiaHunterNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("sturgia_vagabond_option", new TextObject("{=TPoK3GSj}Vagabonds", null), new TextObject("{=2SDWhGmQ}Your family numbered among the poor migrants living in the slums that grow up outside the walls of the river cities, making whatever money they could from a variety of odd jobs. Sometimes they did services for one of the region's many criminal gangs.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaVagabondNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaVagabondNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.SturgiaVagabondNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("sturgia_river_fisherman_option", new TextObject("{=iuAi8rZ4}River Fisherman", null), new TextObject("{=gpNBMzW8}Your family lived by the water, skilled in casting nets, setting lines, and mending the wear and tear of daily fishing. You understood the currents, the seasons of the fish, and the importance of a good catch for your community. Life was dictated by the river's flow and its bounty.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaRiverFishermanNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaRiverFishermanNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.GetSturgiaRiverFishermanNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("sturgia_shipbuilders_option", new TextObject("{=V0GSUvaU}Shipbuilders", null), new TextObject("{=9XmQrI23}Your family builded longships for the Sturgian river lords. You grew up amidst the sounds of hammering and the smell of tar, learning the craft of shipbuilding from your father and uncles.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetSturgiaShipbuildersNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.SturgiaShipbuildersNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.GetSturgiaShipbuildersNarrativeOptionOnSelect), null));

            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("vlandia_retainer_option", new TextObject("{=2TptWc4m}A baron's retainers", null), new TextObject("{=0Suu1Q9q}Your father was a bailiff for a local feudal magnate. He looked after his liege's estates, resolved disputes in the village, and helped train the village levy. He rode with the lord's cavalry, fighting as an armored knight.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaRetainerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaRetainerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaRetainerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("vlandia_merchant_option", new TextObject("{=651FhzdR}Urban merchants", null), new TextObject("{=qNZFkxJb}Your family were merchants in one of the main cities of the kingdom. They organized caravans to nearby towns and were active in the local merchant's guild.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaMerchantNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaMerchantNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaMerchantNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("vlandia_farmer_option", new TextObject("{=RDfXuVxT}Yeomen", null), new TextObject("{=BLZ4mdhb}Your family were small farmers with just enough land to feed themselves and make a small profit. People like them were the pillars of the kingdom's economy, as well as the backbone of the levy.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaFarmerNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaFarmerNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaFarmerNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("vlandia_blacksmith_option", new TextObject("{=p2KIhGbE}Urban blacksmith", null), new TextObject("{=btsMpRcA}Your family owned a smithy in a city. Your father played an active if minor role in the town council, and also served in the militia.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaBlacksmithNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaBlacksmithNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaBlacksmithNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("vlandia_hunter_option", new TextObject("{=YcnK0Thk}Hunters", null), new TextObject("{=yRFSzSDZ}Your family lived in a village, but did not own their own land. Instead, your father supplemented paid jobs with long trips in the woods, hunting and trapping, always keeping a wary eye for the lord's game wardens.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaHunterNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaHunterNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaHunterNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("vlandia_mercenary_option", new TextObject("{=ipQP6aVi}Mercenaries", null), new TextObject("{=yYhX6JQC}Your father joined one of Vlandia's many mercenary companies, composed of men who got such a taste for war in their lord's service that they never took well to peace. Their crossbowmen were much valued across Calradia. Your mother was a camp follower, taking you along in the wake of bloody campaigns.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaMercenaryNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaMercenaryNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaMercenaryNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("vlandia_coastal_fisherman_option", new TextObject("{=MPaZbhRc}Coastal fisherman", null), new TextObject("{=VBy8WxVw}Your family has been fishing these waters for generations, struggling to make a living off the unpredictable sea. You grew up mending nets, hauling in catches, and dreaming of a life beyond the constant struggle for survival.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaCoastalFishermanNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaCoastalFishermanNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.VlandiaCoastalFishermanNarrativeOptionOnSelect), null));
            narrativeMenu.AddNarrativeMenuOption(new NarrativeMenuOption("vlandia_dockers_option", new TextObject("{=rsUCF3H8}Dockers", null), new TextObject("{=OyIKF2r6}Your family toiled on the docks, their hands calloused from hauling the endless flow of goods from the sea. A vital but often thankless task that kept Vlandia's ports alive. You learned the rhythm of the tides and the languages of foreign sailors before you learned to read.", null), new GetNarrativeMenuOptionArgsDelegate(this.GetVlandiaDockersNarrativeOptionArgs), new NarrativeMenuOptionOnConditionDelegate(this.VlandiaDockersNarrativeOptionOnCondition), new NarrativeMenuOptionOnSelectDelegate(this.GetVlandiaDockersNarrativeOptionOnSelect), null));

            characterCreationManager.AddNewMenu(narrativeMenu);
        }

        public void GetEmpireLandlordNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.Polearm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 1);
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

        private void GetEmpireSmallBoatFishermanNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { NavalSkills.Boatswain, DefaultSkills.Throwing };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 1);
        }

        private bool EmpireSmallBoatFishermanNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }

        private void GetEmpireSmallBoatFishermanNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("seafarer");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_empire_smallboatfisherman_mother";
            string fatherAnimation = "act_character_creation_empire_smallboatfisherman_father";
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "mother_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_empire_smallboatfisherman_mother");
                }
                if (narrativeMenuCharacter.StringId == "father_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_empire_smallboatfisherman_father");
                    narrativeMenuCharacter.SetLeftHandItem("hanging_fishes");
                    narrativeMenuCharacter.SetRightHandItem("hanging_fishes");
                }
            }
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        private void GetEmpireImperialFleetNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, NavalSkills.Shipmaster };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 1);
        }

        private bool EmpireImperialFleetNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "empire";
        }

        private void GetEmpireImperialFleetNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("shipmaster_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_empire_imperial_fleet_mother";
            string fatherAnimation = "act_character_creation_empire_imperial_fleet_father";
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "mother_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_empire_imperial_fleet_mother");
                }
                if (narrativeMenuCharacter.StringId == "father_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_empire_imperial_fleet_father");
                    narrativeMenuCharacter.SetRightHandItem("book_right_hand");
                }
            }
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetVlandiaRetainerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.Polearm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 1);
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

        private void GetVlandiaCoastalFishermanNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { NavalSkills.Boatswain, DefaultSkills.Scouting };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 1);
        }

        private bool VlandiaCoastalFishermanNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }

        private void VlandiaCoastalFishermanNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("seafarer");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_vlandia_fisherman_mother";
            string fatherAnimation = "act_character_creation_vlandia_fisherman_father";
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "mother_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_vlandia_fisherman_mother");
                    narrativeMenuCharacter.SetLeftHandItem("fishnet_char_creation");
                }
                if (narrativeMenuCharacter.StringId == "father_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_vlandia_fisherman_father");
                    narrativeMenuCharacter.SetRightHandItem("fishing_rod_s");
                }
            }
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        private void GetVlandiaDockersNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { NavalSkills.Shipmaster, DefaultSkills.Athletics };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 1);
        }

        private bool VlandiaDockersNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "vlandia";
        }

        private void GetVlandiaDockersNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("shipmaster_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_vlandia_dockers_mother";
            string fatherAnimation = "act_character_creation_vlandia_dockers_father";
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "mother_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_vlandia_dockers_mother");
                    narrativeMenuCharacter.SetRightHandItem("sack_s");
                }
                if (narrativeMenuCharacter.StringId == "father_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_vlandia_dockers_father");
                    narrativeMenuCharacter.SetRightHandItem("sack");
                }
            }
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }


        public void GetSturgiaCompanionNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.TwoHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 1);
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
        private void GetSturgiaRiverFishermanNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { NavalSkills.Boatswain, DefaultSkills.Throwing };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 1);
        }

        private bool SturgiaRiverFishermanNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }

        private void GetSturgiaRiverFishermanNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("seafarer");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_sturgia_riverfisherman_mother";
            string fatherAnimation = "act_character_creation_sturgia_riverfisherman_father";
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "mother_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_sturgia_riverfisherman_mother");
                }
                if (narrativeMenuCharacter.StringId == "father_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_sturgia_riverfisherman_father");
                    narrativeMenuCharacter.SetLeftHandItem("fishnet");
                }
            }
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        private void GetSturgiaShipbuildersNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { NavalSkills.Shipmaster, DefaultSkills.Engineering };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 1);
        }

        private bool SturgiaShipbuildersNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "sturgia";
        }

        private void GetSturgiaShipbuildersNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("shipmaster_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_sturgia_shipbuilder_mother";
            string fatherAnimation = "act_character_creation_sturgia_shipbuilder_father";
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "mother_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_sturgia_shipbuilder_mother");
                }
                if (narrativeMenuCharacter.StringId == "father_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_sturgia_shipbuilder_father");
                    narrativeMenuCharacter.SetLeftHandItem("blacksmith_hammer");
                }
            }
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetAseraiKinsfolkNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.Throwing };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 1);
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

        private void GetAseraiFerrymanNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { NavalSkills.Boatswain, DefaultSkills.Trade };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 1);
        }

        private bool AseraiFerrymanNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }

        private void GetAseraiFerrymanNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("seafarer");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_aserai_ferryman_mother";
            string fatherAnimation = "act_character_creation_aserai_ferryman_father";
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "mother_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_aserai_ferryman_mother");
                }
                if (narrativeMenuCharacter.StringId == "father_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_aserai_ferryman_father");
                    narrativeMenuCharacter.SetRightHandItem("shovel_right_hand");
                }
            }
            foreach (NarrativeMenuCharacter narrativeMenuCharacter2 in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter2.StringId == "mother_character")
                {
                    narrativeMenuCharacter2.SetAnimationId("act_character_creation_aserai_ferryman_mother");
                }
                if (narrativeMenuCharacter2.StringId == "father_character")
                {
                    narrativeMenuCharacter2.SetAnimationId("act_character_creation_aserai_ferryman_father");
                    narrativeMenuCharacter2.SetRightHandItem("shovel_right_hand");
                }
            }
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        private void GetAseraiCorsairTradersNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { NavalSkills.Boatswain, NavalSkills.Mariner };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 1);
        }

        private bool AseraiCorsairTradersNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "aserai";
        }

        private void GetAseraiCorsairTradersNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("shipmaster_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_aserai_corsair_trader_mother";
            string fatherAnimation = "act_character_creation_aserai_corsair_trader_father";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetBattaniaRetainerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.TwoHanded, DefaultSkills.Bow };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 1);
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
        private void GetBattaniaCurrachSailorsNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { NavalSkills.Boatswain, DefaultSkills.Bow };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 1);
        }

        private bool BattaniaCurrachSailorsNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }

        private void GetBattaniaCurrachSailorsNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("seafarer");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_battania_currach_sailors_mother";
            string fatherAnimation = "act_character_creation_battania_currach_sailors_father";
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "mother_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_battania_currach_sailors_mother");
                    narrativeMenuCharacter.SetLeftHandItem("bow");
                }
                if (narrativeMenuCharacter.StringId == "father_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_battania_currach_sailors_father");
                    narrativeMenuCharacter.SetRightHandItem("battle_axe");
                }
            }
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        private void GetBattaniaGuardianOfTheLakeNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { NavalSkills.Mariner, DefaultSkills.Polearm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 1);
        }

        private bool BattaniaGuardianOfTheLakeNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "battania";
        }

        private void GetBattaniaGuardianOfTheLakeNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("shipmaster_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_battania_guardian_of_the_lake_mother";
            string fatherAnimation = "act_character_creation_battania_guardian_of_the_lake_father";
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "mother_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_battania_guardian_of_the_lake_mother");
                    narrativeMenuCharacter.SetRightHandItem("javelin_a");
                }
                if (narrativeMenuCharacter.StringId == "father_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_battania_guardian_of_the_lake_father");
                    narrativeMenuCharacter.SetLeftHandItem("heater_shield");
                    narrativeMenuCharacter.SetRightHandItem("blacksmith_sword");
                }
            }
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        public void GetKhuzaitRetainerNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Riding, DefaultSkills.Polearm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 1);
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
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 1);
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
        private void GetKhuzaitRiverForagersNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { NavalSkills.Boatswain, NavalSkills.Shipmaster };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 1);
        }

        private bool KhuzaitRiverForagersNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }

        private void GetKhuzaitRiverForagersNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("seafarer");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_khuzait_river_foragers_mother";
            string fatherAnimation = "act_character_creation_khuzait_river_foragers_father";
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "mother_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_khuzait_river_foragers_mother");
                }
                if (narrativeMenuCharacter.StringId == "father_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_khuzait_river_foragers_father");
                    narrativeMenuCharacter.SetLeftHandItem("fish_stick");
                }
            }
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        private void GetKhuzaitRiverTradersNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Bow, NavalSkills.Mariner };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 1);
        }

        private bool KhuzaitRiverTradersNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "khuzait";
        }

        private void GetKhuzaitRiverTradersNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("shipmaster_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_khuzait_river_traders_mother";
            string fatherAnimation = "act_character_creation_khuzait_river_traders_father";
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "mother_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_khuzait_river_foragers_mother");
                }
                if (narrativeMenuCharacter.StringId == "father_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_khuzait_river_foragers_father");
                    narrativeMenuCharacter.SetLeftHandItem("stick");
                }
            }
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        private void GetNordHersirNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Steward, DefaultSkills.OneHanded };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Vigor, 1);
        }

        private bool NordHersirNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "nord";
        }

        private void NordHersirNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        private void GetNordMarketTraderNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 1);
        }

        private bool NordMarketTraderNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "nord";
        }

        private void NordMarketTraderNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("merchant_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_side_to_side_2";
            string fatherAnimation = "act_character_creation_male_default_side_to_side_2";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        private void GetNordSkaldNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Scouting, DefaultSkills.Charm };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 1);
        }

        private bool NordSkaldNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "nord";
        }

        private void NordSkaldNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("bard");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_father_sitting";
            string fatherAnimation = "act_character_creation_male_default_father_sitting";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        private void GetNordBlacksmithNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Crafting, DefaultSkills.Engineering };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Intelligence, 1);
        }

        private bool NordBlacksmithNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "nord";
        }

        private void NordBlacksmithNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("artisan_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_female_default_mother_front";
            string fatherAnimation = "act_character_creation_male_default_mother_front";
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        private void GetNordHunterNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Bow, DefaultSkills.Medicine };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Control, 1);
        }

        private bool NordHunterNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "nord";
        }

        private void NordHunterNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        private void GetNordVagabondNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Throwing, DefaultSkills.Roguery };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Cunning, 1);
        }

        private bool NordVagabondNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "nord";
        }

        private void NordVagabondNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
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

        private void GetNordSailorsNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Trade, NavalSkills.Boatswain };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Social, 1);
        }

        private bool NordSailorsNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "nord";
        }

        private void GetNordSailorsNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("seafarer");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_nord_sailors_mother";
            string fatherAnimation = "act_character_creation_nord_sailors_father";
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "mother_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_nord_sailors_mother");
                    narrativeMenuCharacter.SetRightHandItem("fish_basket");
                }
                if (narrativeMenuCharacter.StringId == "father_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_nord_sailors_father");
                    narrativeMenuCharacter.SetLeftHandItem("fish_left_hand");
                }
            }
            this.UpdateParentEquipment(characterCreationManager, @object, object2, motherAnimation, fatherAnimation);
        }

        private void GetNordShipwrightsNarrativeOptionArgs(NarrativeMenuOptionArgs args)
        {
            SkillObject[] affectedSkills = new SkillObject[] { DefaultSkills.Engineering, NavalSkills.Shipmaster };
            args.SetAffectedSkills(affectedSkills);
            args.SetFocusToSkills(1);
            args.SetLevelToSkills(10);
            args.SetLevelToAttribute(DefaultCharacterAttributes.Endurance, 1);
        }

        private bool NordShipwrightsNarrativeOptionOnCondition(CharacterCreationManager characterCreationManager)
        {
            return characterCreationManager.CharacterCreationContent.SelectedCulture.StringId == "nord";
        }

        private void GetNordShipwrightsNarrativeOptionOnSelect(CharacterCreationManager characterCreationManager)
        {
            characterCreationManager.CharacterCreationContent.SetParentOccupation("shipmaster_urban");
            string motherEquipmentId = this.GetMotherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            string fatherEquipmentId = this.GetFatherEquipmentId(characterCreationManager, characterCreationManager.CharacterCreationContent.SelectedParentOccupation, characterCreationManager.CharacterCreationContent.SelectedCulture.StringId);
            MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(motherEquipmentId);
            MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(fatherEquipmentId);
            string motherAnimation = "act_character_creation_nord_shipwrights_mother";
            string fatherAnimation = "act_character_creation_nord_shipwrights_father";
            foreach (NarrativeMenuCharacter narrativeMenuCharacter in characterCreationManager.CurrentMenu.Characters)
            {
                if (narrativeMenuCharacter.StringId == "mother_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_nord_shipwrights_mother");
                }
                if (narrativeMenuCharacter.StringId == "father_character")
                {
                    narrativeMenuCharacter.SetAnimationId("act_character_creation_nord_shipwrights_father");
                    narrativeMenuCharacter.SetRightHandItem("blacksmith_hammer");
                }
            }
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
    }
}