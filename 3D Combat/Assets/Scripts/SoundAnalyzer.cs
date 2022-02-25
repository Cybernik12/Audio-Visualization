using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAnalyzer : MonoBehaviour
{
    private const int SAMPLE_SIZE = 1024;

    [SerializeField]
    private float rmsValue;
    [SerializeField]
    private float dbValue;
    [SerializeField]
    private float pitchValue;

    private AudioSource source;
    private float[] samples;
    private float[] spectrum;
    private float sampleRate;

    private GameObject playerModel;

    public float RmsValue
    {
        get
        {
            return rmsValue;
        }
        set
        {
            rmsValue = value;
        }
    }

    public float DbValue
    {
        get
        {
            return dbValue;
        }
        set
        {
            dbValue = value;
        }
    }

    public float PitchValue
    {
        get
        {
            return pitchValue;
        }
        set
        {
            pitchValue = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerModel = GameObject.FindWithTag("PlayerModel");
        source = playerModel.GetComponent<AudioSource>();
        samples = new float[SAMPLE_SIZE];
        spectrum = new float[SAMPLE_SIZE];
        sampleRate = AudioSettings.outputSampleRate;
    }

    // Update is called once per frame
    void Update()
    {
        AnalyzeSound();
    }

    private void AnalyzeSound()
    {
        source.GetOutputData(samples, 0);

        // Get the RMS
        int i = 0;
        float sum = 0;
        for (; i < SAMPLE_SIZE; i++)
        {
            sum = samples[i] * samples[i];
        }
        rmsValue = Mathf.Sqrt(sum / SAMPLE_SIZE);

        // Get the DB value
        dbValue = 20 * Mathf.Log10(rmsValue / 0.1f);

        // Get sound spectrum
        source.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        // Find pitch *Not Used*
        float maxV = 0;
        var maxN = 0;
        for (i = 0; i < SAMPLE_SIZE; i++)
        {
            if (!(spectrum[i] > maxV) || !(spectrum[i] > 0.0f))
                continue;

            maxV = spectrum[i];
            maxN = i;
        }

        float freqN = maxN;
        if (maxN > 0 && maxN < SAMPLE_SIZE - 1)
        {
            var dL = spectrum[maxN - 1] / spectrum[maxN];
            var dR = spectrum[maxN - 1] / spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }
        pitchValue = freqN * (sampleRate / 2) / SAMPLE_SIZE;
    }
}
