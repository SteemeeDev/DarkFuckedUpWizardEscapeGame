using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] Transform itemHoldPos;
    [SerializeField] Transform itemInteractPos;
    [SerializeField] GameObject items;
    [SerializeField] Camera itemCamera;

    public bool interactMode = false; 
    [SerializeField] TMP_Text interactModeText;


    GameObject shownItem;
    InvItem shownInvItem;
    

    public void ShowObject(InvItem item)
    {
        shownInvItem = item;
        if (shownItem != null) Destroy(shownItem);
        shownItem = Instantiate(item.mesh, itemHoldPos);
        shownItem.transform.localPosition = Vector3.zero;
    }


    // Code borrowed from https://www.youtube.com/watch?v=kplusZYqBok
    [SerializeField] float rotateSensitivty = 0.5f;

    Vector3 mPrevPos = Vector3.zero;
    Vector3 mPosDelta = Vector3.zero;

    void RotateObject()
    {
        if (Input.GetMouseButton(0))
        {
            mPosDelta = Input.mousePosition - mPrevPos;
            shownItem.transform.Rotate(itemCamera.transform.up, -Vector3.Dot(mPosDelta, itemCamera.transform.right) * rotateSensitivty, Space.World);
            shownItem.transform.Rotate(itemCamera.transform.right, Vector3.Dot(mPosDelta, itemCamera.transform.up) * rotateSensitivty, Space.World);
        }

        mPrevPos = Input.mousePosition;
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E)) interactMode = !interactMode;
    

        if (shownItem != null)
        {
            if (!interactMode)
            {
                RotateObject();
                interactModeText.text = "Off";
            }
            if (interactMode)
            {
                shownInvItem.interact();
                interactModeText.text = "On";
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(shownItem);
                items.SetActive(!items.activeSelf);
            }
        }




    }
}
