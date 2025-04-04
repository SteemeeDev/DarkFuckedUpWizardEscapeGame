using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Sits on layer 7, as an itemproperty
public class BookCirclePiece : MonoBehaviour
{
    public bool activated = false;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public SpriteRenderer spriteRenderer;
    public Color startCol;
    Vector3 startPos;

    [Range(1, 8)] public int index; // Index for solving puzzle
    private void Start()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        startCol = skinnedMeshRenderer.material.color;
        startPos = transform.parent.localPosition;
        spriteRenderer = transform.parent.GetComponentInChildren<SpriteRenderer>();

        Debug.Log(spriteRenderer.gameObject.name);
    }
    public void Click()
    {
        if (activated)
        {
            spriteRenderer.color = Color.black; 
            StartCoroutine(pressAnim(0.3f));
        }

        else
        {
            spriteRenderer.color = Color.blue * 0.9f;
            StartCoroutine(pressAnim(0.3f));
        }
        
        activated = !activated;
    }

    IEnumerator pressAnim(float animTime)
    {
        float elapsed = 0;
        while (elapsed < animTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animTime;
            if (!activated) t *= -1;
            transform.parent.localPosition = startPos + transform.forward * (t * 0.01f);
            yield return null;
        }

       // if (!activated) transform.localPosition = startPos;
    }
}
