using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "InvItem", order = 0)]
public class InvItem : ScriptableObject
{
    public GameObject renderObject; // Object used for inspection
    public Sprite sprite; // Sprite used for inventory
}
