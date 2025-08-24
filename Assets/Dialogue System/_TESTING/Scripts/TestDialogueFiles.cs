using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

namespace TESTING
{
    public class TestDialogueFiles : MonoBehaviour
    {
        [SerializeField] private TextAsset fileToRead = null;
        void Start()
        {
            StartConversation();
        }
        void StartConversation()
        {
            List<string> lines = FileManager.ReadTextAsset(fileToRead);

            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;

                DIALOGUE_LINE dl = DialogueParser.Parse(line);

                /*for (int i = 0; i < dl.commandData.commands.Count; i++)
                {
                    DL_COMMAND_DATA.Command command = dl.commandData.commands[i];
                    Debug.Log($"comando [{i}] '{command.name}' tem argumentos: [{string.Join(", ", command.arguments)}]");
                }*/
            }
            Debug.Log(FileManager.TextFileName(fileToRead));
            DialogueSystem.instance.Say(lines);
        }
    }
}