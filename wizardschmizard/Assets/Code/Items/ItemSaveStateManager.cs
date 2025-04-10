using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSaveStateManager : MonoBehaviour
{
    [SerializeField] Quaternion lastItemRotation;
    [SerializeField] Vector3 lastItemPosition;

    List<int> order = new List<int>();


    public void SaveLastItemState(GameObject item)
    {
        Debug.Log("Saving item state...");

        lastItemRotation = item.transform.rotation;
        lastItemPosition = item.transform.position;

        if (item.GetComponent<Book>() != null )
        {
            order = item.GetComponent<Book>().order;
        }
    }

    public void LoadLastItemState(GameObject item) {
        if (item.transform.rotation == null) return;
        Debug.Log("Loading last item state...");

        item.transform.rotation = lastItemRotation;
        item.transform.position = lastItemPosition;


        if (item.GetComponent<Book>() != null && order.Count != 0) {
            Debug.Log("Loading last book state...");
            Book BookComponent = item.GetComponent<Book>();
            BookComponent.order = order;

            for (int i = 0; order.Count > i; i++)
            {
                Debug.Log($"Pressing {BookComponent.order[i]}");
                // 
                foreach (BookCirclePiece piece in BookComponent.bookCirclePieces)
                {
                    if (piece.index == order[i])
                    {
                        piece.Click();
                        break;
                    }
                }
            }

        }
    }
}
