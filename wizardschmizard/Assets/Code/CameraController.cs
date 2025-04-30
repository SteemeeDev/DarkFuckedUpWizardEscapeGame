using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform[] lookObjects;
    [SerializeField] Transform downObject;
    [SerializeField] InventoryManager inventory;
    bool lookingDown = false;
    [SerializeField] float turnSpeed = 800f;
    [SerializeField] float turnBufferTime = 0.2f;
    [SerializeField] LayerMask lookObjectLayer = 1 << 14;

    Transform lookObject;
    
    float timeSinceLastTurn;
    int lookIndex = 0;

    Camera mainCam;

    Vector3 startPos;

    private void Awake()
    {
        startPos = transform.position;
        mainCam = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Turn(bool right)
    {
        if (timeSinceLastTurn < turnBufferTime) return;
        if (lookingDown) return;

        transform.position = startPos;

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

        lookObject = lookObjects[lookIndex];
    }

    public void Turn(bool left, bool down)
    {
        if (timeSinceLastTurn < turnBufferTime) return;

        transform.position = startPos;

        timeSinceLastTurn = 0;

        if (down)
        {
            lookingDown = true;
            lookObject = downObject;
        }
        else if (!down)
        {
            lookingDown = false;    
            lookObject = lookObjects[lookIndex];
        }
    }

    RaycastHit hit;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))  Turn(false);
        if (Input.GetKeyDown(KeyCode.RightArrow)) Turn(true);
        if (Input.GetKeyDown(KeyCode.UpArrow))    Turn(false, false);
        if (Input.GetKeyDown(KeyCode.DownArrow))  Turn(false, true);

        timeSinceLastTurn += Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit, 100, lookObjectLayer))
            {
                if (hit.transform.GetComponent<LookObj>() != null && inventory.shownItem == null)
                {
                     lookObject = null;

                     transform.position = hit.transform.GetComponent<LookObj>().CameraPos.position;
                     transform.rotation = hit.transform.GetComponent<LookObj>().CameraPos.rotation;

                    if (hit.transform.GetComponent<MixingManager>() != null)
                    {
                        hit.transform.GetComponent<MixingManager>().active = true;
                    }
                }
            }
        }

        if (lookObject == null) return;

        if (hit.transform != null && hit.transform.GetComponent<MixingManager>() != null)
        {
            hit.transform.GetComponent<MixingManager>().active = false;
        }

        transform.rotation =
        Quaternion.RotateTowards(transform.rotation,
        Quaternion.LookRotation(lookObject.position - transform.position), turnSpeed * Time.deltaTime);
        

    }
}
