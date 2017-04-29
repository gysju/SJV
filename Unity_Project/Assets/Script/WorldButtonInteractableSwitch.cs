using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldButtonInteractableSwitch : MonoBehaviour
{
    Button button;
    BoxCollider boxCollider;

    // Use this for initialization
    void Start()
    {
        button = GetComponent<Button>();
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (button.IsInteractable())
        {
            boxCollider.enabled = true;
        }
        else
        {
            boxCollider.enabled = false;
        }

    }
}
