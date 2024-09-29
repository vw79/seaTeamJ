using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanityBar : MonoBehaviour
{
    public GameObject jumpscare; // Assign your jumpscare object here
    public GameObject gameOverUI; // Assign your game over UI here
    public RectTransform sanityBar;
    public float decreaseDuration = 10f;

    private float initialWidth;
    private float timePassed;
    private bool isGameOver;

    void Start()
    {
        initialWidth = sanityBar.sizeDelta.x;
        isGameOver = false;
    }

    void Update()
    {
        if (!isGameOver)
        {
            if (timePassed < decreaseDuration)
            {
                timePassed += Time.deltaTime;
                float newWidth = Mathf.Lerp(initialWidth, 0, timePassed / decreaseDuration);
                sanityBar.sizeDelta = new Vector2(newWidth, sanityBar.sizeDelta.y);

                if (newWidth <= 0)
                {
                    PlayerDead(); // Trigger game over if sanity bar is depleted
                }
            }
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

    private void PlayerDead()
    {
        isGameOver = true;


        jumpscare.SetActive(true);
        StartCoroutine(DisableIMG());
    }

    IEnumerator DisableIMG()
    {
        yield return new WaitForSeconds(2);
        jumpscare.SetActive(false);
        gameOverUI.SetActive(true);
    }
}
