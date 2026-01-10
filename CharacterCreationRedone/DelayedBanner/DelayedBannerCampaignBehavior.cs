using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;

namespace CharacterCreationRedone.DelayedBanner
{
    internal class DelayedBannerCampaignBehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
            CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
        }

        public override void SyncData(IDataStore dataStore)
        {
        }

        private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
        {
            if (clan == Clan.PlayerClan)
            {
                Game.Current.GameStateManager.PushState(Game.Current.GameStateManager.CreateState<BannerEditorState>(), 1);
            }
        }

        private void OnNewGameCreated(CampaignGameStarter campaignGameStarter)
        {
            Clan.PlayerClan.Banner.ClearAllIcons();
            Hero.MainHero.ClanBanner.ChangeBackgroundColor(Hero.MainHero.Culture.Color, Hero.MainHero.Culture.BackgroundColor2);
            Hero.MainHero.ClanBanner.ChangePrimaryColor(Hero.MainHero.Culture.Color);
        }
        private void OnTick()
        {
            Clan.PlayerClan.Banner.ClearAllIcons();
            Hero.MainHero.ClanBanner.ChangeBackgroundColor(Hero.MainHero.Culture.Color, Hero.MainHero.Culture.BackgroundColor2);
        }
    }
}
