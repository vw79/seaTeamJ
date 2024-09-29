using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObejct : MonoBehaviour
{
    public PlayerController PlayerController;

    public Transform holdPoint;
    public GameObject objectToGrab;

    public bool isHolding = false;
    public bool inRange = false;
    public bool canDeposit = false;

    private int depositTime = 2;
    private float holdTime = 0.0f;

    // Generator light indicators
    public List<SpriteRenderer> generatorLights; // Updated to use SpriteRenderer for sprites
    private int depositCount = 0;

    // Reference to door
    public GameObject door;
    public bool isDoorUnlocked;

    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.F))
        {
            if (!isHolding)
            {
                GrabObject();
            }
        }

        if (canDeposit && Input.GetKey(KeyCode.F))
        {
            holdTime += Time.deltaTime;
            PlayerController.canMove = false;
            if (holdTime >= depositTime)
            {
                DepositObject();
                PlayerController.canMove = true;
                objectToGrab.SetActive(false);
                holdTime = 0.0f;
            }
        }
        else
        {
            PlayerController.canMove = true;
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
        Debug.Log("Deposit Complete");

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

        // Unlock door after all generators are activated
        if (depositCount == generatorLights.Count)
        {
            isDoorUnlocked = true;
            Debug.Log("All generators activated. Door unlocked.");
        }
    }
}