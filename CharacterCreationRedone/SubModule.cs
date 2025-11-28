using HarmonyLib;
using TaleWorlds.MountAndBlade;

namespace CharacterCreationRedone
{
    public class Submodule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            new Harmony("CharacterCreationRedone.CharacterCreationOptions").PatchAll();
        }
    }
}
