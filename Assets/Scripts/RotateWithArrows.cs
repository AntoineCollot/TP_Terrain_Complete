using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithArrows : MonoBehaviour
{
    public float rotationSpeed = 60;
    public Vector3 rotationAxis = new Vector3(0, 1, 0);

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Valuer en -1 et 1
        float userInput = Input.GetAxis("Horizontal");
        //On multiplier par la valeur d'input, sans input on multiplie par 0
        transform.Rotate(rotationAxis.normalized * rotationSpeed * Time.deltaTime * userInput);
    }
}
