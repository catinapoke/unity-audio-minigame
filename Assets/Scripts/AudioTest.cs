using UnityEngine;

// The code example shows how to implement a metronome that procedurally
// generates the click sounds via the OnAudioFilterRead callback.
// While the game is paused or suspended, this time will not be updated and sounds
// playing will be paused. Therefore developers of music scheduling routines do not have
// to do any rescheduling after the app is unpaused

[RequireComponent(typeof(AudioSource))]
public class AudioTest : MonoBehaviour
{
    // Ударов в минуту
    public double bpm = 140.0F;
    // Набор чего нибудь 
    public float gain = 0.5F;
    // Высокие частоты мб
    public int signatureHi = 4;
    // Низкие частоты мб
    public int signatureLo = 4;

    // Следующий такт
    private double nextTick = 0.0F;
    // Амплитуда
    private float amp = 0.0F;
    // Фаза
    private float phase = 0.0F;
    // Частота входного файла
    private double sampleRate = 0.0F;
    private int accent;
    private bool running = false;

    void Start()
    {
        accent = signatureHi;
        double startTick = AudioSettings.dspTime;
        sampleRate = AudioSettings.outputSampleRate;
        nextTick = startTick * sampleRate;
        running = true;
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (!running)
            return;

        
        double samplesPerTick = sampleRate * 60.0F / bpm * 4.0F / signatureLo;
        // Текущее время аудио * частота аудиофайла
        double sample = AudioSettings.dspTime * sampleRate;
        int dataLen = data.Length / channels;

        int n = 0;
        while (n < dataLen)
        {
            float x = gain * amp * Mathf.Sin(phase);
            int i = 0;
            while (i < channels)
            {
                data[n * channels + i] += x;
                i++;
            }

            while (sample + n >= nextTick)
            {
                nextTick += samplesPerTick;
                amp = 1.0F;
                if (++accent > signatureHi)
                {
                    accent = 1;
                    amp *= 2.0F;
                }

                Debug.Log("Tick: " + accent + "/" + signatureHi);
            }

            phase += amp * 0.3F;
            amp *= 0.993F;
            n++;
        }
    }
}