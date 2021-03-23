using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMeshFromHeightMap : MonoBehaviour
{
    public Texture2D heightMap = null;
    Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        Generate();
    }

    public void GetRandomMap()
    {
        heightMap = RandomHeightMap.Instance.Generate(256, 256);
    }

    public void Generate()
    {
        GetComponent<MeshFilter>().mesh = GenerateMesh();
    }

    Mesh GenerateMesh()
    {
        if (heightMap == null)
            GetRandomMap();

        if (heightMap.width > 256 || heightMap.height > 256)
            Debug.LogWarning("HeightMap is too big, it may cause issues");

        Vector3[] points = GeneratePointsFromHeightMap(heightMap.width, heightMap.height, heightMap);
        int[] triangles = GenerateTriangles(heightMap.width, heightMap.height);
        Vector2[] uvs = GenerateUVs(heightMap.width, heightMap.height);

        mesh.vertices = points;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        return mesh;
    }

    Vector3[] GeneratePointsFromHeightMap(int width, int height, Texture2D heightMap)
    {
        //Create the array to store the points
        Vector3[] points = new Vector3[width * height];

        //Read the pixels from the texture
        Color[] pixels = heightMap.GetPixels();

        //Move each point on y according to the heightMap
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                //Get the id in the 1D array.
                //The ids are from left to right, bottom to top.
                int id = z * width + x;

                if(x==0||z==0 || z==height-1||x==width-1)
                    //Keep the edges at 0
                    points[id] = new Vector3(x / (float)width - 0.5f, 0, z / (float)height - 0.5f);
                else
                //Keep the points coords between 0 and 1 by dividing by the size. Make sure to cast in float to avoid euclidian division on ints.
                points[id] = new Vector3(x/(float)width - 0.5f, pixels[id].grayscale, z/(float)height - 0.5f);
            }
        }

        return points;
    }

    int[] GenerateTriangles(int width, int height)
    {
        List<int> triangles = new List<int>();

        //Start at 1 since we are working on x/y and x-1/y-1
        for (int y= 1; y < height; y++)
        {
            for (int x = 1; x < width; x++)
            {
                //First triangle
                //Bottom left
                triangles.Add(width * (y - 1) + x-1);
                //Top Left
                triangles.Add(width * y + x - 1);
                //Bottom Right 
                triangles.Add(width * (y - 1) + x);
                //Second triangle
                //Bottom Right 
                triangles.Add(width * (y - 1) + x);
                //Top Left
                triangles.Add(width * y + x - 1);
                //Top right
                triangles.Add(width * y + x);
            }
        }

        return triangles.ToArray();
    }

    Vector2[] GenerateUVs(int width, int height)
    {
        //UVs are in 2D
        Vector2[] uvs = new Vector2[width * height];

        //Map the UVs left to right, bottom to top
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //Get the id in the 1D array.
                //The ids are from left to right, bottom to top.
                int id = y * width + x;

                uvs[id] = new Vector2(x / (float)width, y / (float)height);
            }
        }

        return uvs;
    }
}
