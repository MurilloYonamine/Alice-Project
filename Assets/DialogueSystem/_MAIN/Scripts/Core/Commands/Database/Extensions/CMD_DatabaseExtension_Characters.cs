using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CHARACTERS;
using UnityEditor;
using UnityEngine;

namespace COMMANDS
{
    public class CMD_DatabaseExtension_Characters : CMD_DatabaseExtension
    {
        private static string[] PARAM_ENABLE => new string[] { "-e", "-enable" };
        private static string[] PARAM_IMMEDIATE => new string[] { "-i", "-immediate" };
        private static string[] PARAM_SPEED => new string[] { "-spd", "-speed" };
        private static string[] PARAM_SMOOTH => new string[] { "-sm", "-smooth" };
        private static string PARAM_XPOS => "-x";
        private static string PARAM_YPOS => "-y";
        private static string[] PARAM_ANIM => new string[] { "-a", "-anim", "-animation" };
        private static string[] PARAM_STATE => new string[] { "-s", "-state" };

        private static char EXPRESSIONLAYER_DELIMITER => DIALOGUE.DL_SPEAKER_DATA.EXPRESSIONLAYER_DELIMITER;
        private static char EXPRESSIONLAYER_JOINER => DIALOGUE.DL_SPEAKER_DATA.EXPRESSIONLAYER_JOINER;

        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("createcharacter", new Action<string[]>(CreateCharacter));
            database.AddCommand("movecharacter", new Func<string[], IEnumerator>(MoveCharacter));
            database.AddCommand("show", new Func<string[], IEnumerator>(ShowAll));
            database.AddCommand("hide", new Func<string[], IEnumerator>(HideAll));
            database.AddCommand("sort", new Action<string[]>(Sort));
            database.AddCommand("highlight", new Func<string[], IEnumerator>(HighlightAll));
            database.AddCommand("unhighlight", new Func<string[], IEnumerator>(UnhighlightAll));

            //Add commands to characters
            CommandDatabase baseCommands = CommandManager.instance.CreateSubDatabase(CommandManager.DATABASE_CHARACTERS_BASE);
            baseCommands.AddCommand("move", new Func<string[], IEnumerator>(MoveCharacter));
            baseCommands.AddCommand("show", new Func<string[], IEnumerator>(Show));
            baseCommands.AddCommand("hide", new Func<string[], IEnumerator>(Hide));
            baseCommands.AddCommand("setpriority", new Action<string[]>(SetPriority));
            baseCommands.AddCommand("setposition", new Action<string[]>(SetPosition));
            baseCommands.AddCommand("setColor", new Func<string[], IEnumerator>(SetColor));
            baseCommands.AddCommand("highlight", new Func<string[], IEnumerator>(Highlight));
            baseCommands.AddCommand("unhighlight", new Func<string[], IEnumerator>(Unhighlight));
            baseCommands.AddCommand("faceleft", new Func<string[], IEnumerator>(FaceLeft));
            baseCommands.AddCommand("faceright", new Func<string[], IEnumerator>(FaceRight));
            baseCommands.AddCommand("animate", new Action<string[]>(Animate));

            //Add character specific databases
            CommandDatabase spriteCommands = CommandManager.instance.CreateSubDatabase(CommandManager.DATABASE_CHARACTERS_SPRITE);
            spriteCommands.AddCommand("setsprite", new Func<string[], IEnumerator>(SetSprite));
        }

        #region Global Commands
        private static void CreateCharacter(string[] data)
        {
            string characterName = data[0];
            bool enable = false;
            bool immediate = false;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_ENABLE, out enable, defaultValue: false);
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            Character character = CharacterManager.instance.CreateCharacter(characterName);

            if (!enable)
                return;

            if (immediate)
                character.isVisible = true;
            else
                character.Show();
                
        }

        private static void Sort(string[] data)
        {
            CharacterManager.instance.SortCharacters(data);
        }

        private static IEnumerator MoveCharacter(string[] data)
        {
            string characterName = data[0];
            Character character = CharacterManager.instance.GetCharacter(characterName);

            if (character == null)
                yield break;

            float x = 0, y = 0;
            float speed = 1;
            bool smooth = false;
            bool immediate = false;

            var parameters = ConvertDataToParameters(data);

            //try to get the x axis position
            parameters.TryGetValue(PARAM_XPOS, out x);

            //try to get the y axis position
            parameters.TryGetValue(PARAM_YPOS, out y);

            //try to get the speed
            parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1);

            //try to get the smoothing
            parameters.TryGetValue(PARAM_SMOOTH, out smooth, defaultValue: false);

            //try to get imediate setting of position
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            Vector2 position = new Vector2(x, y);

            if (immediate)
                character.SetPosition(position);
            else
            {
                CommandManager.instance.AddTerminationActionToCurrentProcess(() => { character?.SetPosition(position); });
                yield return character.MoveToPosition(position, speed, smooth);
            }
        }

        private static IEnumerator ShowAll(string[] data)
        {
            List<Character> characters = new List<Character>();
            bool immediate = false;
            float speed = 1;

            if (data[0].ToLower() == "all")
            {
                characters.AddRange(CharacterManager.instance.allCharacters);
            }
            else
            {
                for (int i = 0; i < data.Length; i++)
                {
                    Character character = CharacterManager.instance.GetCharacter(data[i], createIfDoesNotExist: false);
                    if (character != null)
                        characters.Add(character);
                }
            }

            if (characters.Count == 0)
                yield break;

            //Convert the data array to a parameter container
            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);
            parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);

            //Call the logic on all the characters
            foreach (Character character in characters)
            {
                if (immediate)
                    character.isVisible = true;
                else
                    character.Show(speed);
            }

            if (!immediate)
            {
                CommandManager.instance.AddTerminationActionToCurrentProcess(() =>
                {
                    foreach(Character character in characters)
                        character.isVisible = true;
                });

                while (characters.Any(c => c.isRevealing))
                    yield return null;
            }
        }

        private static IEnumerator HideAll(string[] data)
        {
            List<Character> characters = new List<Character>();
            bool immediate = false;
            float speed = 1f;

            if (data[0].ToLower() == "all")
            {
                characters.AddRange(CharacterManager.instance.allCharacters);
            }
            else
            {
                for (int i = 0; i < data.Length; i++)
                {
                    Character character = CharacterManager.instance.GetCharacter(data[i], createIfDoesNotExist: false);
                    if (character != null)
                        characters.Add(character);
                }
            }

            if (characters.Count == 0)
                yield break;

            //Convert the data array to a parameter container
            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);
            parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);

            //Call the logic on all the characters
            foreach (Character character in characters)
            {
                if (immediate)
                    character.isVisible = false;
                else
                    character.Hide(speed);
            }

            if (!immediate)
            {
                CommandManager.instance.AddTerminationActionToCurrentProcess(() =>
                {
                    foreach (Character character in characters)
                        character.isVisible = false;
                });

                while (characters.Any(c => c.isHiding))
                    yield return null;
            }
        }

        private static IEnumerator HighlightAll(string[] data)
        {
            List<Character> characters = new List<Character>();
            bool immediate = false;
            bool handleUnspecifiedCharacters = true;
            List<Character> unspecifiedCharacters = new List<Character>();

            //Add any characters specified to be highlighted.
            if (data[0].ToLower() == "all")
            {
                characters.AddRange(CharacterManager.instance.allCharacters);
            }
            else
            {
                for (int i = 0; i < data.Length; i++)
                {
                    Character character = CharacterManager.instance.GetCharacter(data[i], createIfDoesNotExist: false);
                    if (character != null)
                        characters.Add(character);
                }
            }

            if (characters.Count == 0)
                yield break;

            //Grab the extra parameters
            var parameters = ConvertDataToParameters(data, startingIndex: 1);

            parameters.TryGetValue(new string[] { "-i", "-immediate" }, out immediate, defaultValue: false);
            parameters.TryGetValue(new string[] { "-o", "-only" }, out handleUnspecifiedCharacters, defaultValue: true);

            //Make all characters perform the logic
            foreach (Character character in characters)
                character.Highlight(immediate: immediate);

            //If we are forcing any unspecified characters to use the opposite highlighted status
            if (handleUnspecifiedCharacters)
            {
                foreach (Character character in CharacterManager.instance.allCharacters)
                {
                    if (characters.Contains(character))
                        continue;

                    unspecifiedCharacters.Add(character);
                    character.UnHighlight(immediate: immediate);
                }
            }

            //Wait for all characters to finish highlighting
            if (!immediate)
            {
                CommandManager.instance.AddTerminationActionToCurrentProcess(() =>
                {
                    foreach (var character in characters)
                        character.Highlight(immediate: true);

                    if (!handleUnspecifiedCharacters) return;

                    foreach (var character in unspecifiedCharacters)
                        character.UnHighlight(immediate: true);
                });

                while (characters.Any(c => c.isHighlighting) || (handleUnspecifiedCharacters && unspecifiedCharacters.Any(uc => uc.isUnHighlighting)))
                    yield return null;
            }
        }

        private static IEnumerator UnhighlightAll(string[] data)
        {
            List<Character> characters = new List<Character>();
            bool immediate = false;
            bool handleUnspecifiedCharacters = true;
            List<Character> unspecifiedCharacters = new List<Character>();

            //Add any characters specified to be highlighted.
            if (data[0].ToLower() == "all")
            {
                characters.AddRange(CharacterManager.instance.allCharacters);
            }
            else
            {
                for (int i = 0; i < data.Length; i++)
                {
                    Character character = CharacterManager.instance.GetCharacter(data[i], createIfDoesNotExist: false);
                    if (character != null)
                        characters.Add(character);
                }
            }

            if (characters.Count == 0)
                yield break;

            //Grab the extra parameters
            var parameters = ConvertDataToParameters(data, startingIndex: 1);

            parameters.TryGetValue(new string[] { "-i", "-immediate" }, out immediate, defaultValue: false);
            parameters.TryGetValue(new string[] { "-o", "-only" }, out handleUnspecifiedCharacters, defaultValue: true);

            //Make all characters perform the logic
            foreach (Character character in characters)
                character.UnHighlight(immediate: immediate);

            //If we are forcing any unspecified characters to use the opposite highlighted status
            if (handleUnspecifiedCharacters)
            {
                foreach (Character character in CharacterManager.instance.allCharacters)
                {
                    if (characters.Contains(character))
                        continue;

                    unspecifiedCharacters.Add(character);
                    character.Highlight(immediate: immediate);
                }
            }

            //Wait for all characters to finish highlighting
            if (!immediate)
            {
                CommandManager.instance.AddTerminationActionToCurrentProcess(() =>
                {
                    foreach (var character in characters)
                        character.UnHighlight(immediate: true);

                    if (!handleUnspecifiedCharacters) return;

                    foreach (var character in unspecifiedCharacters)
                        character.Highlight(immediate: true);
                });

                while (characters.Any(c => c.isUnHighlighting) || (handleUnspecifiedCharacters && unspecifiedCharacters.Any(uc => uc.isHighlighting)))
                    yield return null;
            }
        }

        private static IEnumerator FaceLeft(string[] data)
        {
            yield return FaceDirection(left: true, data);
        }

        private static IEnumerator FaceRight(string[] data)
        {
            yield return FaceDirection(left: false, data);
        }

        private static IEnumerator FaceDirection(bool left, string[] data)
        {
            string characterName = data[0];
            Character character = CharacterManager.instance.GetCharacter(characterName);

            if (character == null)
                yield break;

            float speed = 1;
            bool immediate = false;

            var parameters = ConvertDataToParameters(data);

            //Try to get the speed of the flip
            parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);

            //Try to see if this is an immediate effect or not.
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            if (left)
            {
                CommandManager.instance.AddTerminationActionToCurrentProcess(() => { character?.FaceLeft(immediate: true); });
                yield return character.FaceLeft(speed, immediate);
            }
            else
            {
                CommandManager.instance.AddTerminationActionToCurrentProcess(() => { character?.FaceRight(immediate: true); });
                yield return character.FaceRight(speed, immediate);
            }
                
        }

        private static void Animate(string[] data)
        {
            string characterName = data[0];
            Character character = CharacterManager.instance.GetCharacter(characterName);

            if (character == null)
            {
                Debug.LogError($"No character called '{data[0]}' was found. Can not animate.");
                return;
            }

            string animation;
            bool state;

            var parameters = ConvertDataToParameters(data, 1);

            //Try to get the speed of the flip
            parameters.TryGetValue(PARAM_ANIM, out animation);

            //Try to see if this is an immediate effect or not.
            bool hasState = parameters.TryGetValue(PARAM_STATE, out state, defaultValue: true);

            if (hasState)
                character.Animate(animation, state);
            else
                character.Animate(animation);
        }
        #endregion

        #region BASE CHARACTER COMMANDS
        private static IEnumerator Show(string[] data)
        {
            Character character = CharacterManager.instance.GetCharacter(data[0]);

            if (character == null)
                yield break;

            bool immediate = false;
            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(new string[] { "-i", "-immediate" }, out immediate, defaultValue: false);

            if (immediate)
                character.isVisible = true;
            else
            {
                //A long running process should have a stop action to cancel out the coroutine and run logic that should complete this command if interrupted
                CommandManager.instance.AddTerminationActionToCurrentProcess(() => { if (character != null) character.isVisible = true; });

                yield return character.Show();
            }
        }

        private static IEnumerator Hide(string[] data)
        {
            Character character = CharacterManager.instance.GetCharacter(data[0]);

            if (character == null)
                yield break;

            bool immediate = false;
            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(new string[] { "-i", "-immediate" }, out immediate, defaultValue: false);

            if (immediate)
                character.isVisible = false;
            else
            {
                //A long running process should have a stop action to cancel out the coroutine and run logic that should complete this command if interrupted
                CommandManager.instance.AddTerminationActionToCurrentProcess(() => { if (character != null) character.isVisible = false; });

                yield return character.Hide();
            }
        }

        public static void SetPosition(string[] data)
        {
            Character character = CharacterManager.instance.GetCharacter(data[0], createIfDoesNotExist: false);
            float x = 0, y = 0;

            if (character == null || data.Length < 2)
                return;

            var parameters = ConvertDataToParameters(data, 1);

            parameters.TryGetValue(PARAM_XPOS, out x, defaultValue: 0);
            parameters.TryGetValue(PARAM_YPOS, out y, defaultValue: 0);

            character.SetPosition(new Vector2(x, y));
        }

        public static void SetPriority(string[] data)
        {
            Character character = CharacterManager.instance.GetCharacter(data[0], createIfDoesNotExist: false);
            int priority;

            if (character == null || data.Length < 2)
                return;

            if (!int.TryParse(data[1], out priority))
                priority = 0;

            character.SetPriority(priority);
        }

        public static IEnumerator SetColor(string[] data)
        {
            Character character = CharacterManager.instance.GetCharacter(data[0], createIfDoesNotExist: false);
            string colorName;
            float speed;
            bool immediate;

            if (character == null || data.Length < 2)
                yield break;

            //Grab the extra parameters
            var parameters = ConvertDataToParameters(data, startingIndex: 1);

            //Try to get the color name
            parameters.TryGetValue(new string[] { "-c", "-color" }, out colorName);
            //Try to get the speed of the transition
            bool specifiedSpeed = parameters.TryGetValue(new string[] { "-spd", "-speed" }, out speed, defaultValue: 1f);
            //Try to get the instant value
            if (!specifiedSpeed)
                parameters.TryGetValue(new string[] { "-i", "-immediate" }, out immediate, defaultValue: true);
            else
                immediate = false;

            //Get the color value from the name
            Color color = Color.white;
            color = color.GetColorFromName(colorName);

            if (immediate)
                character.SetColor(color);
            else
            {
                CommandManager.instance.AddTerminationActionToCurrentProcess(() => { character?.SetColor(color); });
                character.TransitionColor(color, speed);
            }

            yield break;
        }

        public static IEnumerator Highlight(string[] data)
        {
            //format: SetSprite(character sprite)
            Character character = CharacterManager.instance.GetCharacter(data[0], createIfDoesNotExist: false) as Character;

            if (character == null)
                yield break;

            bool immediate = false;

            //Grab the extra parameters
            var parameters = ConvertDataToParameters(data, startingIndex: 1);

            parameters.TryGetValue(new string[] { "-i", "-immediate" }, out immediate, defaultValue: false);

            if (immediate)
                character.Highlight(immediate: true);
            else
            {
                CommandManager.instance.AddTerminationActionToCurrentProcess(() => { character?.Highlight(immediate: true); });
                yield return character.Highlight();
            }

        }

        public static IEnumerator Unhighlight(string[] data)
        {
            //format: SetSprite(character sprite)
            Character character = CharacterManager.instance.GetCharacter(data[0], createIfDoesNotExist: false) as Character;

            if (character == null)
                yield break;

            bool immediate = false;

            //Grab the extra parameters
            var parameters = ConvertDataToParameters(data, startingIndex: 1);

            parameters.TryGetValue(new string[] { "-i", "-immediate" }, out immediate, defaultValue: false);

            if (immediate)
                character.UnHighlight(immediate: true);
            else
            {
                CommandManager.instance.AddTerminationActionToCurrentProcess(() => { character?.UnHighlight(immediate: true); });
                yield return character.UnHighlight();
            }

        }
        #endregion

        #region SPRITE CHARACTER COMMANDS
        private static IEnumerator SetSprite(string[] data)
        {
            //format: SetSprite(character sprite)
            Character_Sprite character = CharacterManager.instance.GetCharacter(data[0], createIfDoesNotExist: false) as Character_Sprite;
            int layer = 0;
            string spriteName;
            bool immediate = false;
            float speed;

            if (character == null || data.Length < 2)
                yield break;

            //Grab the extra parameters
            var parameters = ConvertDataToParameters(data, startingIndex: 1);

            //Try to get the sprite name
            parameters.TryGetValue(new string[] { "-s", "-sprite" }, out spriteName);
            //Try to get the layer
            parameters.TryGetValue(new string[] { "-l", "-layer" }, out layer, defaultValue: 0);

            //Try to get the transition speed
            bool specifiedSpeed = parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f);

            //Try to get whether this is an immediate transition or not
            if (!specifiedSpeed)
                parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            //Get every sprite and layer pair specified in the spritename parameter if configured this way. This makes it easier and quicker to write.
            List<(Sprite sprite, int layer)> assignments = new List<(Sprite sprite, int layer)>();
            if (spriteName.Contains(EXPRESSIONLAYER_DELIMITER) || spriteName.Contains(EXPRESSIONLAYER_JOINER))
            {
                int layerCounter = 0;

                foreach (string pair in spriteName.Split(EXPRESSIONLAYER_JOINER))
                {
                    string[] v = pair.Split(EXPRESSIONLAYER_DELIMITER);
                    Sprite pairSprite = null;
                    int pairLayer = 0;

                    //If we do not specify a layer to the sprite we can just set multiple sprites on incrementing layers
                    if (v.Length < 2)
                    {
                        pairSprite = character.GetSprite(v[0]);
                        pairLayer = layerCounter++;
                    }
                    else
                    {
                        pairSprite = character.GetSprite(v[1]);
                        pairLayer = 0;
                        if (!int.TryParse(v[0], out pairLayer))
                            pairLayer = layerCounter++;
                    }
                    
                    assignments.Add((pairSprite, pairLayer));
                }
            }
            else
            {
                assignments.Add((character.GetSprite(spriteName), layer));
            }

            //We need to add a termination command that can run on every single expression layer reference
            CommandManager.instance.AddTerminationActionToCurrentProcess(
                () =>
                { 
                    if (character != null)
                    {
                        foreach (var pair in assignments)
                        {
                            Sprite sprite = pair.sprite;
                            int layer = pair.layer;

                            character.SetSprite(sprite, layer);
                        }
                    }
                }
                );

            //Run the logic on every expression and layer pair
            Coroutine retVal = null;
            foreach (var pair in assignments)
            {
                Sprite sprite = pair.sprite;
                layer = pair.layer;

                if (sprite == null)
                    continue;

                if (immediate)
                {
                    character.SetSprite(sprite, layer);
                }
                else
                {
                    retVal = character.TransitionSprite(sprite, layer, speed);
                }
            }

            if (!immediate)
                yield return retVal;
        }
        #endregion
    }
}