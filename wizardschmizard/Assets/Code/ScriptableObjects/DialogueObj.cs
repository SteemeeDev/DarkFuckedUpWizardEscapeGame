using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue", order = 0)]
public class DialogueObj : ScriptableObject
{
    [TextAreaAttribute(8, 20)] public string Dialogue;
}
