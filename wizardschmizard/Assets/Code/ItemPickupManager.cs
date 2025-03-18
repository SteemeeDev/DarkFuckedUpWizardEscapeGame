using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ItemPickupManager : MonoBehaviour
{
    [SerializeField] InventoryManager inventoryManager;

    const int worldItemLayer = 1 << 8;
    RaycastHit hit;
    Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit, 100, worldItemLayer) && Input.GetMouseButtonDown(0))
        {
            WorldItem hitWorldItem = hit.transform.GetComponent<WorldItem>();
            if (hitWorldItem != null)
            {
                inventoryManager.AddItem(hitWorldItem.invItem);
                Destroy(hit.transform.gameObject);
            }
        }
    }
}
