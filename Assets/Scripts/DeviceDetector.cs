using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceDetector : MonoBehaviour
{
    public GameObject mobileControls;

    void Start()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.Android)
        {
            mobileControls.SetActive(true);
        }
        else if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                mobileControls.SetActive(true);
            }
            else if (SystemInfo.deviceType == DeviceType.Desktop)
            {
                mobileControls.SetActive(false);
            }
        }
        else
        {
            mobileControls.SetActive(false);
        }
    }
}