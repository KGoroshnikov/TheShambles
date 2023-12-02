using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode, ImageEffectAllowedInSceneView, RequireComponent(typeof(Camera))]
public class DitherEffect : MonoBehaviour
{
    public Material ditherMat;
    [Range(0.0f, 1.0f)]
    public float ditherStrength = 0.1f;
    [Range(1, 32)]
    public int colorDepth = 4;
    [Space]
    [Range(0.0f, 1.0f)]
    public float lsd = 0f;


    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        ditherMat.SetFloat("_DitherStrength", ditherStrength * 0.1f);
        ditherMat.SetInt("_ColorDepth", colorDepth);
        ditherMat.SetFloat("_lsd", lsd);
        Graphics.Blit(src, dest, ditherMat);
    }
}