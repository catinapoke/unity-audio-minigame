using UnityEngine;
using System;

public class Sinus : MonoBehaviour
{
    // un-optimized version
    public double frequency = 440;
    public double gain = 0.05;
    private double increment;
    private double phase = 0;
    private double sampling_frequency = 48000;

    void OnAudioFilterRead(float[] data, int channels)
    {
        // update increment in case frequency has changed
        increment = frequency * 2 * Math.PI / sampling_frequency;
        for (var i = 0; i < data.Length; i = i + channels)
        {
            phase = phase + increment;
            float gainedSinus = (float) (gain * Math.Sin(phase));
            // this is where we copy audio data to make them “available” to Unity
            data[i] += gainedSinus;
            // if we have stereo, we copy the mono data to each channel
            if (channels == 2)
                data[i + 1] += gainedSinus;

            if (phase > 2 * Math.PI)
                phase = 0;
        }
    }
}