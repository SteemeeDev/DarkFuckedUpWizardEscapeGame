using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] Transform itemHoldPos;
    [SerializeField] GameObject itemParent;
    [SerializeField] GameObject[] inventoryElements;
    [SerializeField] Camera itemCamera;

    float scrollInput;
    [SerializeField] float zoomIntensity;
    [SerializeField] float maxZoom = 2f;

    public GameObject shownItem;
    public InvItem shownInvItem;

    public List<GameObject> items;
    
    public void AddItem(InvItem item)
    {
        GameObject newItem = new GameObject(item.name);
        newItem.transform.parent = itemParent.transform;
        newItem.AddComponent<Image>();
        MenuInvItem menuInvItem = newItem.AddComponent<MenuInvItem>();
        menuInvItem.invItem = item;
        items.Add(newItem);
        newItem.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    public void RemoveItem(InvItem item)
    {
        foreach (GameObject newItem in items)
        {
            if (newItem.GetComponent<MenuInvItem>().invItem == item)
            {
                Destroy(shownItem);
                items.Remove(newItem);
                Destroy(newItem);
                return;
            }
        }

        Debug.Log("Item not found!");
    }
    public void ShowObject(InvItem item)
    {
        Debug.Log("Displaying item: " + item.name);

        shownInvItem = item;
        if (shownItem != null) Destroy(shownItem);
        shownItem = Instantiate(item.renderObject, itemHoldPos);
        shownItem.transform.localPosition = Vector3.zero;   
    }


    // Code borrowed from https://www.youtube.com/watch?v=kplusZYqBok
    [SerializeField] float rotateSensitivty = 0.5f;

    Vector3 mPrevPos = Vector3.zero;
    Vector3 mPosDelta = Vector3.zero;

    void RotateObject()
    {
        mPosDelta = Input.mousePosition - mPrevPos;
        shownItem.transform.Rotate(itemCamera.transform.up, -Vector3.Dot(mPosDelta, itemCamera.transform.right) * rotateSensitivty, Space.World);
        shownItem.transform.Rotate(itemCamera.transform.right, Vector3.Dot(mPosDelta, itemCamera.transform.up) * rotateSensitivty, Space.World);
    }

    void Update()
    {
        if (shownItem != null) 
        {
            if (Input.GetMouseButton(1))
                RotateObject();
            mPrevPos = Input.mousePosition;

            scrollInput += Input.GetAxis("Mouse ScrollWheel") * zoomIntensity;
            scrollInput = math.clamp(scrollInput, -maxZoom, maxZoom);

            shownItem.transform.position = itemHoldPos.position + itemCamera.transform.forward * scrollInput;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (shownItem != null) Destroy(shownItem);
            itemParent.SetActive(!itemParent.activeSelf);
            foreach (GameObject g in inventoryElements)
            {
                g.SetActive(!g.activeSelf);
            }
        }
    }
}