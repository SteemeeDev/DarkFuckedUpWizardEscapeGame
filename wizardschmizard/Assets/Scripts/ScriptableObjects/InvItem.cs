using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "InvItem", order = 0)]
public class InvItem : ScriptableObject
{
    public GameObject mesh; // inspect model
    public Sprite sprite; // Sprite used for inventory

    // What should happen when in interact mode
    public virtual void interact()
    {
        Debug.Log("Sigma");
    }
}
