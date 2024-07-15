using HarmonyLib;
using SOMD.ModLogic;
using TabletopTweaks.Core.Utilities;
using UnityModManagerNet;

namespace SOMD
{
    static class Main
    {
        public static bool Enabled;
        public static ModContextSOMD SOMDContext;
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            SOMDContext = new ModContextSOMD(modEntry);
            SOMDContext.ModEntry.OnSaveGUI = OnSaveGUI;
            SOMDContext.ModEntry.OnGUI = UMMSettingsUI.OnGUI;
            harmony.PatchAll();
            PostPatchInitializer.Initialize(SOMDContext);
            return true;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            SOMDContext.SaveAllSettings();
        }
    }
}