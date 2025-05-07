using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CauldronLiquid : MonoBehaviour
{
    [Range(0, 1.0f)] public float fill;
    [SerializeField] float scale;
    [SerializeField, HideInInspector] AnimationCurve scaleCurve; // Unused
    [SerializeField] ParticleSystem particles;

    Vector3 startPos;
    Vector3 startScale;

    [SerializeField] Transform topPos;

    MeshRenderer _renderer;
    Color startCol;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();

        startPos = transform.position;
        startScale = transform.localScale;
    }

    public void UpdateLiquid(Color color)
    {
        if (_renderer != null) _renderer.enabled = true;

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

        _renderer.material.color = Color.Lerp(_renderer.material.color, color, 0.5f);

        _renderer.material.color = new Color(
            _renderer.material.color.r,
            _renderer.material.color.g, 
            _renderer.material.color.b, 
            0
            );

        _renderer.material.color += new Color(0, 0, 0, 101.0f/255.0f);
    }

    public IEnumerator Fail(float animTime)
    {
        particles.Play();

        float elapsed = 0;

        while (elapsed <= animTime)
        {
            elapsed += Time.deltaTime;
            float t = 1.0f - elapsed / animTime;
            fill = t;
            UpdateLiquid(startCol);
            yield return null;
        }

        if (_renderer != null) _renderer.enabled = false;
        
        particles.Stop();

        _renderer.material.color = startCol;
    }
}
