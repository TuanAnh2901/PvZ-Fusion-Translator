#if CassidyCode
using HarmonyLib;
using Il2Cpp;
using Il2CppTMPro;
using MelonLoader;
using PvZ_Fusion_Translator.AssetStore;
using Unity.VisualScripting;
using UnityEngine;
using System.Text.RegularExpressions;

namespace PvZ_Fusion_Translator.Patches.GameObjects
{
    [HarmonyPatch(typeof(AbyssMenu))]
    public static class AbyssMenu_Patch
    {

        [HarmonyPatch(nameof(AbyssMenu.Awake))]
        [HarmonyPostfix]
        private static void Awake(AbyssMenu __instance)
        {
            TMP_FontAsset fontAsset = FontStore.LoadTMPFont(Utils.Language.ToString());

            foreach (TextMeshProUGUI txt in __instance.levelInfo)
            {
                txt.text = StringStore.TranslateText(txt.text, false);
                txt.font = fontAsset;
            }
        }

        [HarmonyPatch(nameof(AbyssMenu.UpdateLevelInfo))]
        [HarmonyPostfix]
        private static void UpdateLevelInfo(AbyssMenu __instance)
        {
            TMP_FontAsset fontAsset = FontStore.LoadTMPFont(Utils.Language.ToString());

            foreach (TextMeshProUGUI txt in __instance.levelInfo)
            {
                string regexStr = "第(\\d+)关\n场景：([^\\s]+)\n总波数：(\\d+)波\n特殊环境：([^\\s：]+)\n减伤强度：(\\d+)";

                Regex regex = new Regex(regexStr);

                if(regex.IsMatch(txt.text))
                {   
                    Match match = regex.Match(txt.text);
                    int groupCount = match.Groups.Count;

                    List<string> dynamicParts = [];

                    for (int i = 1; i < groupCount; i++)
                    {
                        string groupValue = match.Groups[i].Value;
                        string translatedValue = StringStore.translationString.ContainsKey(groupValue) ? StringStore.translationString[groupValue] : groupValue;
                        dynamicParts.Add(translatedValue);
                    }

                    txt.text = string.Format(StringStore.translationStringRegex[regexStr], [.. dynamicParts]);
                    txt.font = fontAsset;
                }
            }
        }
    }
}
#endif

//#if VTACode
using HarmonyLib;
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
    [HarmonyPatch(typeof(AbyssMenu))]
    public static class AbyssMenu_Patch
    {
        //     TODO: Old code
        //     [HarmonyPatch(nameof(AbyssMenu.Awake))]
        //     [HarmonyPostfix]
        //     private static void Awake(AbyssMenu __instance)
        //     {
        //         TMP_FontAsset fontAsset = FontStore.LoadTMPFont(Utils.Language.ToString());

                 //foreach (TextMeshProUGUI txt in __instance.levelInfo)
        //         {
        //             txt.text = StringStore.TranslateText(txt.text, false);
        //             txt.font = fontAsset;
        //         }
        //     }
        // }



        // All of the code below is genetated by Copilot and Augment AI(base on Claude 3.7 Sonnet)

        // Dictionary to store regex patterns for different languages
        private static readonly Dictionary<string, Regex> languagePatterns = new Dictionary<string, Regex>();

        // Dictionary to store level info templates for different languages
        private static readonly Dictionary<string, string> levelInfoTemplates = new Dictionary<string, string>();

        static AbyssMenu_Patch()
        {
            // Initialize patterns
            InitializePatterns();

            // Initialize templates
            InitializeTemplates();
        }

        private static void InitializePatterns()
        {
            // Default Vietnamese pattern (used if no patterns are found in translation files)
            languagePatterns["Vietnamese"] = new Regex(
                @"Màn\s*(\d+)\s*Cảnh::\s*(.*?)\s*Số đợt:\s*(\d+)\s*Đặc điểm màn chơi:\s*(.*?)\s*Mức độ giảm sât thương:\s*(.*?)(?:\n|$)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline
            );

            // Default Chinese pattern
            languagePatterns["Chinese"] = new Regex(
                @"第(\d+)关\n场景：(.*?)\n总波数：(\d+)波\n特殊环境：(.*?)(?:\n减伤强度：(.*?))?$",
                RegexOptions.Singleline
            );

            // English pattern (example)
            languagePatterns["English"] = new Regex(
                @"Level\s*(\d+)\s*Scene:\s*(.*?)\s*Total waves:\s*(\d+)\s*Special environment:\s*(.*?)\s*Damage reduction:\s*(.*?)(?:\n|$)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline
            );

            // Load from translation_regexs
            foreach (var kvp in StringStore.translationStringRegex)
            {
                if (kvp.Key.EndsWith("_LevelPattern"))
                {
                    string language = kvp.Key.Replace("_LevelPattern", "");
                    if (!string.IsNullOrEmpty(language) && !string.IsNullOrEmpty(kvp.Value))
                    {
                        try
                        {
                            languagePatterns[language] = new Regex(kvp.Value, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                            //Log.LogInfo($"Loaded regex pattern for {language}: {kvp.Value}");
                        }
                        catch (System.Exception ex)
                        {
                            Log.LogError($"Failed to load regex pattern for {language}: {ex.Message}");
                        }
                    }
                }
            }
        }

        private static void InitializeTemplates()
        {
            // Default templates
            levelInfoTemplates["Vietnamese"] = "<b>Màn {0}</b>\nCảnh: {1}\nSố đợt: {2}\nĐặc điểm màn chơi: {3}\nMức độ giảm sát thương: {4}";
            levelInfoTemplates["Chinese"] = "<b>第{0}关</b>\n场景: {1}\n总波数: {2}波\n特殊环境: {3}\n减伤强度: {4}";
            levelInfoTemplates["English"] = "<b>Level {0}</b>\nScene: {1}\nTotal waves: {2}\nSpecial environment: {3}\nDamage reduction: {4}";

            // Load from translation_strings
            foreach (var kvp in StringStore.translationString)
            {
                if (kvp.Key.EndsWith("_LevelInfoTemplate"))
                {
                    string language = kvp.Key.Replace("_LevelInfoTemplate", "");
                    if (!string.IsNullOrEmpty(language) && !string.IsNullOrEmpty(kvp.Value))
                    {
                        levelInfoTemplates[language] = kvp.Value;
                        //Log.LogInfo($"Loaded template for {language}: {kvp.Value}");
                    }
                }
            }
        }

        private static Regex GetRegexForCurrentLanguage()
        {
            string currentLanguage = Utils.Language.ToString();
            if (languagePatterns.ContainsKey(currentLanguage))
            {
                return languagePatterns[currentLanguage];
            }

            // Fallback to Chinese pattern if current language is not supported
            return languagePatterns["Chinese"];
        }

        private static string GetTemplateForCurrentLanguage()
        {
            string currentLanguage = Utils.Language.ToString();
            if (levelInfoTemplates.ContainsKey(currentLanguage))
            {
                return levelInfoTemplates[currentLanguage];
            }

            // Fallback to Chinese template if current language is not supported
            return levelInfoTemplates["Chinese"];
        }

        [HarmonyPatch(nameof(AbyssMenu.Awake))]
        [HarmonyPostfix]
        private static void Awake(AbyssMenu __instance)
        {
            TMP_FontAsset fontAsset = FontStore.LoadTMPFont(Utils.Language.ToString());
            string currentLanguage = Utils.Language.ToString();

            foreach (TextMeshProUGUI txt in __instance.levelInfo)
            {
                string originalText = txt.text;

                // Try to match with the current language pattern
                Match match = GetRegexForCurrentLanguage().Match(originalText);

                // If not successful, try with Chinese pattern (assuming Chinese is the base language)
                if (!match.Success)
                {
                    match = languagePatterns["Chinese"].Match(originalText);
                }

                if (match.Success)
                {
                    string template = GetTemplateForCurrentLanguage();
                    string translatedText = string.Format(
                        template,
                        match.Groups[1].Value,
                        StringStore.TranslateText(match.Groups[2].Value.Trim(), false),
                        match.Groups[3].Value,
                        StringStore.TranslateText(match.Groups[4].Value.Trim(), false),
                        match.Groups.Count > 5 && !string.IsNullOrEmpty(match.Groups[5].Value) ? match.Groups[5].Value : "0%"
                    );

                    // Apply translation and font
                    txt.text = translatedText;
                    txt.font = fontAsset;

                    // Debug log
                    //Log.LogInfo($"Original ({currentLanguage}): {originalText.Replace("\n", "\\n")}");
                    //Log.LogInfo($"Translated ({currentLanguage}): {translatedText.Replace("\n", "\\n")}");
                }
                else
                {
                    // Default handling if no pattern matches
                    txt.text = StringStore.TranslateText(originalText, false);
                    txt.font = fontAsset;
                }
            }
        }

        [HarmonyPatch(nameof(AbyssMenu.UpdateLevelInfo))]
        [HarmonyPostfix]
        private static void UpdateLevelInfo(AbyssMenu __instance)
        {
            // Call Awake again to ensure synchronization
            Awake(__instance);
        }


    }
}
//#endif