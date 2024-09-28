using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolePunchController : MonoBehaviour
{
    public Material fadeMaterial;
    public Transform playerTransform;

    public float holeRadius = 0.3f;
    public float holeFeather = 0.2f;
    public Color overlayColor = Color.black;
    [Range(0, 1)] public float overlayAlpha = 1.0f;

    void Update()
    {
        Vector3 playerWorldPos = playerTransform.position;

        fadeMaterial.SetVector("_HoleCenter", new Vector4(playerWorldPos.x, playerWorldPos.y, 0, 0));
        fadeMaterial.SetFloat("_HoleRadius", holeRadius);
        fadeMaterial.SetFloat("_HoleFeather", holeFeather);
        fadeMaterial.SetColor("_OverlayColor", overlayColor);
        fadeMaterial.SetFloat("_OverlayAlpha", overlayAlpha);
    }
}