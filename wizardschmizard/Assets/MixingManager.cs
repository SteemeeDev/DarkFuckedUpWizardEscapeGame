using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class MixingManager : MonoBehaviour
{
    public bool active;

    [SerializeField] InventoryManager inventory;
    [SerializeField] DialogueManager dialogueManger;
    [SerializeField] InvItem bookItem;
    [SerializeField] GameObject bookOnTable;
    [SerializeField] Image ingredientImage;
    [SerializeField] CauldronLiquid liquid;

    public List<MixingIngredient> ingredients = new();
    public int maxIngredients = 10;

    const int ingredientLayer = 1 << 16;
    RaycastHit hit;

    Camera mainCam;
    private void Start()
    {
        mainCam = Camera.main;
    }

    bool dragging = false;
    MixingIngredient ingredient;
    // Update is called once per frame
    void Update()
    {
        if (!active) return;

        foreach (GameObject item in inventory.items)
        {
            MenuInvItem menuInvItem = item.GetComponent<MenuInvItem>();
            if (menuInvItem.invItem == bookItem)
            {
               // StartCoroutine(dialogueManger.ReadDialogue("You place down the book...", 0));
                inventory.RemoveItem(bookItem);
                bookOnTable.SetActive(true);
                return;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit, 100, ingredientLayer))
            {
                if (!hit.transform.CompareTag("MixingPot"))
                {
                    dragging = true;
                    ingredient = hit.transform.GetComponent<MixingIngredient>();
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
            ingredientImage.enabled = false;

            if (!(ingredients.Count >= maxIngredients))
            {
                if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit, 100, ingredientLayer))
                {
                    if (hit.transform != null && hit.transform.CompareTag("MixingPot") && ingredient != null)
                    {
                        Debug.Log($"Added {ingredient.name} to the pot!");
                        ingredients.Add(ingredient);
                        liquid.fill = (float)ingredients.Count / (float)maxIngredients;
                        liquid.UpdateLiquid();
                    }
                }
            }
        }

        if (dragging)
        {
            ingredientImage.enabled = true;
            ingredientImage.sprite = ingredient._sprite;
            ingredientImage.transform.position = Input.mousePosition;
        }
    }
}
