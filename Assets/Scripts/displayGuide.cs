using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class displayGuide : MonoBehaviour
{
    public GameObject guide;
    public GrabObejct grabObject;

    private string objectTag;

    void Start()
    {
        guide.SetActive(false);
        objectTag = gameObject.tag;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Ensures that the guide is shown only if player is in range and conditions are met
        if (collision.gameObject.tag == "Player")
        {
            if (objectTag == "Generator" && grabObject.isHolding)
            {
                guide.SetActive(true);
            }
            else if (objectTag == "Door" && grabObject.isDoorUnlocked)
            {
                guide.SetActive(true);
            }
            else
            {
                guide.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            guide.SetActive(false);
        }
    }
}