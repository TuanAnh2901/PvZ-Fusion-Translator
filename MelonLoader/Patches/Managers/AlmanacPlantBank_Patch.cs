using HarmonyLib;
using Il2Cpp;
using Il2CppTMPro;
using PvZ_Fusion_Translator.AssetStore;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static PvZ_Fusion_Translator.FileLoader;

namespace PvZ_Fusion_Translator.Patches.Managers
{
	[HarmonyPatch(typeof(AlmanacPlantBank))]
	public static partial class AlmanacPlantBank_Patch
	{
		[HarmonyPatch(nameof(AlmanacPlantBank.InitNameAndInfoFromJson))]
		[HarmonyPrefix]
		private static bool InitNameAndInfoFromJson(Il2Cpp.AlmanacPlantBank __instance)
		{
			#if MULTI_LANGUAGE
			string currentLanguage = Utils.Language.ToString();
			string almanacDir = GetAssetDir(AssetType.Almanac, Utils.Language);
			#else
			string almanacDir = GetAssetDir(AssetType.Almanac);
			string currentLanguage = "English";
			#endif
			string path = Path.Combine(almanacDir, "LawnStringsTranslate.json");

			if (!File.Exists(path))
			{
				Log.LogError($"LawnStringsTranslate.json file not found at path: {path}");
				return true;
			}

			#if OBFUSCATE
			if (CheckSumStore.IsModified(path))
			{
				Log.LogError("File {0} was modified!", path);
				return true;
			}
			#endif

			string json;
			json = File.ReadAllText(path);
			
			bool hasAlmanacFont = false;
			TMP_FontAsset almanacFontAsset = null;
			if (FontStore.fontAssetDictSecondary.ContainsKey(currentLanguage + "_Almanac") || FontStore.fontAssetDictSecondary.ContainsKey(currentLanguage))
			{
				almanacFontAsset = FontStore.LoadTMPFontAlmanac(currentLanguage);
				hasAlmanacFont = true;
			}

			#if MULTI_LANGUAGE
			TMP_FontAsset fontAsset = FontStore.LoadTMPFont(currentLanguage);
			#else
			TMP_FontAsset fontAsset = FontStore.LoadTMPFont();
			#endif

			TextMeshPro component = __instance.introduce.GetComponent<TextMeshPro>();
			TextMeshPro component2 = __instance.plantName.GetComponent<TextMeshPro>();
			TextMeshPro component3 = __instance.plantName.transform.GetChild(0).GetComponent<TextMeshPro>();
			TextMeshPro component4 = __instance.cost.GetComponent<TextMeshPro>();

            Il2Cpp.AlmanacPlantBank.PlantData plantData = JsonUtility.FromJson<Il2Cpp.AlmanacPlantBank.PlantData>(json);

            foreach (Il2Cpp.AlmanacPlantBank.PlantInfo plantInfo in plantData.plants)
            {
                if (plantInfo.seedType == __instance.theSeedType)
                {
                    component.text = plantInfo.cost + "\n\n" + plantInfo.info + "\n\n" + plantInfo.introduce;
                    component.overflowMode = TextOverflowModes.Page;
                    component2.text = plantInfo.name;
                    component2.autoSizeTextContainer = true;
                    component3.text = Utils.RemoveColorTags(plantInfo.name);
                    component3.autoSizeTextContainer = true;
                    //component4.text = plantInfo.cost; //TODO: This make a showing bug
                    //component4.autoSizeTextContainer = true;

                    if (hasAlmanacFont)
                    {
                        component.font = almanacFontAsset;
                        component4.font = almanacFontAsset; //TODO: may mannually set the font for cost
                    }
                    else
                    {
                        component.font = fontAsset;
                        component4.font = fontAsset;
                    }
                    component2.font = fontAsset;
                    component3.font = fontAsset;
                    return false;
                }
            }

            return true;
		}

		[HarmonyPatch(nameof(AlmanacPlantBank_Patch.OnMouseDown))]
		[HarmonyPrefix]
		private static bool OnMouseDown(Il2Cpp.AlmanacPlantBank __instance)
		{
			TextMeshPro component = __instance.introduce.GetComponent<TextMeshPro>();
			if (component != null)
			{
				component.pageToDisplay = component.pageToDisplay > component.m_pageNumber ? 1 : component.pageToDisplay + 1;
				return false;
			}
			return true;
		}

		[HarmonyPatch(typeof(AlmanacPlantBank), "Start")]
		public class AlmanacPlantBankPatch
		{
			static void Postfix(AlmanacPlantBank __instance)
			{
				if (__instance.skinButton != null)
				{
					// Load custom font for localization
					TMP_FontAsset fontAsset = FontStore.LoadTMPFont(Utils.Language.ToString());

					// Locate all TextMeshProUGUI components in skinButton
					TextMeshProUGUI[] textComponents = __instance.skinButton.GetComponentsInChildren<TextMeshProUGUI>(true);

					foreach (TextMeshProUGUI txt in textComponents)
					{
						if (txt.text.Contains("换肤")) // Check for the specific text
						{
							// Translate and replace the text
							txt.text = StringStore.TranslateText(txt.text, false);
							txt.font = fontAsset; // Set the new font
						}
					}
				}

				else
				{
					Debug.LogWarning("SkinButton is null!");
				}
			}
		}

	}
}