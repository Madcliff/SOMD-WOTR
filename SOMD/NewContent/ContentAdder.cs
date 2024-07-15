using HarmonyLib;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.JsonSystem;
using TabletopTweaks.Core.Utilities;
using static SOMD.Main;

namespace SOMD.NewContent
{
    class ContentAdder
    {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch
        {
            static bool Initialized;

            [HarmonyPriority(Priority.First)]
            [HarmonyPostfix]
            static void CreateNewBlueprints()
            {
                if (Initialized) return;
                Initialized = true;
                SOMD.SecretsofMagicalDiscipline.addSecretsofMagicalDiscipline();
            }
            [HarmonyPriority(Priority.Last)]
            [HarmonyPostfix]
            static void ApplyNewMetamagics()
            {
            }
            [HarmonyPriority(Priority.Last)]
            [HarmonyPostfix]
            static void ApplyLateSelections()
            {
            }
        }
    }
}
