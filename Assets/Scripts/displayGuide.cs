using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class displayGuide : MonoBehaviour
{
    public GameObject guide;
    public GrabObejct grabObject;

    void Start()
    {
        guide.SetActive(false);
    }

    private void Update()
    {
        if (!grabObject.isHolding)
        {
            guide.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (grabObject.isHolding)
            {
                guide.SetActive(true);
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