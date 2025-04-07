using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuTurn : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] bool side; // False for left, true for right
    [SerializeField] bool down;
    [SerializeField] bool up;
    [SerializeField] CameraController CamController;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!up && !down) {
            CamController.Turn(side);
        }
        else
        {
            CamController.Turn(false, down);
        }
    }
}
