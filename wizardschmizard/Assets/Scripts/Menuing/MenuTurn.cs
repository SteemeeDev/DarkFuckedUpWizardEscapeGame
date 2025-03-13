using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuTurn : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] bool left;

    [SerializeField] CameraController CamController;

    public void OnPointerEnter(PointerEventData eventData)
    {
        CamController.Turn(left);
    }
}
