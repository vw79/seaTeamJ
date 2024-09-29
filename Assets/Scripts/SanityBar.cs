using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanityBar : MonoBehaviour
{
    public RectTransform sanityBar;
    public float decreaseDuration = 10f;

    private float initialWidth;
    private float timePassed;

    void Start()
    {
        initialWidth = sanityBar.sizeDelta.x;
    }

    void Update()
    {
        if (timePassed < decreaseDuration)
        {
            timePassed += Time.deltaTime;
            float newWidth = Mathf.Lerp(initialWidth, 0, timePassed / decreaseDuration);
            sanityBar.sizeDelta = new Vector2(newWidth, sanityBar.sizeDelta.y);
        }
    }

    public void IncreaseSanity(float amount)
    {
        // Reset or increase sanity bar
        timePassed -= amount;
        if (timePassed < 0)
        {
            timePassed = 0; // Ensure we don't go below 0
        }

        // Update the sanity bar size immediately
        float newWidth = Mathf.Lerp(initialWidth, 0, timePassed / decreaseDuration);
        sanityBar.sizeDelta = new Vector2(newWidth, sanityBar.sizeDelta.y);
    }
}