using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform[] lookObjects;

    int lookIndex = 0;

    Vector3 currentLook;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            lookIndex--;
            if (lookIndex < 0)
            {
                lookIndex = lookObjects.Length - 1;
            }

            StopAllCoroutines();
            StartCoroutine(Turn(0.5f));
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            lookIndex++;
            if (lookIndex >= lookObjects.Length)
            {
                lookIndex = 0;
            }

            StopAllCoroutines();
            StartCoroutine(Turn(0.5f));
        }
    }

    IEnumerator Turn(float turnTime)
    {
        float elapsed = 0;
        while (elapsed < turnTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / turnTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookObjects[lookIndex].position- transform.position), t);
            yield return null;
        }
    }
}
