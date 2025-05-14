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
    [SerializeField] float minPointDistance;
    [SerializeField] float maxPointDistance;
    [SerializeField] float minOverlapDistance;

    [SerializeField] LineEnd[] lineEnds;

    const int itemLayer = 1 << 6;         // Circle layer
    const int itemPropertyLayer = 1 << 7; // LineEnd segments

    Transform currentCyl;

    [Space(10)]
    InventoryManager inventoryManager;
    [SerializeField] InvItem NextItem;

    Coroutine drawer;
    private void Awake()
    {
        lineEnds = transform.GetComponentsInChildren<LineEnd>();

        itemCam = GameObject.FindGameObjectWithTag("ItemCamera").GetComponent<Camera>();

        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (Physics.Raycast(itemCam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 999))
            {
                LineEnd hitLineEnd = hit.transform.GetComponent<LineEnd>();
                if (hitLineEnd != null)
                {
                    drawer = StartCoroutine(DrawLine(lineRenderers[(int)hitLineEnd._lineColor], true, Mathf.Infinity));

                    foreach (LineEnd lineEnd in lineEnds)
                    {
                        if (lineEnd == hitLineEnd)
                        {
                            lineEnd.connected = true;
                            continue;
                        }
                        if (lineEnd._lineColor == hitLineEnd._lineColor)
                        {
                            lineEnd.connected = false;
                        }
                    }
                }
                else
                {
                    foreach (LineRenderer lr in lineRenderers)
                    {

                        LineShake lineShake = lr.GetComponent<LineShake>();

                        if (lineShake == null ||
                            lineShake.points.Count <= 0 ||
                            lineShake.lineColor == LineEnd.LineColor.Gray)
                        {
                            continue;
                        }
     

                        float pointMoveDelta =
                            (transform.TransformPoint(lineShake.points.ElementAt(lineShake.points.Count - 1))
                            - (hit.point + hit.normal * 0.01f)).magnitude;


                        if (pointMoveDelta <= maxPointDistance)
                        {
                            Debug.Log(pointMoveDelta);
                            drawer = StartCoroutine(DrawLine(lr, false, pointMoveDelta * 1.2f));
                        }
                            
                    }

                }
            }

        }
        else if (Input.GetMouseButtonUp(0)) {
            StopCoroutine(drawer);
            if (PuzzleFinished())
            {
                inventoryManager.RemoveItem(inventoryManager.shownInvItem);
                inventoryManager.AddItem(NextItem);
                inventoryManager.ShowObject(NextItem);
            }
            Debug.Log($"Finished puzzle: {PuzzleFinished()}");
        }
    }

    IEnumerator DrawLine(LineRenderer lr, bool shouldClear, float pointMoveDelta)
    {
        // Componont that handles storing the position data
        LineShake lineShake = lr.transform.GetComponent<LineShake>();

        if (shouldClear)
        {
            lineShake.points.Clear();
        }

        while (Physics.Raycast(itemCam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            if (lineShake.points.Count > 0)
            {
                pointMoveDelta =
                    (transform.TransformPoint(lineShake.points.ElementAt(lineShake.points.Count - 1))
                    - (hit.point + hit.normal * 0.01f)).magnitude;
            }

            LineEnd _lineEnd = hit.transform.GetComponent<LineEnd>();
            if (_lineEnd != null)
            {
                if (_lineEnd._lineColor == lineShake.lineColor)
                {
                    hit.transform.GetComponent<LineEnd>().connected = true;
                }
                else
                {
                    StopCoroutine(drawer);
                    StartCoroutine(ClearLine(lineShake, 1f, 0.25f));
                }
            }

            // Check if point has moved enough
            if (pointMoveDelta > minPointDistance)
            {
                // Debug.Log("PointMoveDelta: " + pointMoveDelta);

                // TODO: Consider if we should always run this top statement
                if(pointMoveDelta > maxPointDistance && lineShake.points.Count > 0)
                {
                 //   Debug.LogWarning("Mouse moved too fast!");

                    Vector2 prevPointScreenPos = itemCam.WorldToScreenPoint(transform.TransformPoint(lineShake.points.ElementAt(lineShake.points.Count - 1)));
                    Vector2 currrentPointScreenPos = itemCam.WorldToScreenPoint(hit.point + hit.normal * 0.01f);

                    Vector2 mouseLerpPos;

                    float lerpPoints = (int)(pointMoveDelta / (minPointDistance));
                 //   Debug.Log($"Placing {lerpPoints} points");

                    for (int i = 0; i <= lerpPoints; i++)
                    {
                        mouseLerpPos = Vector2.Lerp(prevPointScreenPos, currrentPointScreenPos, (float)i / lerpPoints);

                        Debug.DrawRay(itemCam.ScreenPointToRay(mouseLerpPos).origin, itemCam.ScreenPointToRay(mouseLerpPos).direction, Color.blue, 20);
                        if (Physics.Raycast(itemCam.ScreenPointToRay(mouseLerpPos), out RaycastHit _hit))
                        {
                            lineShake.points.Add(transform.InverseTransformPoint(_hit.point + _hit.normal * 0.01f));

                            CheckOverlap((_hit.point + _hit.normal * 0.01f), lr);
                        }

                    }
                }
                else
                {
                    lineShake.points.Add(transform.InverseTransformPoint(hit.point + hit.normal * 0.01f));

                    CheckOverlap((hit.point + hit.normal * 0.01f), lr);
                }

            }

            yield return new WaitForFixedUpdate();
        }
       
    }

    (float, int) DistanceToClosestPoint(List<Vector3> pointList, Vector3 targetPoint)
    {
        float distance = Mathf.Infinity;
        int index = int.MaxValue;

        for (int i = pointList.Count - 1; i >= 0; i--)
        {
            float pointDistance = Vector3.Distance(pointList[i], targetPoint);
            //  Debug.Log(pointDistance);
            if (pointDistance < distance)
            {
                distance = pointDistance;
                index = i;
            }
        }


        return (distance, index);
    }

    void CheckOverlap(Vector3 point, LineRenderer lr)
    {
        for (int i = 0; i < lineRenderers.Length; i++)
        {
            LineRenderer _lr = lineRenderers[i];
            if (_lr == lr ) continue;
            LineShake line = _lr.GetComponent<LineShake>();

            (float, int) closestPoint = DistanceToClosestPoint(
                line.points,
                transform.InverseTransformPoint(point));

        //    Debug.Log("Distance to " + _lr.transform.name + ": \n" + schmistance);

            if (closestPoint.Item1 < minOverlapDistance)
            {
                if (line.lineColor == LineEnd.LineColor.Gray)
                {
                   // Debug.Log("CLEARING " + lr.name + " CAUSE: point at " + point);
                   // lr.GetComponent<LineShake>().points.Clear();
                   // lr.positionCount = 0;
                    StartCoroutine(ClearLine(lr.GetComponent<LineShake>(), 1.0f, 0.25f));

                    foreach (LineEnd lineEnd in lineEnds)
                    {
                        if (lineEnd._lineColor == lr.GetComponent<LineShake>().lineColor)
                        {
                            lineEnd.connected = false;
                        }
                    }

                    StopCoroutine(drawer);
                }
                else
                {
                    //Debug.Log("CLEARING " + _lr.name);
                    line.points.Clear();
                    _lr.positionCount = 0;


                    foreach (LineEnd lineEnd in lineEnds)
                    {
                        if (lineEnd._lineColor == line.lineColor)
                        {
                            lineEnd.connected = false;
                        }
                    }
                }


            }
        }
    }

    IEnumerator ClearLine(LineShake line, float time, float waitingTime)
    {
        float stepTime =  time / (float)line.points.Count;

        yield return new WaitForSeconds(waitingTime);

        while (line.points.Count > 0)
        {
            line.points.RemoveAt(line.points.Count - 1);
            yield return new WaitForSeconds(stepTime - Time.deltaTime);
        }

        line.points.Clear();
        line.lineRenderer.positionCount = 0;
    }

    bool PuzzleFinished()
    {
        foreach (LineEnd lineEnd in lineEnds)
        {
            if (!lineEnd.connected) return false;
        }
        return true;
    }
}
