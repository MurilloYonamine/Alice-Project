using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using UnityEngine.UIElements;

namespace DIALOGUE {
    public class DialogueSystem : MonoBehaviour {
        [SerializeField] private DialogueSystemConfigurationSO _config;
        public DialogueSystemConfigurationSO config => _config;

        public DialogueContainer dialogueContainer = new DialogueContainer();
        public ConversationManager conversationManager { get; private set; }
        private TextArchitect architect;
        [SerializeField] private CanvasGroup mainCanvas;

        public static DialogueSystem instance { get; private set; }

        public delegate void DialogueSystemEvent();
        public event DialogueSystemEvent onUserPrompt_Next;
        public event DialogueSystemEvent onClear;

        public bool isRunningConversation => conversationManager.isRunning;

        public DialogueContinuePrompt prompt;
        private CanvasGroupController cgController;

        private void Awake() {
            if (instance == null) {
                instance = this;
                Initialize();
            }
            else
                DestroyImmediate(gameObject);
        }

        bool _initialized = false;
        private void Initialize() {
            if (_initialized)
                return;

            architect = new TextArchitect(dialogueContainer.dialogueText, TABuilder.BuilderTypes.Typewriter);
            conversationManager = new ConversationManager(architect);

            cgController = new CanvasGroupController(this, mainCanvas);
            dialogueContainer.Initialize();
        }

        public void OnUserPrompt_Next() {
            onUserPrompt_Next?.Invoke();
        }

        public void OnSystemPrompt_Next() {
            onUserPrompt_Next?.Invoke();
        }

        public void OnSystemPrompt_Clear() {
            onClear?.Invoke();
        }

        public void OnStartViewingHistory() {
            prompt.Hide();
            conversationManager.allowUserPrompts = false;
        }

        public void OnStopViewingHistory() {
            prompt.Show();
            conversationManager.allowUserPrompts = true;
        }

        public void ApplySpeakerDataToDialogueContainer(string speakerName) {
            Character character = null;
            CharacterConfigData config = null;

            // Verificar se o CharacterManager existe antes de usar
            if (CharacterManager.instance != null) {
                character = CharacterManager.instance.GetCharacter(speakerName);
                config = character != null ? character.config : CharacterManager.instance.GetCharacterConfig(speakerName);
            }

            // Se não conseguimos obter a config, usar configuração padrão
            if (config == null) {
                Debug.LogWarning($"Could not find character config for '{speakerName}'. Using default configuration.");
                // Criar uma configuração padrão temporária
                config = CreateDefaultCharacterConfig();
            }

            ApplySpeakerDataToDialogueContainer(config);
        }

        private CharacterConfigData CreateDefaultCharacterConfig() {
            CharacterConfigData defaultConfig = new CharacterConfigData();

            // Usar as configurações padrão do sistema
            if (config != null) {
                defaultConfig.dialogueColor = Color.white;
                defaultConfig.nameColor = Color.white;
                defaultConfig.dialogueFontScale = 1f;
                defaultConfig.nameFontScale = 1f;
                // Deixar as fontes como null para usar as padrões do sistema
            }

            return defaultConfig;
        }

        public void ApplySpeakerDataToDialogueContainer(CharacterConfigData config) {
            //Set Dialogue details
            dialogueContainer.SetDialogueColor(config.dialogueColor);
            dialogueContainer.SetDialogueFont(config.dialogueFont);
            float fontSize = this.config.defaultDialogueFontSize * this.config.dialogueFontScale * config.dialogueFontScale;
            dialogueContainer.SetDialogueFontSize(fontSize);

            //Set name details
            dialogueContainer.nameContainer.SetNameColor(config.nameColor);
            dialogueContainer.nameContainer.SetNameFont(config.nameFont);
            fontSize = this.config.defaultNameFontSize * config.nameFontScale;
            dialogueContainer.nameContainer.SetNameFontSize(fontSize);
        }

        public void ShowSpeakerName(string speakerName = "") {
            if (speakerName.ToLower() != "narrator")
                dialogueContainer.nameContainer.Show(speakerName);
            else {
                HideSpeakerName();
                dialogueContainer.nameContainer.nameText.text = "";
            }

        }

        public void HideSpeakerName() => dialogueContainer.nameContainer.Hide();

        public Coroutine Say(string speaker, string dialogue) {
            List<string> conversation = new List<string>() { $"{speaker} \"{dialogue}\"" };
            return Say(conversation);
        }

        public Coroutine Say(List<string> lines, string filePath = "") {
            Conversation conversation = new Conversation(lines, file: filePath);
            return conversationManager.StartConversation(conversation);
        }

        public Coroutine Say(Conversation conversation) {
            return conversationManager.StartConversation(conversation);
        }

        public bool isVisible => cgController.isVisible;
        public Coroutine Show(float speed = 1f, bool immediate = false) => cgController.Show(speed, immediate);

        public Coroutine Hide(float speed = 1f, bool immediate = false) => cgController.Hide(speed, immediate);
    }

}