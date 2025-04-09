using HarmonyLib;
using Il2Cpp;
using Il2CppTMPro;
using PvZ_Fusion_Translator.AssetStore;
using UnityEngine;
using System.Globalization;
using MelonLoader;

namespace PvZ_Fusion_Translator.Patches
{
	internal class Z_OtherPatches
	{
		[HarmonyPatch(typeof(PlayerName))]
		public static class PlayerName_Patch
		{
			[HarmonyPostfix]
			[HarmonyPatch(nameof(PlayerName.Update))]
			private static void Update(PlayerName __instance)
			{
				__instance.textMesh.text = StringStore.TranslateText(__instance.textMesh.text);
			}   
		}

		[HarmonyPatch(typeof(DY))]
		public static class DY_Patch
		{
			[HarmonyPostfix]
			[HarmonyPatch(nameof(DY.Update))]
			private static void Update(DY __instance)
			{
				__instance.TextMeshPro.text = StringStore.TranslateText(__instance.TextMeshPro.text);
				
			}
		}

		[HarmonyPatch(typeof(Money))]
		public static class Money_Patch
		{
			[HarmonyPostfix]
			[HarmonyPatch(nameof(Money.Update))]
			private static void Update(Money __instance)
			{
				__instance.beanCount.text = StringStore.TranslateText(__instance.beanCount.text);
				__instance.beanCount2.text = StringStore.TranslateText(__instance.beanCount2.text);
			}
		}
		#if TESTING
		[HarmonyPatch(typeof(PlantDataLoader))]
		public static class PlantDataLoaderPatches
		{
			private static bool CheckForInstalledBlackScreenFix()
			{
				foreach (MelonMod mod in MelonMod.RegisteredMelons)
				{
					if (mod.Info.Name == "BlackScreenFix")
						return true;
				}
				return false;
			}

			[HarmonyPatch(nameof(PlantDataLoader.LoadPlantData))]
			[HarmonyPrefix]
			private static bool LoadPlantData()
			{
				if (CheckForInstalledBlackScreenFix())
					return true;
				string text = Resources.Load<TextAsset>("plant_data").text;
				StringReader reader = new(text);
				bool firstLine = true;
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					if (firstLine)
					{
						firstLine = false;
						continue;
					}
					PlantDataLoader.PlantData_ data = new();
					string[] stringValuesArray = line.Split(',');
					if (IntTryParse(stringValuesArray[0], out int plantType))
						data.field_Public_PlantType_0 = (PlantType)plantType;
					if (FloatTryParse(stringValuesArray[1], out float attackInterval))
						data.field_Public_Single_0 = attackInterval;
					if (FloatTryParse(stringValuesArray[2], out float productionInterval))
						data.field_Public_Single_1 = productionInterval;
					if (IntTryParse(stringValuesArray[3], out int attackDamage))
						data.attackDamage = attackDamage;
					if (IntTryParse(stringValuesArray[4], out int maxHealth))
						data.field_Public_Int32_0 = maxHealth;
					if (FloatTryParse(stringValuesArray[5], out float cooldown))
						data.field_Public_Single_2 = cooldown;
					if (IntTryParse(stringValuesArray[6], out int price))
						data.field_Public_Int32_1 = price;
					PlantDataLoader.plantData[(int)data.field_Public_PlantType_0] = data;
				}
				return false;
			}
			private static bool IntTryParse(string s, out int result)
			{
				if (int.TryParse(s, out result))
					return true;
				return false;
			}
			private static bool FloatTryParse(string s, out float result)
			{
				if (float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
					return true;
				return false;
			}
		}
		#endif

		[HarmonyPatch(typeof(Plant))]
		public static class Plant_Patch
		{
			[HarmonyPostfix]
			[HarmonyPatch(nameof(Plant.UpdateHealthText))]
			private static void UpdateHealthText(Plant __instance)
			{
				Transform healthTransform = __instance.transform.Find("health");
				if (healthTransform != null)
				{
					StringStore.TranslateTextTransform(__instance.transform);
				}
				
			}
		}

		[HarmonyPatch(typeof(Zombie))]
		public static class Zombie_Patch
		{
			[HarmonyPostfix]
			[HarmonyPatch(nameof(Zombie.UpdateHealthText))]
			private static void UpdateHealthText(Zombie __instance)
			{
				Transform healthTransform = __instance.transform.Find("health");
				if (healthTransform != null)
				{
					StringStore.TranslateTextTransform(__instance.transform);
				}
			}
		}

		[HarmonyPatch(typeof(UIMgr))]
		public class UIMgr_Patch
		{
			[HarmonyPatch(nameof(UIMgr.EnterMainMenu))]
			[HarmonyPostfix]
			private static void EnterMainMenu(UIMgr __instance)
			{
				// Log.LogInfo("Entered Main Menu");
				#if MULTI_LANGUAGE
				WarningStore.WarningReload(Utils.Language);
				#else
				WarningStore.WarningReload();
				#endif
			}
		}

		[HarmonyPatch(typeof(MagnetBox))]
		public class MagnetBox_Patch
		{
			[HarmonyPostfix]
			[HarmonyPatch(nameof(MagnetBox.Update))]
			private static void Update(MagnetBox __instance)
			{
				StringStore.TranslateTextTransform(__instance.transform);
			}
		}

		[HarmonyPatch(typeof(MagnetInterface))]
		public class MagnetInterface_Patch
		{
			[HarmonyPostfix]
			[HarmonyPatch(nameof(MagnetInterface.Update))]
			private static void Update(MagnetInterface __instance)
			{
				StringStore.TranslateTextTransform(__instance.transform);
			}
		}

		[HarmonyPatch(typeof(ThanksMenu))]
		public class ThanksMenu_Patch
		{
			[HarmonyPostfix]
			[HarmonyPatch(nameof(ThanksMenu.Update))]
			private static void Update(ThanksMenu __instance)
			{
				TextMeshProUGUI textTransform = __instance.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
				if (textTransform != null)
				{
					textTransform.text = StringStore.TranslateText(textTransform.text);
					textTransform.autoSizeTextContainer = true;
				}
			}
		}

		[HarmonyPatch(typeof(LanternUmbrella))]
		public class LanternUmbrella_Patch
		{
			[HarmonyPostfix]
			[HarmonyPatch(nameof(LanternUmbrella.Update))]
			private static void Update(LanternUmbrella __instance)
			{
				__instance.energyText = StringStore.TranslateText(__instance.energyText);
				__instance.energyTextShadow = StringStore.TranslateText(__instance.energyTextShadow);
			}
		}
	}
}
