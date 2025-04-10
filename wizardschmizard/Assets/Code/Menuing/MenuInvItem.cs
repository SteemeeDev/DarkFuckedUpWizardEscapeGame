using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuInvItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public InvItem invItem;
    InventoryManager invManager;

    private void Start()
    {
        if (invItem != null) GetComponent<Image>().sprite = invItem.sprite;
        invManager = GetComponentInParent<InventoryManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }
    public void OnPointerExit(PointerEventData eventData)
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (invManager.shownItem != null && invManager.shownInvItem == invItem) {
            invManager.saveManager.SaveLastItemState(invManager.shownItem);
            Destroy(invManager.shownItem);
        }
        else invManager.ShowObject(invItem);
    }
}
