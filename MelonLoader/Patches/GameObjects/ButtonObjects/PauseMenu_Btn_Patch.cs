using HarmonyLib;
using Il2Cpp;
using PvZ_Fusion_Translator.AssetStore;
using UnityEngine;

namespace PvZ_Fusion_Translator.Patches.GameObjects.ButtonObjects
{
    [HarmonyPatch(typeof(PauseMenu_Btn))]
	public static class PauseMenu_Btn_Patch
	{
		[HarmonyPatch(nameof(PauseMenu_Btn.Start))]
		[HarmonyPostfix]
		private static void Start(PauseMenu_Btn __instance) => StringStore.TranslateTextTransform(__instance.transform);


		[HarmonyPatch(nameof(PauseMenu_Btn.OnMouseUp))]
		[HarmonyPostfix]
		private static void OnMouseUp(PauseMenu_Btn __instance)
		{
			if (__instance.buttonNumber == 10)
			{
				try
				{
					GameObject warningMessage = GameObject.Find("WarningMessage");
					if (warningMessage != null)
					{
						//Log.LogInfo("Destroying warning message");
						//UnityEngine.Object.Destroy(warningMessage);

						GameAPP.theGameStatus = 0;
					}
				}
				catch (System.Exception ex)
				{
					Log.LogError($"Error handling warning message OK button: {ex.Message}");
					Log.LogError($"Stack trace: {ex.StackTrace}");
				}
			}
		}
	}
}
