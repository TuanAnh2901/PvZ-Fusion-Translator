#if MULTI_LANGUAGE
using HarmonyLib;
using Il2Cpp;
using Il2CppTMPro;
using PvZ_Fusion_Translator;
using PvZ_Fusion_Translator.AssetStore;
using UnityEngine;
using Object = UnityEngine.Object;

public class OptionButtonData
{
	public OptionBtn Button { get; set; }
	public Vector3 Position { get; set; }
	public Utils.LanguageEnum Language { get; set; }
}

namespace PvZ_Fusion_Translator.Patches
{
	internal class OptLanguageBtn_Patch
	{
		public static Dictionary<int, OptionButtonData> LanguageBtnDict = [];
		private static bool buttonsCreated = false; // Prevent duplicate creation
		private static OptionBtn cachedTemplateButton;

		public static void CreateLanguageButtons(OptionBtn templateButton)
		{
			if (buttonsCreated) return; // Avoid duplicate calls
			buttonsCreated = true;
			cachedTemplateButton = templateButton; // Cache for re-creation

			float startX = 4.3241f; // Starting X position
			float startY = 2.7769f; // Starting Y position
			float xSpacing = 2.56f; // Horizontal spacing between columns
			float ySpacing = 1.2505f; // Vertical spacing between rows
			int buttonsPerColumn = 6; // Number of buttons per column
			int buttonType = 80; // Starting optionType for buttons

			int i = 0; // Index for languages

			foreach (Utils.LanguageEnum language in Enum.GetValues(typeof(Utils.LanguageEnum)))
			{
				if (language == Utils.LanguageEnum.LANG_END) continue; // Skip invalid language

				// Clone the button
				OptionBtn newButton = Object.Instantiate(templateButton, templateButton.transform.parent);
				newButton.optionType = buttonType++; // Assign a unique optionType

				// Calculate column and row
				int column = i / buttonsPerColumn;
				int row = i % buttonsPerColumn;

				// Position the button
				float xPos = startX + column * xSpacing; // Calculate column position
				float yPos = startY - row * ySpacing; // Calculate row position
				newButton.transform.position = new Vector3(xPos, yPos);

				LanguageBtnDict[newButton.GetInstanceID()] = new OptionButtonData
				{
					Button = newButton, 
					Position = newButton.transform.position,
					Language = language,
				};

				// Update button text
				UpdateButtonText(newButton, language.ToString());

				i++; // Increment index
			}
		}

		public static void UpdateButtonText(OptionBtn button, string languageName)
		{
			TMP_FontAsset defaultAsset = FontStore.LoadTMPFont("English");

			// Update text in all child transforms
			for (int i = 0; i < 3; i++) // Assuming 3 child text objects
			{
				Transform textTransform = button.transform.GetChild(i);
				if (textTransform != null)
				{

					if (i == 0) textTransform.gameObject.SetActive(false);

					TextMeshProUGUI text = textTransform.GetComponent<TextMeshProUGUI>();
					if (text != null)
					{
						if (i == 0) 
						{
							text.text = "Language Changed!";
							text.color = Color.red;
							
						}
						else 
						{
							text.text = languageName; // Update language name
						}
						text.fontSize = 16;
						text.font = defaultAsset;
						text.autoSizeTextContainer = false;
					}
				}
			}
		}

		[HarmonyPatch(typeof(OptionBtn))]
		public static class OptLangBtn_Patch
		{
			[HarmonyPatch(nameof(OptionBtn.Awake))]
			[HarmonyPostfix]
			private static void Awake(OptionBtn __instance)
			{
				// Reset flag when UI is reinitialized
				if (!buttonsCreated || cachedTemplateButton == null)
				{
					buttonsCreated = false; // Reset to allow button recreation
					CreateLanguageButtons(__instance);
				}
			}

			[HarmonyPatch("OnMouseUpAsButton")]
			[HarmonyPrefix]
			private static void OnMouseUpAsButton(OptionBtn __instance)
			{
				if (LanguageBtnDict.TryGetValue(__instance.GetInstanceID(), out var languageButton))
				{
					Utils.ChangeLanguage(languageButton.Language.ToString()); // Change language
					Debug.Log($"Language changed to: {languageButton.Language.ToString()}");

					var child = languageButton.Button.transform.GetChild(0);
					if (child != null)
					{
						child.gameObject.SetActive(true);

						// Use a delayed action with a safer check
						Task.Delay(1000).ContinueWith(task =>
						{
							// Ensure the button is still valid before attempting to deactivate the child
							if (languageButton.Button != null && child != null && child.gameObject.activeSelf)
							{
								child.gameObject.SetActive(false);
							}
						});
					}
				}
			}

			[HarmonyPatch(nameof(OptionBtn.Update))]
			[HarmonyPostfix]
			private static void Update(OptionBtn __instance)
			{
				// Ensure buttons maintain their positions
				if (LanguageBtnDict.TryGetValue(__instance.GetInstanceID(), out var languageButton))
				{
					// just reuse already calculated positions on creating
					languageButton.Button.transform.position = languageButton.Position;
				}
			}
		}
	}
}
#endif