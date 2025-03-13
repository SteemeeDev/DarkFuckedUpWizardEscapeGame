using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public abstract class PhysItem : MonoBehaviour
{
    public const int itemLayer = 1 << 6;
    public const int itemPropertyLayer = 1 << 7;
    public Camera itemCamera;

    private void Awake()
    {
        itemCamera = GameObject.FindGameObjectWithTag("ItemCamera").GetComponent<Camera>();
    }

    public virtual void Click()
    {

    }
    RaycastHit hit;
    public virtual void Update()
    {
        if (Physics.Raycast(itemCamera.ScreenPointToRay(Input.mousePosition), out hit, 100, itemLayer) && Input.GetMouseButtonDown(0))
        {
            Click();
        }
    }
}
