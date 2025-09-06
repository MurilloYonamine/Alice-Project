using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.XPath;
using UnityEngine;

public class FileManager
{
    private const string KEY = "SECRETKEY";

    public static string TextFileName(TextAsset filePath)
    {
        if (filePath == null)
        {
            Debug.LogError("File not found!");
            return null;
        }
        return filePath.name;
    }
    public static List<string> ReadTextFile(string filePath, bool includeBlankLines = true)
    {
        if (!filePath.StartsWith('/'))
            filePath = FilePaths.root + filePath;

        List<string> lines = new List<string>();
        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (includeBlankLines || !string.IsNullOrWhiteSpace(line))
                        lines.Add(line);
                }
            }
        }
        catch (FileNotFoundException ex)
        {
            Debug.LogError($"File not found: '{ex.FileName}'");
        }

        return lines;
    }

    public static List<string> ReadTextAsset(string filePath, bool includeBlankLines = true)
    {
        TextAsset asset = Resources.Load<TextAsset>(filePath);
        if (asset == null)
        {
            Debug.LogError($"Asset not found: '{filePath}'");
            return null;
        }

        return ReadTextAsset(asset, includeBlankLines);
    }

    public static List<string> ReadTextAsset(TextAsset asset, bool includeBlankLines = true)
    {
        List<string> lines = new List<string>();
        using (StringReader sr = new StringReader(asset.text))
        {
            while (sr.Peek() > -1)
            {
                string line = sr.ReadLine();
                if (includeBlankLines || !string.IsNullOrWhiteSpace(line))
                    lines.Add(line);
            }
        }

        return lines;
    }

    public static bool TryCreateDirectoryFromPath(string path)
    {
        if (Directory.Exists(path) || File.Exists(path))
            return true;

        if (path.Contains("."))
        {
            path = Path.GetDirectoryName(path);
            if (Directory.Exists(path))
                return true;
        }

        if (path == string.Empty)
            return false;

        try
        {
            Directory.CreateDirectory(path);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Could not create directory! {e}");
            return false;
        }
    }

    public static void Save(string filePath, string JSONData, bool encrypt = false)
    {
        if (!TryCreateDirectoryFromPath(filePath))
        {
            Debug.LogError($"FAILED TO SAVE FILE '{filePath}' Please see the console for error details.");
            return;
        }

        if (encrypt)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(JSONData);
            byte[] keyBytes = Encoding.UTF8.GetBytes(KEY);
            byte[] encryptedBytes = XOR(dataBytes, keyBytes);

            File.WriteAllBytes(filePath, encryptedBytes);
        }
        else
        {
            StreamWriter sw = new StreamWriter(filePath);
            sw.Write(JSONData);
            sw.Close();
        }

        Debug.Log($"Saved data to file '{filePath}'");
    }

    public static T Load<T>(string filePath, bool encrypt = false)
    {
        if (File.Exists(filePath))
        {
            if (encrypt)
            {
                byte[] encryptedBytes = File.ReadAllBytes(filePath);
                byte[] keyBytes = Encoding.UTF8.GetBytes(KEY);

                byte[] decryptedBytes = XOR(encryptedBytes, keyBytes);

                string decryptedString = Encoding.UTF8.GetString(decryptedBytes);

                return JsonUtility.FromJson<T>(decryptedString);
            }
            else
            {
                string JSONData = File.ReadAllLines(filePath)[0];
                return JsonUtility.FromJson<T>(JSONData);
            }
        }
        else
        {
            Debug.LogError($"Error - File does not exist! '{filePath}'");
            return default(T);
        }
    }

    private static byte[] XOR(byte[] input, byte[] key)
    {
        byte[] output = new byte[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            output[i] = (byte)(input[i] ^ key[i % key.Length]);
        }

        return output;
    }
}
