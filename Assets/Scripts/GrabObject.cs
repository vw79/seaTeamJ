using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrabObejct : MonoBehaviour
{
    public PlayerController PlayerController;

    public Transform holdPoint;
    public GameObject objectToGrab;

    public bool isHolding = false;
    public bool inRange = false;
    public bool canDeposit = false;
    public bool canDepositToSanity = false;
    public float increaseSanityValue = 10f;

    private int depositTime = 2;
    private float holdTime = 0.0f;

    // Generator light indicators
    public List<SpriteRenderer> generatorLights; // Updated to use SpriteRenderer for sprites
    private int depositCount = 0;

    // Reference to door
    public GameObject door;
    public bool isDoorUnlocked;

    // Reference to sanity bar
    public SanityBar sanityBar;

    // Reference to win menu
    public GameObject winMenu;

    // References for the holding bar UI
    public RectTransform holdingBarFill; // Using RectTransform for resizing the fill bar
    public Image holdingBarBackground; // Reference to the background bar
    private float initialWidth;

    // Reference for deposit indication UI images
    public List<Image> indicatorCircles; // Add your four indicator circles here in the inspector

    // Reference to HolePunchController
    public HolePunchController holePunchController;

    private float originalHoleRadius;
    private float originalHoleFeather;
    private float originalOverlayAlpha;

    void Start()
    {
        initialWidth = holdingBarFill.sizeDelta.x;
        holdingBarFill.sizeDelta = new Vector2(0, holdingBarFill.sizeDelta.y); // Start with empty bar
        holdingBarBackground.gameObject.SetActive(false); // Hide the background at the start

        // Store the original values of HolePunchController
        if (holePunchController != null)
        {
            originalHoleRadius = holePunchController.holeRadius;
            originalHoleFeather = holePunchController.holeFeather;
            originalOverlayAlpha = holePunchController.overlayAlpha;
        }
    }

    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.F))
        {
            if (!isHolding)
            {
                GrabObject();
            }
        }

        if ((canDeposit || canDepositToSanity) && Input.GetKey(KeyCode.F))
        {
            holdTime += Time.deltaTime;
            PlayerController.canMove = false;

            // Show and update the holding bar and its background
            holdingBarBackground.gameObject.SetActive(true);
            holdingBarFill.gameObject.SetActive(true);
            float newWidth = Mathf.Lerp(0, initialWidth, holdTime / depositTime);
            holdingBarFill.sizeDelta = new Vector2(newWidth, holdingBarFill.sizeDelta.y);

            if (holdTime >= depositTime)
            {
                if (canDeposit)
                {
                    DepositObject(); // Unlock the door
                }
                else if (canDepositToSanity)
                {
                    DepositToSanityGenerator(); // Increase sanity bar
                    StartCoroutine(ChangeHolePunchValues());
                }

                PlayerController.canMove = true;
                objectToGrab.SetActive(false);
                holdTime = 0.0f;
                holdingBarFill.sizeDelta = new Vector2(0, holdingBarFill.sizeDelta.y); // Reset the bar
                holdingBarFill.gameObject.SetActive(false);
                holdingBarBackground.gameObject.SetActive(false);
            }
        }
        else
        {
            PlayerController.canMove = true;
            holdTime = 0.0f; // Reset hold time if key is released
            holdingBarFill.sizeDelta = new Vector2(0, holdingBarFill.sizeDelta.y);
            holdingBarFill.gameObject.SetActive(false);
            holdingBarBackground.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Resource"))
        {
            inRange = true;
            objectToGrab = other.gameObject;
        }

        if (other.CompareTag("Generator") && isHolding)
        {
            canDeposit = true;
        }

        if (other.CompareTag("Light") && isHolding) // For increasing sanity
        {
            canDepositToSanity = true;
        }

        if (other.CompareTag("Door") && isDoorUnlocked)
        {
            ShowWinMenu();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Resource"))
        {
            inRange = false;
            objectToGrab = null;
        }

        if (other.CompareTag("Generator"))
        {
            canDeposit = false;
        }

        if (other.CompareTag("Light"))
        {
            canDepositToSanity = false;
        }
    }

    private void GrabObject()
    {
        if (objectToGrab != null)
        {
            objectToGrab.transform.position = holdPoint.position; // Move object to hold position
            objectToGrab.transform.SetParent(holdPoint); // Make object a child of holdPoint
            objectToGrab.GetComponent<Rigidbody2D>().isKinematic = true; // Disable physics while holding
            isHolding = true;
        }
    }

    private void DepositObject()
    {
        canDeposit = false;
        isHolding = false;
        Debug.Log("Deposit Complete for Unlocking");

        // Activate generator emission light
        if (depositCount < generatorLights.Count)
        {
            // Convert HSV (Hue 120 for green, Saturation 1, Value 1) to RGB
            Color emissionColor = Color.HSVToRGB(120f / 360f, 1f, 1f);

            // Multiply by intensity to set the emission color
            emissionColor *= 3.80065f;

            // Set the emission color for the material
            Material material = generatorLights[depositCount].material;
            material.SetColor("_Color", emissionColor);

            depositCount++;
        }

        // Update the indicator circles UI
        if (depositCount - 1 < indicatorCircles.Count)
        {
            indicatorCircles[depositCount - 1].color = Color.green; // Change the color to green
        }

        // Unlock door after all generators are activated
        if (depositCount == generatorLights.Count)
        {
            isDoorUnlocked = true;
            Debug.Log("All generators activated. Door unlocked.");
        }
    }

    private void DepositToSanityGenerator()
    {
        // Increase sanity bar
        if (sanityBar != null)
        {
            sanityBar.IncreaseSanity(increaseSanityValue);
        }

        canDepositToSanity = false;
        isHolding = false;
        Debug.Log("Deposit Complete for Increasing Sanity");
    }

    private void ShowWinMenu()
    {
        if (winMenu != null)
        {
            winMenu.SetActive(true);
            Debug.Log("Win Menu enabled.");
        }
    }

    private IEnumerator ChangeHolePunchValues()
    {
        if (holePunchController != null)
        {
            // Change the HolePunchController values
            holePunchController.holeRadius = 220f;
            holePunchController.holeFeather = 300f;
            holePunchController.overlayAlpha = 0.75f;

            yield return new WaitForSeconds(10f);

            // Revert back to the original values
            holePunchController.holeRadius = originalHoleRadius;
            holePunchController.holeFeather = originalHoleFeather;
            holePunchController.overlayAlpha = originalOverlayAlpha;
        }
    }
}