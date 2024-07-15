using SOMD.Config;
using TabletopTweaks.Core.ModLogic;
using static UnityModManagerNet.UnityModManager;

namespace SOMD.ModLogic
{
    internal class ModContextSOMD : ModContextBase
    {
        public AddedContent AddedContent;

        public ModContextSOMD(ModEntry ModEntry) : base(ModEntry)
        {
            //LoadAllSettings();
        }
        public override void LoadAllSettings()
        {
            LoadBlueprints("SOMD.Config", this);
            LoadLocalization("SOMD.Localization");
        }
        public override void AfterBlueprintCachePatches()
        {
            base.AfterBlueprintCachePatches();
            if (Debug)
            {
                ModLocalizationPack.RemoveUnused();
                SaveLocalization(ModLocalizationPack);
            }
        }
    }
}
