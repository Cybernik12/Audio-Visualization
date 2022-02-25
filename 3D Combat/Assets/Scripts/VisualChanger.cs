using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualChanger : MonoBehaviour
{
    // this.material.color = Color.HSVToRGB(visualScale[visualIndex] / 10, pitchValue / 100, 1);

    private GameObject player;

    private SoundAnalyzer soundAnalyzer;

    Renderer rend;

    private float rmsValue;
    private float dbValue;
    private float pitchValue;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindWithTag("Player");

        soundAnalyzer = player.GetComponent<SoundAnalyzer>();

        rmsValue = soundAnalyzer.RmsValue;
        dbValue = soundAnalyzer.DbValue;
        pitchValue = soundAnalyzer.PitchValue;

        rend.material.color = new Vector4(pitchValue / 255, pitchValue / 255, pitchValue / 255, 1);

    }
}
