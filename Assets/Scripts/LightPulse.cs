using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPulse : MonoBehaviour
{
    public Light sceneLight;
    public AudioVisualizer visualizer;

    public float minIntensity = 0.5f;
    public float maxIntensity = 2f;
    public float smoothSpeed = 5f;
    public Color beatColor = Color.red;

    private Color originalColor;

    void Start()
    {
        originalColor = sceneLight.color;
    }

    // Update is called once per frame
    void Update()
    {
        float targetIntensity = Mathf.Lerp(minIntensity, maxIntensity, visualizer.BassIntensity);
        sceneLight.intensity = Mathf.Lerp(sceneLight.intensity, targetIntensity, smoothSpeed * Time.deltaTime);

        if (visualizer.IsBeat)
        {
            sceneLight.color = Color.Lerp(sceneLight.color, beatColor, Time.deltaTime * smoothSpeed);
        }
        else
        {
            sceneLight.color = Color.Lerp(sceneLight.color, originalColor, Time.deltaTime * smoothSpeed);
        }
    }
}
