using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class NewBehaviourScript : PhysItem
{
    bool a = false;
    MeshRenderer m_MeshRenderer;
    Color startCol;
    private void Start()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();
        startCol = m_MeshRenderer.material.color;
    }
    public override void Click()
    {
        base.Click();
        Debug.Log("Click on book!");
        if (a)
            m_MeshRenderer.material.color = Color.black;
        else
        {
            m_MeshRenderer.material.color = startCol;
        }
        a = !a;
    }


    public override void Update()
    {
        // Interact with subitems before the whole item
        RaycastHit hit;
        
        if (Physics.Raycast(itemCamera.ScreenPointToRay(Input.mousePosition), out hit, 100, itemPropertyLayer) && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Hit: " + hit.transform.name + " by " + this);
            hit.transform.GetComponent<BookCirclePiece>().Click();
        }
    }
}