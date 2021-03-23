using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenerateMeshFromHeightMap))]
public class MorphTerrain : MonoBehaviour
{
    public KeyCode startMorhKey = KeyCode.Space;

    public float animTime = 2;
    const float TRANSITION_SCALE = 0.1f;
    public float minScale = 1;
    public float maxScale = 4;

    GenerateMeshFromHeightMap terrainGenerator;

    private void Start()
    {
        terrainGenerator = GetComponent<GenerateMeshFromHeightMap>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(startMorhKey))
        {
            //Stop any coroutine on going to avoid creating a mess if spamming the button
            StopAllCoroutines();
            //Start the anim
            StartCoroutine(Morph());
        }
    }

    IEnumerator Morph()
    {
        float t = 0;
        Vector3 scale = transform.localScale;
        float startScale = scale.y;

        //Scale down loop
        while(t<1)
        {
            t += Time.deltaTime / animTime * 2;
            scale.y = Curves.QuadEaseInOut(startScale, TRANSITION_SCALE, t);
            transform.localScale = scale;

            yield return null;
        }

        //Update the terrain
        terrainGenerator.GetRandomMap();
        terrainGenerator.Generate();

        t = 0;
        float targetScale = Random.Range(minScale, maxScale);
        //Scale Up loop
        while (t < 1)
        {
            t += Time.deltaTime / animTime * 2;
            scale.y = Curves.QuadEaseInOut(TRANSITION_SCALE, targetScale, t);
            transform.localScale = scale;

            yield return null;
        }
    }
}
