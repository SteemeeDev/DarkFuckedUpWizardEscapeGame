using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCirclePiece : MonoBehaviour
{
    bool a = true;
    SpriteRenderer spriteRenderer;
    Color startCol;
    private void Start()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        startCol = spriteRenderer.color;
    }
    public void Click()
    {
        if (a)
            spriteRenderer.color = Color.black;
        else
        {
            spriteRenderer.color = startCol;
        }
        a = !a;
    }
}
