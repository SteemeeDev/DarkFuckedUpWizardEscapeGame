using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCirclePiece : MonoBehaviour
{
    bool a = false;
    SpriteRenderer m_spriteRenderer;
    Color startCol;
    private void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        startCol = m_spriteRenderer.material.color;
    }
    public void Click()
    {
        Debug.Log("CLIKED!");
        if (a)
            m_spriteRenderer.material.color = Color.black;
        else
        {
            m_spriteRenderer.material.color = startCol;
        }
        a = !a;
    }
}
