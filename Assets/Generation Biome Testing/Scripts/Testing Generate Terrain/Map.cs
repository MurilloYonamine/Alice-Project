using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Map : MonoBehaviour
{
    [field: SerializeField] public BiomePreset[] Biomes { get; private set; }
    [field: SerializeField] public GameObject TilePrefab { get; private set; }

    [Header("Dimensions")]
    [field: SerializeField] public int Width { get; private set; } = 50;
    [field: SerializeField] public int Height { get; private set; } = 50;
    [field: SerializeField] public float Scale { get; private set; } = 1.0f;
    [field: SerializeField] public Vector2 Offset { get; private set; }

    [Header("Height Map")]
    [field: SerializeField] public Wave[] HeightWaves { get; private set; }
    [field: SerializeField] public float[,] HeightMap { get; private set; }

    [Header("Moisture Map")]
    [field: SerializeField] public Wave[] MoistureWaves { get; private set; }
    [field: SerializeField] private float[,] MoistureMap { get; set; }

    [Header("Heat Map")]
    [field: SerializeField] public Wave[] HeatWaves { get; private set; }
    [field: SerializeField] private float[,] HeatMap { get; set; }
    private void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        // height map
        HeightMap = NoiseGenerator.Generate(Width, Height, Scale, HeightWaves, Offset);
        // moisture map
        MoistureMap = NoiseGenerator.Generate(Width, Height, Scale, MoistureWaves, Offset);
        // heat map
        HeatMap = NoiseGenerator.Generate(Width, Height, Scale, HeatWaves, Offset);

        for (int x = 0; x < Width; ++x)
        {
            for (int y = 0; y < Height; ++y)
            {
                GameObject tile = Instantiate(TilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                tile.GetComponent<SpriteRenderer>().sprite = GetBiome(HeightMap[x, y], MoistureMap[x, y], HeatMap[x, y]).GetTleSprite();
            }
        }
    }
    private BiomePreset GetBiome(float height, float moisture, float heat)
    {
        List<BiomeTempData> biomeTemp = new List<BiomeTempData>();
        foreach (BiomePreset biome in Biomes)
        {
            if (biome.MatchCondition(height, moisture, heat))
                biomeTemp.Add(new BiomeTempData(biome));
        }

        float curVal = 0.0f;
        BiomePreset biomeToReturn = null;

        foreach (BiomeTempData biome in biomeTemp)
        {
            if (biomeToReturn == null)
            {
                biomeToReturn = biome.biome;
                curVal = biome.GetDiffValue(height, moisture, heat);
                continue;
            }
            if (biome.GetDiffValue(height, moisture, heat) < curVal)
            {
                biomeToReturn = biome.biome;
                curVal = biome.GetDiffValue(height, moisture, heat);
            }
        }

        if (biomeToReturn == null)
            biomeToReturn = Biomes[0];

        return biomeToReturn;
    }
}
