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
        if (a)
            m_MeshRenderer.material.color = Color.white;
        else
        {
            m_MeshRenderer.material.color = startCol;
        }
        a = !a;
    }


    public override void Update()
    {
        // Interact with subitems before the whole item
        if (Physics.Raycast(itemCamera.ScreenPointToRay(Input.mousePosition), 100, itemPropertyLayer) && Input.GetMouseButtonDown(0))
        {
            Debug.Log("I FEEL SO SIGMA!");
        }
        else
        {
            base.Update();
        }
    }
}
