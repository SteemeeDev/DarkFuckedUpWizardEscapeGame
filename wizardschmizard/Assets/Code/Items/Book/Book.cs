using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using static UnityEngine.Rendering.DebugUI.Table;

public class Book : MonoBehaviour
{
    int[] correctOrder = { 1, 2, 3, 4, 5, 6, 7, 8 };
    List<int> order = new List<int>();
    [SerializeField] BookCirclePiece[] bookCirclePieces;
    [SerializeField] GameObject bookMiddlePiece;
    [SerializeField] Color correctEmissiveColor;
    [SerializeField] InvItem openBookItem;
    [SerializeField] InvItem closedBookItem;

    bool finishedOrder = false;
    
    public const int itemLayer = 1 << 6;
    public const int itemPropertyLayer = 1 << 7;

    Camera itemCamera;
    InventoryManager inventoryManager;
    
    Quaternion startRot;

    private void Awake()
    {
        itemCamera = GameObject.FindGameObjectWithTag("ItemCamera").GetComponent<Camera>();
        inventoryManager = FindObjectOfType<InventoryManager>();

        startRot = transform.rotation;
    }

    public void Update()
    {
        // Check if pattern is correct
        
        if (order.Count >= correctOrder.Length && !finishedOrder)
        {
            bool correct = true;

            for (int i = 0; i < order.Count; i++)
            {
                if (order[i] != correctOrder[i]) correct = false;
            }

            Debug.Log("CHOSEN ORDER: ");
            foreach (int i in order)
            {
                Debug.Log(i);
            }
            Debug.Log("CORRECT ORDER: ");
            foreach (int i in correctOrder)
            {
                Debug.Log(i);
            }

            if (!correct)
            {
                Debug.Log("WRONG!");
                order.Clear();
                foreach (var piece in bookCirclePieces)
                {
                    piece.Click();
                }
            }
            else if (finishedOrder != true)
            {
                finishedOrder = true;
                Debug.Log("RIGHT!");

                // This might break in the future - if startcolor actually gets used
                bookMiddlePiece.GetComponent<SkinnedMeshRenderer>().material.color *= correctEmissiveColor * 3;

                foreach (var piece in bookCirclePieces)
                {
                    piece.activated = true;
                    piece.skinnedMeshRenderer.material.color = piece.startCol * correctEmissiveColor * 3;
                    piece.spriteRenderer.color = Color.black;
                }

                StartCoroutine(FinishTurn());


                Debug.Log("Got through!");
            }


        }   
        // Interact with subitems before the whole item
        RaycastHit hit;
        
        if (Physics.Raycast(itemCamera.ScreenPointToRay(Input.mousePosition), out hit, 100, itemPropertyLayer) && Input.GetMouseButtonDown(0) && !finishedOrder)
        {
           // Debug.Log("Hit: " + hit.transform.name + " by " + this);
            BookCirclePiece piece = hit.transform.GetComponent<BookCirclePiece>();
            if (!piece.activated) order.Add(piece.index);
            else order.Remove(piece.index);
            
            piece.Click();

            Debug.Log("Piece index: " + piece.index);
        }
    }

    IEnumerator FinishTurn()
    {
        float elapsed = 0;
        float time = 1f;

        Quaternion currentRot = transform.rotation;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / time;

            transform.rotation = Quaternion.Lerp(currentRot, startRot, t);
            yield return null;
        }
        Quaternion rot = transform.rotation;

        inventoryManager.RemoveItem(closedBookItem);
        inventoryManager.AddItem(openBookItem);
        inventoryManager.ShowObject(openBookItem);

        inventoryManager.shownItem.transform.rotation = rot;
    }
}