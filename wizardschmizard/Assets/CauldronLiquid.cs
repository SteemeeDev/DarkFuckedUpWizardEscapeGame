using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CauldronLiquid : MonoBehaviour
{
    [Range(0, 1.0f)] public float fill;
    [SerializeField] float scale;

    Vector3 startPos;
    Vector3 startScale;

    [SerializeField] Transform topPos;

    private void Awake()
    {
        startPos = transform.position;
        startScale = transform.localScale;
    }

    [ContextMenu("Update Fill")]
    public void UpdateLiquid()
    {
        transform.position = new Vector3(
            transform.position.x,
            startPos.y + (topPos.position.y - startPos.y) * fill,
            transform.position.z
        );
        transform.localScale = new Vector3(
            startScale.x * scale * (fill + 1),
            startScale.y,
            startScale.z * scale * (fill + 1)
        );
    }
}
