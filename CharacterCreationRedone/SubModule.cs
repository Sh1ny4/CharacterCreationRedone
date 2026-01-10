using CharacterCreationRedone.DelayedBanner;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace CharacterCreationRedone
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            new Harmony("CharacterCreationRedone.CharacterCreationOptions").PatchAll();
        }

        protected override void InitializeGameStarter(Game game, IGameStarter gameStarterObject)
        {
            if (game.GameType is Campaign)
            {
                CampaignGameStarter campaignGameStarter = gameStarterObject as CampaignGameStarter;
                campaignGameStarter.AddBehavior(new DelayedBannerCampaignBehavior());
                return;
            }
        }
    }
}
