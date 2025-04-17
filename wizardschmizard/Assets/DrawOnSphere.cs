using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DrawOnSphere : MonoBehaviour
{
    Camera itemCam;
    [SerializeField] LineRenderer[] lineRenderers;
    [SerializeField] float minPointDistance;

    int lineIndex = 0;

    private void Awake()
    {
        itemCam = GameObject.FindGameObjectWithTag("ItemCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("LineIndex: " + lineIndex);
            StartCoroutine(drawLine(lineRenderers[lineIndex]));
        }
        else if (Input.GetMouseButtonUp(0)) {
            StopAllCoroutines();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (lineIndex == 0) lineIndex = lineRenderers.Length - 1;
            else lineIndex--;
            //StopAllCoroutines();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (lineIndex >= lineRenderers.Length - 1) lineIndex = 0;
            else lineIndex++;
            //StopAllCoroutines();
        }
    }

    IEnumerator drawLine(LineRenderer lr)
    {
        LineShake lineShake = lr.transform.GetComponent<LineShake>();
        float pointMoveDelta = Mathf.Infinity;
        bool firstRun = true;


        while (true)
        {
            if (Physics.Raycast(itemCam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                if (firstRun)
                {
                    lineShake.points.Clear();
                    firstRun = false;
                }
                else
                {
                    if (lineShake.points.Count > 0)
                    {
                        pointMoveDelta =
                            (transform.TransformPoint(lineShake.points.ElementAt(lineShake.points.Count - 1))
                            - (hit.point)).magnitude;
                    }
                }

                //Debug.Log(pointMoveDelta);
                if (pointMoveDelta > minPointDistance)
                {
                    //Debug.Log("Hit sphere
                    lr.positionCount = lineShake.points.Count;
                    lineShake.points.Add(transform.InverseTransformPoint(hit.point));
                    //lr.SetPosition(positionCount - 1, transform.InverseTransformPoint((hit.point + hit.normal * 0.02f)));
                }
            }

            yield return new WaitForSeconds(0.02f);
        }
    }

}
