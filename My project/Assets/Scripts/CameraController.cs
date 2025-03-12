using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform[] lookObjects;
    [SerializeField] float turnSpeed = 800f;

    int lookIndex = 0;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Turn(bool left)
    {   
        if (left)
        {
            lookIndex--;
            if (lookIndex < 0)
            {
                lookIndex = lookObjects.Length - 1;
            }
        }
        else
        {
            lookIndex++;
            if (lookIndex >= lookObjects.Length)
            {
                lookIndex = 0;
            }
        }
    }

    private void Update()
    {

        transform.rotation =
            Quaternion.RotateTowards(transform.rotation,
            Quaternion.LookRotation(lookObjects[lookIndex].transform.position - transform.position), turnSpeed * Time.deltaTime);
    }
}
