using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuInvItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] InvItem invItem;
    [SerializeField] 
    InventoryManager invManager;

    private void Start()
    {
        GetComponent<Image>().sprite = invItem.sprite;
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
        invManager.ShowObject(invItem);
    }
}
