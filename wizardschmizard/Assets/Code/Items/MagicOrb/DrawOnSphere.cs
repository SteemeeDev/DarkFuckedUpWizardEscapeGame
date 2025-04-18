using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows.Speech;

public class DrawOnSphere : MonoBehaviour
{
    Camera itemCam;
    [SerializeField] LineRenderer[] lineRenderers;
    [SerializeField] Transform[] cylinders;
    [SerializeField] float minPointDistance;
    [SerializeField] float minOverlapDistance;
    [SerializeField] float maxMouseMoveDistance;


    Transform currentCyl;


    private void Awake()
    {
        foreach (Transform cyl in cylinders)
        {
            cyl.localPosition = Random.onUnitSphere / 2;
            cyl.up = (transform.position - cyl.position).normalized;
        }

        itemCam = GameObject.FindGameObjectWithTag("ItemCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (Physics.Raycast(itemCam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                for (int i = 0; i < cylinders.Length; i++)
                {
                    if (cylinders[i] == hit.transform)
                    {
                        StartCoroutine(drawLine(lineRenderers[i]));
                    }
                }
     
            }

        }
        else if (Input.GetMouseButtonUp(0)) {
            StopAllCoroutines();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (Transform cyl in cylinders)
            {
                cyl.localPosition = Random.onUnitSphere / 2;
                cyl.up = (transform.position - cyl.position).normalized;
            }

            foreach (LineRenderer lineRenderer in lineRenderers)
            {
                lineRenderer.GetComponent<LineShake>().points.Clear();
                lineRenderer.positionCount = 0;
            }
        }
    }

    IEnumerator drawLine(LineRenderer lr)
    {
        LineShake lineShake = lr.transform.GetComponent<LineShake>();
        float pointMoveDelta = Mathf.Infinity;

      
        Vector3 prevMousePos = Input.mousePosition;
        float mouseDelta;

        bool firstRun = true;

        while (Physics.Raycast(itemCam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
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
                        - (hit.point + hit.normal * 0.01f)).magnitude;
                }
            }
            
            mouseDelta = (Input.mousePosition - prevMousePos).magnitude;

            // Place points between if mouse moves too fast
            if (mouseDelta > maxMouseMoveDistance)
            {
                Debug.LogWarning("Mouse moved too fast!");

                Vector3 mouseLerpPos;

                float lerpPoints = (int)(mouseDelta * 0.25f);
                //Debug.Log(lerpPoints);

                for (int i = 0; i <= lerpPoints; i++)
                {
                    mouseLerpPos = Vector2.Lerp(prevMousePos, Input.mousePosition, (float)i / lerpPoints);
                    Debug.DrawRay(itemCam.ScreenPointToRay(mouseLerpPos).origin, itemCam.ScreenPointToRay(mouseLerpPos).direction, Color.blue, 6);
                    if (Physics.Raycast(itemCam.ScreenPointToRay(mouseLerpPos), out RaycastHit _hit)){
                        lr.positionCount = lineShake.points.Count;
                        lineShake.points.Add(transform.InverseTransformPoint(_hit.point + _hit.normal * 0.01f));

                        CheckOverlap((_hit.point + _hit.normal * 0.01f), lr);
                    }

                }
            }

            prevMousePos = Input.mousePosition;
            // Check if point has moved enough
            if (pointMoveDelta > minPointDistance)
            {
                lr.positionCount = lineShake.points.Count;
                lineShake.points.Add(transform.InverseTransformPoint(hit.point + hit.normal * 0.01f));
                CheckOverlap((hit.point + hit.normal * 0.01f), lr);
            }

            yield return null;
        }
       
    }

    float DistanceToClosestPoint(List<Vector3> pointList, Vector3 targetPoint)
    {
        float distance = Mathf.Infinity;

        for (int i = pointList.Count - 1; i >= 0; i--)
        {
            float pointDistance = Vector3.Distance(pointList[i], targetPoint);
          //  Debug.Log(pointDistance);
            if (pointDistance < distance) distance = pointDistance;
        }


        return distance;
    }

    
    void CheckOverlap(Vector3 point, LineRenderer lr)
    {
        foreach (LineRenderer _lr in lineRenderers)
        {
            if (_lr == lr ) continue;
            

            float closestPoint =
                DistanceToClosestPoint(_lr.GetComponent<LineShake>().points,
            transform.InverseTransformPoint(point));

        //    Debug.Log("Distance to " + _lr.transform.name + ": \n" + schmistance);

            if (closestPoint < minOverlapDistance)
            {
                _lr.GetComponent<LineShake>().points.Clear();
                _lr.positionCount = 0;
            }
        }
    }
}
