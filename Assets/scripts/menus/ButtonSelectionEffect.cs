using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSelectionEffect : MonoBehaviour
{
    private Vector3 targetScale;
    private Vector3 startScale;
    [SerializeField] private float scaleSpeed = 10f; // Speed of the scaling transition

    private bool isPointerOver = false;

    void Start()
    {
        startScale = transform.localScale;
        targetScale = transform.localScale * 1.1f;
    }

    void Update()
    {
        // Smoothly scale towards the target scale based on pointer state
        if (isPointerOver)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, startScale, Time.deltaTime * scaleSpeed);
        }
    }

    public void OnPointerEnter()
    {
        isPointerOver = true; // Set pointer state
    }

    public void OnPointerExit()
    {
        isPointerOver = false; // Reset pointer state
    }
}
