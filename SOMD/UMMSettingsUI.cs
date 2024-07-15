using SOMD;
using TabletopTweaks.Core.UMMTools;
using UnityModManagerNet;

namespace SOMD
{
    internal static class UMMSettingsUI
    {
        private static int selectedTab;
        public static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            UI.AutoWidth();
            UI.TabBar(ref selectedTab,
                    () => UI.Label("SETTINGS WILL NOT BE UPDATED UNTIL YOU RESTART YOUR GAME.".yellow().bold()),
                    new NamedAction("Added Content", () => SettingsTabs.AddedContent())
            );
        }
    }

    internal static class SettingsTabs
    {

        public static void AddedContent()
        {
            var TabLevel = SetttingUI.TabLevel.Zero;
           
        }
    }
}
