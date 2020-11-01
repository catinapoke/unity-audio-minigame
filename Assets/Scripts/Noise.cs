using UnityEngine;

public class Noise : MonoBehaviour
{
    // un-optimized version of a noise generator
    private System.Random RandomNumber = new System.Random();
    [SerializeField] private float _gain = 0.2f; 
    
    
    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            //RandomNumber.NextDouble()
            float randomNoise = 1.0f - (float)RandomNumber.NextDouble() * _gain;
            data[i] *= randomNoise;
            // if we have stereo, we copy the mono data to each channel
            if (channels == 2)
                data[i + 1] *= randomNoise;
        }
    }
}