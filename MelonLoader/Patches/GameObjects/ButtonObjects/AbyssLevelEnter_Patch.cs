﻿using HarmonyLib;
using Il2Cpp;
using Il2CppTMPro;
using PvZ_Fusion_Translator.AssetStore;
using static PvZ_Fusion_Translator.FileLoader;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;

namespace PvZ_Fusion_Translator.Patches.Managers
{
    [HarmonyPatch(typeof(AbyssLevelEnter))]
    public static class AbyssLevelEnter_Patch
    {

        //TODO: WTF is this? And it even make the changing language disappeared?

        //[HarmonyPatch(nameof(AbyssLevelEnter.Start))]
        //[HarmonyPostfix]
        //private static void Start(AbyssLevelEnter __instance)
        //{
        //    TMP_FontAsset fontAsset = FontStore.LoadTMPFont(Utils.Language.ToString());

        //    if (__instance.levelName != null)
        //    {
        //        string originalLevelNameText = __instance.levelName.text;
        //        __instance.levelName.text = StringStore.TranslateText(originalLevelNameText, false);
        //        Log.LogInfo($"Translated LevelNameText: '{originalLevelNameText}' -> '{__instance.levelName.text}'");
        //        __instance.levelName.font = fontAsset;
        //    }
        //} 


        [HarmonyPatch(nameof(AbyssLevelEnter.SetLevel))]
        [HarmonyPrefix]
        private static void SetLevelPrefix(AbyssLevelEnter __instance, int level, AbyssMenu abyssMenu)
        {
            // 拦截原始的格式化字符串，替换为翻译后的版本
            string originalFormat = "第{0}关";
            string translatedFormat = StringStore.TranslateText(originalFormat, false);
            
            // 在这里不设置文本，因为原始方法会设置，我们只需在Postfix中处理字体
        }

        [HarmonyPatch(nameof(AbyssLevelEnter.SetLevel))]
        [HarmonyPostfix]
        private static void SetLevel(AbyssLevelEnter __instance)
        {
            TMP_FontAsset fontAsset = FontStore.LoadTMPFont(Utils.Language.ToString());
            string originalLevelNameText = __instance.levelName.text;
            __instance.levelName.text = StringStore.TranslateText(originalLevelNameText, false);
            __instance.levelName.font = fontAsset;
        }
    }
}