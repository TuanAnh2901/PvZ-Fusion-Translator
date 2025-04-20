using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Common.Attributes;
using Il2CppTMPro;
using PvZ_Fusion_Translator.AssetStore;
using UnityEngine;

namespace PvZ_Fusion_Translator.Patches.GameObjects
{
    [HarmonyPatch(typeof(TravelRefresh))]
	public static class TravelRefresh_Patch
	{
		[HarmonyPatch(nameof(TravelRefresh.Start))]
		[HarmonyPostfix]
		private static void Start(TravelRefresh __instance)
		{
#if FIX
            //TextMeshProUGUI pointsText = __instance.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            //pointsText = StringStore.TranslateText(pointsText);
            //pointsText.autoSizeTextContainer = true;


            //Transform TravelMenuTransform = __instance.transform.parent.transform;

            //Transform refreshTransform = TravelMenuTransform.Find("Refresh2").transform.Find("bank");
            //if (refreshTransform != null)
            //{
            //	Transform text_1Transform = refreshTransform.Find("text_1");
            //	if (text_1Transform != null)
            //	{
            //		TextMeshProUGUI text_1 = text_1Transform.GetComponent<TextMeshProUGUI>();

            //		if (text_1)
            //		{
            //			text_1 = StringStore.TranslateText(text_1);
            //			text_1.autoSizeTextContainer = true;
            //		}
            //	}

            //	Transform text_2Transform = refreshTransform.Find("text_2");
            //	if (text_2Transform != null)
            //	{
            //		TextMeshProUGUI text_2 = text_2Transform.GetComponent<TextMeshProUGUI>();

            //		if (text_2)
            //		{
            //                     text_2 = StringStore.TranslateText(text_2);
            //                     text_2.autoSizeTextContainer = true;
            //		}
            //	}
            //}
#endif
            TMP_FontAsset fontAsset = FontStore.LoadTMPFont(Utils.Language.ToString());
            string originalText = __instance.text.text;
            string originalTextshadow = __instance.text_shadow.text;
            __instance.text.text = StringStore.TranslateText(originalText, false);
            __instance.text_shadow.text = StringStore.TranslateText(originalTextshadow, false);
            __instance.text.font = fontAsset;
            __instance.text_shadow.font = fontAsset;
        }
	}
}
