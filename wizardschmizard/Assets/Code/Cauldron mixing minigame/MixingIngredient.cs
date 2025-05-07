using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixingIngredient : MonoBehaviour
{
    public enum Ingredient
    {
        WizardEggs,
        Cocoa,
        Sugar,
        Flour,
        Watuh
    }

    
    public Ingredient _ingredient;
    public Sprite _sprite;
    [ColorUsage(true, true)] public Color _color;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
