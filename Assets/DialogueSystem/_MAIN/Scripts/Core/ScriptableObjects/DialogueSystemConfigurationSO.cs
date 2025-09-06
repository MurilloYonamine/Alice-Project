using CHARACTERS;
using TMPro;
using UnityEngine;

namespace DIALOGUE
{
    [CreateAssetMenu(fileName = "Dialogue System Configuration", menuName = "Dialogue System/Dialogue Configuration Asset")]
    public class DialogueSystemConfigurationSO : ScriptableObject
    {
        public const float DEFAULT_FONTSIZE_DIALOGUE = 18;
        public const float DEFAULT_FONTSIZE_NAME = 22;

        public CharacterConfigSO characterConfigurationAsset;

        public Color defaultTextColor = Color.white;
        public TMP_FontAsset defaultFont;

        public float dialogueFontScale = 1f;
        public float defaultNameFontSize = DEFAULT_FONTSIZE_NAME;
        public float defaultDialogueFontSize = DEFAULT_FONTSIZE_DIALOGUE;
    }
}