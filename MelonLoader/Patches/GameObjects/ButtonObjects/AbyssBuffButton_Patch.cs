#if FIX
using HarmonyLib;
using Il2Cpp;
using Il2CppTMPro;
using PvZ_Fusion_Translator.AssetStore;
using UnityEngine;

namespace PvZ_Fusion_Translator.Patches.GameObjects.ButtonObjects
{
    [HarmonyPatch(typeof(AbyssBuffButton))]
    public static class AbyssBuffButton_Patch
    {
        [HarmonyPatch(nameof(AbyssBuffButton.Start))]
        [HarmonyPostfix]
        private static void Start(AbyssBuffButton __instance)
        {
            TMP_FontAsset fontAsset = FontStore.LoadTMPFont(Utils.Language.ToString());

            __instance.description.text = StringStore.TranslateText(__instance.description.text);
            __instance.description.font = fontAsset;
        }
    }
}
#endif


using HarmonyLib;
using Il2Cpp;
using Il2CppTMPro;
using PvZ_Fusion_Translator.AssetStore;
using UnityEngine;

namespace PvZ_Fusion_Translator.Patches.GameObjects.ButtonObjects
{
    [HarmonyPatch(typeof(AbyssBuffButton))]
    public static class AbyssBuffButton_Patch
    {
        [HarmonyPatch(nameof(AbyssBuffButton.SetType))]
        [HarmonyPostfix]
        private static void SetType_Postfix(AbyssBuffButton __instance)
        {
            TMP_FontAsset fontAsset = FontStore.LoadTMPFont(Utils.Language.ToString());
            
            // Handle translation for both normal and sold states
            if (!__instance.sold)
            {
                // Get original text components
                string buffName = StringStore.TranslateText(__instance.theBuffType.ToString());
                string description = StringStore.TranslateText(__instance.description.text.Split('\n')[0]);
                
                // Rebuild the text with translations
                __instance.description.text = $"{buffName}\n{description}";
            }
            else
            {
                __instance.description.text = StringStore.TranslateText("当前词条已出售");
            }
            
            __instance.description.font = fontAsset;
        }
    }
}
