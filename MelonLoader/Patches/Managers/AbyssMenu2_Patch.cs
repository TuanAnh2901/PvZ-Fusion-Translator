using HarmonyLib;
using Il2Cpp;
using Il2CppTMPro;
using PvZ_Fusion_Translator.AssetStore;
using UnityEngine;

namespace PvZ_Fusion_Translator.Patches.Managers
{
	[HarmonyPatch(typeof(AbyssMenu2))]
    public static class AbyssMenu2_Patch
    {
        [HarmonyPatch(nameof(AbyssMenu2.Start))]
        [HarmonyPostfix]
        private static void Start(AbyssMenu2 __instance)
        {
            TMP_FontAsset fontAsset = FontStore.LoadTMPFont(Utils.Language.ToString());

            // BuffCountText
            string originalBuffCountText = __instance.buffCountText.text;
            __instance.buffCountText.text = StringStore.TranslateText(originalBuffCountText);
            __instance.buffCountText.font = fontAsset;
            Debug.Log($"Translated BuffCountText: '{originalBuffCountText}' -> '{__instance.buffCountText.text}'");

            // Money
            string originalMoneyText = __instance.money.text;
            __instance.money.text = StringStore.TranslateText(originalMoneyText);
            __instance.money.font = fontAsset;
            Debug.Log($"Translated Money: '{originalMoneyText}' -> '{__instance.money.text}'");

            // RefreshText
            string originalRefreshText = __instance.refreshText.text;
            __instance.refreshText.text = StringStore.TranslateText(originalRefreshText);
            __instance.refreshText.font = fontAsset;
            Debug.Log($"Translated RefreshText: '{originalRefreshText}' -> '{__instance.refreshText.text}'");

            // Statistics
            string originalStatisticsText = __instance.statistics.text;
            __instance.statistics.text = StringStore.TranslateText(originalStatisticsText);
            __instance.statistics.font = fontAsset;
            Debug.Log($"Translated Statistics: '{originalStatisticsText}' -> '{__instance.statistics.text}'");

            // Transform BuffText
            Transform buffTextTransform = __instance.transform.FindChild("BuffText").transform;
            if (buffTextTransform != null)
            {
                StringStore.TranslateTextTransform(buffTextTransform);
                Debug.Log($"Translated BuffText Transform: '{buffTextTransform.name}'");
            }

            // Transform Money
            Transform moneyTransform = __instance.transform.FindChild("Money").transform;
            if (moneyTransform != null)
            {
                StringStore.TranslateTextTransform(moneyTransform);
                Debug.Log($"Translated Money Transform: '{moneyTransform.name}'");
            }

            // Transform BuffCountText
            Transform buffCountTextTransform = __instance.transform.FindChild("BuffCountText").transform;
            if (buffCountTextTransform != null)
            {
                StringStore.TranslateTextTransform(buffCountTextTransform);
                Debug.Log($"Translated BuffCountText Transform: '{buffCountTextTransform.name}'");
            }

            // Transform RefreshText
            Transform refreshTextTransform = __instance.transform.FindChild("RefreshText").transform;
            if (refreshTextTransform != null)
            {
                StringStore.TranslateTextTransform(refreshTextTransform);
                Debug.Log($"Translated RefreshText Transform: '{refreshTextTransform.name}'");
            }
        }

        [HarmonyPatch(nameof(AbyssMenu2.Awake))]
        [HarmonyPostfix]
        private static void Awake(AbyssMenu2 __instance)
        {
            TMP_FontAsset fontAsset = FontStore.LoadTMPFont(Utils.Language.ToString());

            // BuffCountText
            string originalBuffCountText = __instance.buffCountText.text;
            __instance.buffCountText.text = StringStore.TranslateText(originalBuffCountText);
            Debug.Log($"Translated BuffCountText: '{originalBuffCountText}' -> '{__instance.buffCountText.text}'");
            __instance.buffCountText.font = fontAsset;

            // Money
            string originalMoneyText = __instance.money.text;
            __instance.money.text = StringStore.TranslateText(originalMoneyText);
            Debug.Log($"Translated Money: '{originalMoneyText}' -> '{__instance.money.text}'");
            __instance.money.font = fontAsset;

            // RefreshText
            string originalRefreshText = __instance.refreshText.text;
            __instance.refreshText.text = StringStore.TranslateText(originalRefreshText);
            Debug.Log($"Translated RefreshText: '{originalRefreshText}' -> '{__instance.refreshText.text}'");
            __instance.refreshText.font = fontAsset;

            // Statistics
            string originalStatisticsText = __instance.statistics.text;
            __instance.statistics.text = StringStore.TranslateText(originalStatisticsText);
            Debug.Log($"Translated Statistics: '{originalStatisticsText}' -> '{__instance.statistics.text}'");
            __instance.statistics.font = fontAsset;

            // Transform BuffText
            Transform buffTextTransform = __instance.transform.FindChild("BuffText").transform;
            if (buffTextTransform != null)
            {
                StringStore.TranslateTextTransform(buffTextTransform);
                Debug.Log($"Translated BuffText Transform: '{buffTextTransform.name}'");
            }

            // Transform Money
            Transform moneyTransform = __instance.transform.FindChild("Money").transform;
            if (moneyTransform != null)
            {
                StringStore.TranslateTextTransform(moneyTransform);
                Debug.Log($"Translated Money Transform: '{moneyTransform.name}'");
            }

            // Transform BuffCountText
            Transform buffCountTextTransform = __instance.transform.FindChild("BuffCountText").transform;
            if (buffCountTextTransform != null)
            {
                StringStore.TranslateTextTransform(buffCountTextTransform);
                Debug.Log($"Translated BuffCountText Transform: '{buffCountTextTransform.name}'");
            }

            // Transform RefreshText
            Transform refreshTextTransform = __instance.transform.FindChild("RefreshText").transform;
            if (refreshTextTransform != null)
            {
                StringStore.TranslateTextTransform(refreshTextTransform);
                Debug.Log($"Translated RefreshText Transform: '{refreshTextTransform.name}'");
            }
        }
    }
}
