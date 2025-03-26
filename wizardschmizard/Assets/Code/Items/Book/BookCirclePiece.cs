using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sits on layer 7, as an itemproperty
public class BookCirclePiece : MonoBehaviour
{
    public bool activated = false;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Color startCol;

    [Range(1, 8)] public int index; // Index for solving puzzle
    private void Start()
    {
        skinnedMeshRenderer = GetComponentInParent<SkinnedMeshRenderer>();
        startCol = skinnedMeshRenderer.material.color;
        skinnedMeshRenderer.material.color = startCol * 0.5f;
    }
    public void Click()
    {
        if (activated) 
            skinnedMeshRenderer.material.color = startCol * 0.5f; 
        else
            skinnedMeshRenderer.material.color = startCol;
        
        activated = !activated;
    }
}
