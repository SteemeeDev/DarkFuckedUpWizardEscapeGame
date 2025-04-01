using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBook : MonoBehaviour
{
    SkinnedMeshRenderer m_SkinnedMeshRenderer;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        m_SkinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        for (int i = 100; i > 0; i--)
        {
            i += (int)(Time.deltaTime % 0.005f);
            m_SkinnedMeshRenderer.SetBlendShapeWeight(0, i);
            yield return new WaitForSeconds(0.005f);
        }
        
    }
}
