using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ItemPickupManager : MonoBehaviour
{
    [SerializeField] InventoryManager inventoryManager;
    [SerializeField] DialogueManager dialogueManager;

    const int worldItemLayer = 1 << 8;
    const int worldPreviewItemLayer = 1 << 9;
    RaycastHit hit;
    Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1)) return;
       
        if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit, 100, worldItemLayer))
        {
            WorldItem hitWorldItem = hit.transform.GetComponent<WorldItem>();
            if (Input.GetMouseButtonDown(0) && hitWorldItem != null)
            {
                inventoryManager.AddItem(hitWorldItem.invItem);
                Destroy(hit.transform.gameObject);
            }
            else if (Input.GetMouseButtonDown(1) && hitWorldItem != null)
            {
                if (hitWorldItem.invItem.dialogue != null) StartCoroutine(dialogueManager.ReadDialogue(hitWorldItem.invItem.dialogue, 0));
            }
        }


        if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit, 100, worldPreviewItemLayer)) 
        {
            WorldItem hitWorldItem = hit.transform.GetComponent<WorldItem>();

            if (Input.GetMouseButtonDown(0) && hitWorldItem != null) 
            {
                inventoryManager.ShowObject(hitWorldItem.invItem);
            }
            else if (Input.GetMouseButtonDown(1) && hitWorldItem != null)
            {
                if (hitWorldItem.invItem.dialogue != null)
                {
                    dialogueManager.StopAllCoroutines();
                    StartCoroutine(dialogueManager.ReadDialogue(hitWorldItem.invItem.dialogue, 0));
                }
            } 

        }
    }
}
