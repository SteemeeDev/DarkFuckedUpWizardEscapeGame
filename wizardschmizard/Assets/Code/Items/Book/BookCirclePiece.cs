using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sits on layer 7, as an itemproperty
public class BookCirclePiece : MonoBehaviour
{
    public bool activated = false;
    public SpriteRenderer spriteRenderer;
    public Color startCol;

    [Range(1, 8)] public int index; // Index for solving puzzle
    private void Start()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        startCol = spriteRenderer.color;
        spriteRenderer.color = startCol * 0.5f;
    }
    public void Click()
    {
        if (activated) 
            spriteRenderer.color = startCol * 0.5f; 
        else
            spriteRenderer.color = startCol;
        
        activated = !activated;
    }
}
