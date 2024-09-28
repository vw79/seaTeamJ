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
}
