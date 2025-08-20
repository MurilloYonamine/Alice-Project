using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorAttributes;
[CreateAssetMenu(fileName = "Biome Preset", menuName = "New Biome Preset")]
public class BiomePreset : ScriptableObject
{
    [field: SerializeField] public Sprite[] Tiles { get; private set; }

    [field: SerializeField, Range(0f, 1f)] public float MinHeight { get; private set; }
    [field: SerializeField, Range(0f, 1f)] public float MinMoisture { get; private set; }
    [field: SerializeField, Range(0f, 1f)] public float MinHeat { get; private set; }

    public Sprite GetTleSprite()
    {
        return Tiles[Random.Range(0, Tiles.Length)];
    }

    public bool MatchCondition(float height, float moisture, float heat)
    {
        return height >= MinHeight && moisture >= MinMoisture && heat >= MinHeat;
    }

    [Button("Randomize")]
    private void Randomize()
    {
        MinHeight = Random.Range(0f, 1f);
        MinMoisture = Random.Range(0f, 1f);
        MinHeat = Random.Range(0f, 1f);
    }
}