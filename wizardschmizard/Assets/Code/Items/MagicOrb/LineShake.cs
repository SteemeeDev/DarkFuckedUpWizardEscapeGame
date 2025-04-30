using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class LineShake : MonoBehaviour
{
    [SerializeField] DrawOnSphere DrawOnSphere;
    [SerializeField] float shakeMagnitude = 0.015f;
    [SerializeField] bool shake;
    public LineEnd.LineColor lineColor;
    public LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public List<Vector3> points;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (points.Count > 0)
        {
            if (points.Count != lineRenderer.positionCount) lineRenderer.positionCount = points.Count;

            if (shake)
            {
                for (int i = 0; i < lineRenderer.positionCount; i++)
                {
                    Vector3 randomPos = points[i] +
                        new Vector3(
                            Random.Range(-shakeMagnitude, shakeMagnitude),
                            Random.Range(-shakeMagnitude, shakeMagnitude),
                            Random.Range(-shakeMagnitude, shakeMagnitude)
                        );

                    lineRenderer.SetPosition(i, randomPos);
                }
            }
            else
            {
                for (int i = 0; i < lineRenderer.positionCount; i++)
                {
                    Vector3 fixedPos = points[i];
                    lineRenderer.SetPosition(i, fixedPos);
                }
            }

        }
    }
}
