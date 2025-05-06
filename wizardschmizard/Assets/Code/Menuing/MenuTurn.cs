using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuTurn : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] bool side; // False for left, true for right
    [SerializeField] bool down;
    [SerializeField] bool up;
    [SerializeField] CameraController CamController;

    Image img;
    Color startColor;

    private void Start()
    {
        img = GetComponent<Image>();
        startColor = img.color;
    }

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

    private void Update()
    {
        if (CamController.lookingDown)
        {
            img.color = up ? startColor : new Color(0, 0, 0, 0);
        }
        else if (!CamController.lookingDown)
        {
            img.color = up ? new Color(0, 0, 0, 0) : startColor;
        }
    }
}
