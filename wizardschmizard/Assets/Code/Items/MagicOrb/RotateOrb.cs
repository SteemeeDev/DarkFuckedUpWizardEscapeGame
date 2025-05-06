using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


public class RotateOrb : MonoBehaviour
{
    float rotationSpeed = 10;

    int x = 1, y = 1, z = 1;


    float elapsed = 0;
    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;

        if (elapsed > 5.0f)
        {
            x = Random.Range(-1, 2);
            y = Random.Range(-1, 2);
            z = Random.Range(-1, 2);

            elapsed = 0;

            Debug.Log($"x: {x} || y: {y} || z: {z}");
        }

        transform.Rotate(
            rotationSpeed * Time.deltaTime * Math.Sign(x), 
            rotationSpeed * Time.deltaTime * Math.Sign(y), 
            rotationSpeed * Time.deltaTime * Math.Sign(z)
        );
    }
}
