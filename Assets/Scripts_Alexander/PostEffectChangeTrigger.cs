using Cyan;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class PostEffectChangeTrigger : MonoBehaviour
{
    [SerializeField]
    private UniversalRendererData rendererData = null;
    [SerializeField]
    private string featureName = null;

    public int pixelSize = 6;
    public float colorDepth = 32;
    public float dithering = 0.0005f;
    public float lsd = 0;

    private Animation animation;
    private void Start()
    {
        animation = GetComponent<Animation>();
    }
    private void Update()
    {
        if (TryGetFeature(out var feature))
        {
            Blit blitFeature = feature as Blit;
            Material material = blitFeature.settings.blitMaterial;
            material.SetInt("_PixelSize", pixelSize);
            material.SetFloat("_ColorDepth", colorDepth);
            material.SetFloat("_DitherStrength", dithering);
            material.SetFloat("_LSD", lsd);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        animation.Play();
    }

    private bool TryGetFeature(out ScriptableRendererFeature feature)
    {
        feature = rendererData.rendererFeatures.Where((f) => f.name == featureName).FirstOrDefault();
        return feature != null;
    }


}
