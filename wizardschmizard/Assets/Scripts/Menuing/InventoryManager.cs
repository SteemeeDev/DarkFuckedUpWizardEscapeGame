using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] Transform itemHoldPos;
    [SerializeField] GameObject items;
    [SerializeField] Camera itemCamera;

    GameObject shownItem;

    public void ShowObject(InvItem item)
    {
        if (shownItem != null) Destroy(shownItem);
        shownItem = Instantiate(item.prefab, itemHoldPos);
        shownItem.transform.localPosition = Vector3.zero;
    }


    // Code borrowed from https://www.youtube.com/watch?v=kplusZYqBok

    [SerializeField] float rotateSensitivty = 0.5f;

    Vector3 mPrevPos = Vector3.zero;
    Vector3 mPosDelta = Vector3.zero;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            mPosDelta = Input.mousePosition - mPrevPos;
            shownItem.transform.Rotate(itemCamera.transform.up, -Vector3.Dot(mPosDelta, itemCamera.transform.right) * rotateSensitivty, Space.World);
            shownItem.transform.Rotate(itemCamera.transform.right, Vector3.Dot(mPosDelta, itemCamera.transform.up)  * rotateSensitivty, Space.World);
        }

        mPrevPos = Input.mousePosition;


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (shownItem != null) Destroy(shownItem);
            items.SetActive(!items.activeSelf);
        }
    }
}
