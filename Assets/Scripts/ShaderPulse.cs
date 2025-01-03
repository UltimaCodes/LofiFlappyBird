using UnityEngine;

public class ShaderPulse : MonoBehaviour
{
    public Material backgroundMaterial;
    public AudioVisualizer visualizer;

    [Range(0.1f, 5f)] public float smoothSpeed = 2f;
    private float smoothedBassIntensity = 0f;
    private float smoothedIsBeat = 0f;

    void Update()
    {
        smoothedBassIntensity = Mathf.Lerp(smoothedBassIntensity, visualizer.BassIntensity, Time.deltaTime * smoothSpeed);

        float targetIsBeat = visualizer.IsBeat ? 1f : 0f;
        smoothedIsBeat = Mathf.Lerp(smoothedIsBeat, targetIsBeat, Time.deltaTime * smoothSpeed);

        backgroundMaterial.SetFloat("_BassIntensity", smoothedBassIntensity);
        backgroundMaterial.SetFloat("_IsBeat", smoothedIsBeat);
    }
}