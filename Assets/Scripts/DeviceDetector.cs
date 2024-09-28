using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceDetector : MonoBehaviour
{
    void Start()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("Running on a mobile device.");
        }
        else if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                Debug.Log("Running on a mobile browser.");
            }
            else if (SystemInfo.deviceType == DeviceType.Desktop)
            {
                Debug.Log("Running on a desktop browser.");
            }
        }
        else
        {
            Debug.Log("Running on a desktop or other device.");
        }
    }
}