using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CHARACTERS;
using UnityEngine;

namespace COMMANDS
{
    public class CMD_DatabaseExtension_Characters : CMD_DatabaseExtension
    {
        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("createcharacter", new Action<string[]>(CreateCharacter));

            //Add commands to characters
            CommandDatabase baseCommands = CommandManager.instance.CreateSubDatabase(CommandManager.DATABASE_CHARACTERS_BASE);
            baseCommands.AddCommand("setColor", new Func<string[], IEnumerator>(SetColor));
        }

        #region Global Commands
        private static void CreateCharacter(string[] data)
        {
            string characterName = data[0];
            Character character = CharacterManager.instance.CreateCharacter(characterName);
        }
        #endregion

        #region BASE CHARACTER COMMANDS
        public static IEnumerator SetColor(string[] data)
        {
            Character character = CharacterManager.instance.GetCharacter(data[0], createIfDoesNotExist: false);
            string colorName;

            if (character == null || data.Length < 2)
                yield break;

            //Grab the extra parameters
            var parameters = ConvertDataToParameters(data, startingIndex: 1);

            //Try to get the color name
            parameters.TryGetValue(new string[] { "-c", "-color" }, out colorName);

            //Get the color value from the name
            Color color = Color.white;
            color = color.GetColorFromName(colorName);

            character.SetColor(color);

            yield break;
        }
        #endregion
    }
}