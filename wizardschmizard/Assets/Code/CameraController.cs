using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform[] lookObjects;
    [SerializeField] float turnSpeed = 800f;
    [SerializeField] float turnBufferTime = 0.2f;

    float timeSinceLastTurn;
    int lookIndex = 0;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Turn(bool left)
    {  
        
        if (left && timeSinceLastTurn > turnBufferTime)
        {
            lookIndex--;
            
            if (lookIndex < 0)
            {
                lookIndex = lookObjects.Length - 1;
            }

            timeSinceLastTurn = 0;
        }
        else if (!left && timeSinceLastTurn > turnBufferTime)
        {
            lookIndex++;

            if (lookIndex >= lookObjects.Length)
            {
                lookIndex = 0;
            }
            
            timeSinceLastTurn = 0;
        }
    }

    private void Update()
    {
        timeSinceLastTurn += Time.deltaTime;

        transform.rotation =
            Quaternion.RotateTowards(transform.rotation,
            Quaternion.LookRotation(lookObjects[lookIndex].transform.position - transform.position), turnSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftArrow)) Turn(true);
        if (Input.GetKeyDown(KeyCode.RightArrow)) Turn(false);
    }
}
