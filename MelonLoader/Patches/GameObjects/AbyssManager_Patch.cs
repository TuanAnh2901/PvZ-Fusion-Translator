using HarmonyLib;
using Il2Cpp;
using Il2CppTMPro;
using PvZ_Fusion_Translator.AssetStore;
using System.Collections.Generic;
using System.Reflection;

namespace PvZ_Fusion_Translator.Patches.Managers
{
    [HarmonyPatch(typeof(AbyssManager))]
    public static class AbyssManager_Patch
    {
        [HarmonyPatch(nameof(AbyssManager.UpdateStatistics))]
        [HarmonyPostfix]
        private static void UpdateStatistics(AbyssManager __instance, TextMeshProUGUI statistics)
        {
            // Get the current font asset for the selected language
            TMP_FontAsset fontAsset = FontStore.LoadTMPFont(Utils.Language.ToString());
            
            // Translate the text and set the font
            statistics.text = StringStore.TranslateText(statistics.text, false);
            statistics.font = fontAsset;
        }

        [HarmonyPatch(typeof(AbyssManager), MethodType.Constructor, new Type[] { })]
        [HarmonyPatch(MethodType.StaticConstructor)]
        [HarmonyPostfix]
        private static void StaticConstructorPostfix()
        {
            // Get the abyssBuffDescription dictionary field using reflection
            FieldInfo fieldInfo = typeof(AbyssManager).GetField("abyssBuffDescription", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (fieldInfo == null)
                return;

            // Get the current dictionary
            var dictionary = fieldInfo.GetValue(null) as Dictionary<AbyssBuff, ValueTuple<string, int>>;
            if (dictionary == null)
                return;

            // Create a new dictionary to hold the translated values
            var translatedDictionary = new Dictionary<AbyssBuff, ValueTuple<string, int>>();

            // Translate each string in the dictionary
            foreach (var kvp in dictionary)
            {
                string originalText = kvp.Value.Item1;
                string translatedText = StringStore.TranslateText(originalText, false);
                translatedDictionary[kvp.Key] = new ValueTuple<string, int>(translatedText, kvp.Value.Item2);
            }

            // Set the translated dictionary back to the field
            fieldInfo.SetValue(null, translatedDictionary);
        }
    }
}
