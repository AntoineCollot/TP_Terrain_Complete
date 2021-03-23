using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHeightMap : MonoBehaviour
{
    public static RandomHeightMap Instance;

    public DetailLevel[] detailLevels;

    [System.Serializable] public struct DetailLevel
    {
        public float frequency;
        public float strength;
    }

    private void Awake()
    {
        Instance = this;
    }

    public Texture2D Generate(int width, int height)
    {
        Texture2D heightMap = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Color[] pixels = new Color[width * height];
        Vector2 randomOffset = new Vector2(Random.Range(0f, 10000f), Random.Range(0f, 100000f));

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int id = y * width + x;

                float value = 0;
                for (int d = 0; d < detailLevels.Length; d++)
                {
                    float perlinX = x / (float)width * detailLevels[d].frequency + randomOffset.x;
                    float perlinY = y / (float)height * detailLevels[d].frequency + randomOffset.y;
                    value += Mathf.PerlinNoise(perlinX, perlinY) * detailLevels[d].strength;
                }
                pixels[id] = new Color(value, value, value, 1);
            }
        }

        heightMap.SetPixels(pixels);
        heightMap.Apply();
        return heightMap;
    }


}
