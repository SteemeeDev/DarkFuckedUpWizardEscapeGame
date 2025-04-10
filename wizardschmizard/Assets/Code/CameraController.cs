using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform[] lookObjects;
    [SerializeField] Transform downObject;
    bool lookingDown = false;
    [SerializeField] float turnSpeed = 800f;
    [SerializeField] float turnBufferTime = 0.2f;

    float timeSinceLastTurn;
    int lookIndex = 0;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Turn(bool right)
    {
        if (timeSinceLastTurn < turnBufferTime) return;
        if (lookingDown) return;

        timeSinceLastTurn = 0;

        if (!right)
        {
            lookIndex--;
            
            if (lookIndex < 0)
            {
                lookIndex = lookObjects.Length - 1;
            }
        }
        else if (right)
        {
            lookIndex++;

            if (lookIndex >= lookObjects.Length)
            {
                lookIndex = 0;
            }
        }

    }

    public void Turn(bool left, bool down)
    {
        if (timeSinceLastTurn < turnBufferTime) return;

        timeSinceLastTurn = 0;

        if (down)
        {
            lookingDown = true;
        }
        else if (!down)
        {
            lookingDown = false;
        }
    }


    private void Update()
    {
        timeSinceLastTurn += Time.deltaTime;

        if (!lookingDown)
        {
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation,
                Quaternion.LookRotation(lookObjects[lookIndex].transform.position - transform.position), turnSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation,
                Quaternion.LookRotation(downObject.position - transform.position), turnSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow )) Turn(false);
        if (Input.GetKeyDown(KeyCode.RightArrow)) Turn(true);
        if (Input.GetKeyDown(KeyCode.UpArrow   )) Turn(false, false);
        if (Input.GetKeyDown(KeyCode.DownArrow )) Turn(false, true);
    }
}
