using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBook : MonoBehaviour
{
    SkinnedMeshRenderer m_SkinnedMeshRenderer;
    [SerializeField] float openTime = 1;


    // Start is called before the first frame update
    IEnumerator Start()
    {
        m_SkinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
       
        float elapsed = 0;
        while (elapsed < openTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / openTime; 
            m_SkinnedMeshRenderer.SetBlendShapeWeight(0, (1.0f-t)*100.0f);
            yield return null;
        }   
    }
}
