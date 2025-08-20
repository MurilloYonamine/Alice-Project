using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{
    public static float[,] Generate(int width, int height, float scale, Wave[] waves, Vector2 offset)
    {
        float[,] noiseMap = new float[width, height];

        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                float samplePosX = (float)x * scale + offset.x;
                float samplePosY = (float)y * scale + offset.y;

                float normalization = 0.0f;
                foreach (Wave wave in waves)
                {
                    noiseMap[x, y] += wave.Amplitude * Mathf.PerlinNoise(samplePosX * wave.Frequency + wave.Seed, samplePosY * wave.Frequency + wave.Seed);
                    normalization += wave.Amplitude;
                }
                noiseMap[x, y] /= normalization;
            }
        }

        return noiseMap;
    }

}
