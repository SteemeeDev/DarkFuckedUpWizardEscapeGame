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
                    StartCoroutine(DrawLine(lineRenderers[(int)hitLineEnd._lineColor]));

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
            }

        }
        else if (Input.GetMouseButtonUp(0)) {
            StopAllCoroutines();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (PuzzleFinished())
            {
                inventoryManager.RemoveItem(inventoryManager.shownInvItem);
                inventoryManager.AddItem(NextItem);
                inventoryManager.ShowObject(NextItem);
            }
            Debug.Log($"Finished puzzle: {PuzzleFinished()}");

            /*
            foreach (LineRenderer lineRenderer in lineRenderers)
            {
                lineRenderer.GetComponent<LineShake>().points.Clear();
                lineRenderer.positionCount = 0;
            }*/
        }
    }

    IEnumerator DrawLine(LineRenderer lr)
    {
        // Componont that handles storing the position data
        LineShake lineShake = lr.transform.GetComponent<LineShake>();

        // How much the point we are trying to place has moved
        float pointMoveDelta = Mathf.Infinity;

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
            
            LineEnd _lineEnd = hit.transform.GetComponent<LineEnd>();
            if (_lineEnd != null)
            {
                if (_lineEnd._lineColor == lineShake.lineColor)
                {
                    hit.transform.GetComponent<LineEnd>().connected = true;
                }
                else
                {
                    StopAllCoroutines();
                }
            }

            // Check if point has moved enough
            if (pointMoveDelta > minPointDistance)
            {
                // Debug.Log("PointMoveDelta: " + pointMoveDelta);

                // TODO: Consider if we should always run this top statement
                if(pointMoveDelta > maxPointDistance && lineShake.points.Count > 0)
                {
                    Debug.LogWarning("Mouse moved too fast!");

                    Vector2 prevPointScreenPos = itemCam.WorldToScreenPoint(transform.TransformPoint(lineShake.points.ElementAt(lineShake.points.Count - 1)));
                    Vector2 currrentPointScreenPos = itemCam.WorldToScreenPoint(hit.point + hit.normal * 0.01f);

                    Vector2 mouseLerpPos;

                    float lerpPoints = (int)(pointMoveDelta / (minPointDistance));
                    Debug.Log($"Placing {lerpPoints} points");

                    for (int i = 0; i <= lerpPoints; i++)
                    {
                        mouseLerpPos = Vector2.Lerp(prevPointScreenPos, currrentPointScreenPos, (float)i / lerpPoints);

                        Debug.DrawRay(itemCam.ScreenPointToRay(mouseLerpPos).origin, itemCam.ScreenPointToRay(mouseLerpPos).direction, Color.blue, 20);
                        if (Physics.Raycast(itemCam.ScreenPointToRay(mouseLerpPos), out RaycastHit _hit))
                        {
                            lr.positionCount = lineShake.points.Count;
                            lineShake.points.Add(transform.InverseTransformPoint(_hit.point + _hit.normal * 0.01f));

                            CheckOverlap((_hit.point + _hit.normal * 0.01f), lr);
                        }

                    }
                }
                else
                {
                    lr.positionCount = lineShake.points.Count;
                    lineShake.points.Add(transform.InverseTransformPoint(hit.point + hit.normal * 0.01f));
                    CheckOverlap((hit.point + hit.normal * 0.01f), lr);
                }

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
        for (int i = 0; i < lineRenderers.Length; i++)
        {
            LineRenderer _lr = lineRenderers[i];
            if (_lr == lr ) continue;
            

            float closestPoint =
                DistanceToClosestPoint(_lr.GetComponent<LineShake>().points,
            transform.InverseTransformPoint(point));

        //    Debug.Log("Distance to " + _lr.transform.name + ": \n" + schmistance);

            if (closestPoint < minOverlapDistance)
            {
                if (_lr.GetComponent<LineShake>().lineColor == LineEnd.LineColor.Gray)
                {
                    lr.GetComponent<LineShake>().points.Clear();
                    lr.positionCount = 0;

                    foreach (LineEnd lineEnd in lineEnds)
                    {
                        if (lineEnd._lineColor == lr.GetComponent<LineShake>().lineColor)
                        {
                            lineEnd.connected = false;
                        }
                    }

                    StopAllCoroutines();
                }
                else
                {
                    _lr.GetComponent<LineShake>().points.Clear();
                    _lr.positionCount = 0;

                    foreach (LineEnd lineEnd in lineEnds)
                    {
                        if (lineEnd._lineColor == _lr.GetComponent<LineShake>().lineColor)
                        {
                            lineEnd.connected = false;
                        }
                    }
                }


            }
        }
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
