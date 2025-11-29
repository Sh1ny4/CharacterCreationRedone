using HarmonyLib;
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
    }
}
