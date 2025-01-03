using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisualizer : MonoBehaviour
{
    public AudioSource audioSource;
    private float[] spectrumData = new float[1024];
    private float[] historyBuffer = new float[43];
    private float bpm;
    private float lastBeatTime;
    private int beatCount;

    public float BassIntensity { get; private set; }
    public bool IsBeat { get; private set; }

    void Start()
    {
        bpm = 0;
        lastBeatTime = 0;
        beatCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.Blackman);

        BassIntensity = 0;
        for (int i = 0; i < 10; i++)
            BassIntensity += spectrumData[i];
        BassIntensity *= 10;

        DetectBeat();
    }

    void DetectBeat()
    {
        float currentEnergy = 0;
        for (int i = 0; i < spectrumData.Length; i++)
            currentEnergy += spectrumData[i] * spectrumData[i];
        currentEnergy /= spectrumData.Length;

        for (int i = historyBuffer.Length - 1; i > 0; i--)
            historyBuffer[i] = historyBuffer[i - 1];
        historyBuffer[0] = currentEnergy;

        float averageEnergy = 0;
        foreach (float energy in historyBuffer)
            averageEnergy += energy;
        averageEnergy /= historyBuffer.Length;

        float beatThreshold = 1.3f * averageEnergy;
        IsBeat = currentEnergy > beatThreshold;

        if (IsBeat)
        {
            float currentTime = Time.time;
            if (lastBeatTime > 0)
            {
                float interval = currentTime  - lastBeatTime;
                bpm = 60f / interval;
                beatCount++;
            }
        }
    }

    public float GetBPM()
    {
        return bpm;
    }
}
