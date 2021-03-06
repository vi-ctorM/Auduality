using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundVisual : MonoBehaviour
{
    private const int SAMPLE_SIZE = 1024;
    private const float delta = .1f;
    
    public float rmsValue;
    public float dbValue;
    public float pitchValue;

    public float backgroundIntensity;
    public SpriteRenderer backgroundMaterial;
    public Color minColor;// = new Color(;
    public Color maxColor;
        
    public float maxVisualScale = 25.0f;
    public float visualModifier = 50.0f;
    public float smoothSpeed = 1.0f;
    public float keepPercentage = 0.5f;

    private AudioSource source;
    private float[] samples;
    private float[] spectrum;
    private float sampleRate;

    private void Start(){
	source = GetComponent<AudioSource>();
	samples = new float[SAMPLE_SIZE];

	spectrum = new float[SAMPLE_SIZE];

	sampleRate = AudioSettings.outputSampleRate;
    }

    private void Update(){
	AnalyzeSound();
	UpdateBackground();
    }



    private void UpdateBackground(){
	backgroundIntensity -= Time.deltaTime * smoothSpeed;
	if(backgroundIntensity < dbValue/30)
	    backgroundIntensity = -dbValue/30;

	print("Background color: " + backgroundMaterial.color);
	// print("intensity: " + backgroundIntensity);

	backgroundMaterial.color =  Color.Lerp(minColor, maxColor, backgroundIntensity);

	// backgroundMaterial.color =  new Color(0, 0, 1, 1);
    }
    
    private void AnalyzeSound(){
    	source.GetOutputData(samples, 0);

    	// Get the RMS value
    	float sum = 0;
    	for (int i = 0; i < SAMPLE_SIZE; ++i) {
    	    sum = samples[i] * samples[i];
    	}
    	rmsValue = Mathf.Sqrt(sum /SAMPLE_SIZE);

    	// Get the db value
    	dbValue = 20 * Mathf.Log10(rmsValue / 0.1f);

    	// Get sound spectrum
    	source.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

    // 	float maxV = 0;
    // 	var maxN = 0;
    // 	for(int i = 0; i < SAMPLE_SIZE; i++){
    // 	    if(!(spectrum[i] > maxV) || !(spectrum[i] > 0.0f))
    // 		continue;

    // 	    maxV = spectrum[i];
    // 	    maxN = i;
    // 	}

    // 	float freqN = maxN;
    // 	if(maxN > 0 && maxN < SAMPLE_SIZE - 1){
    // 	    var dL = spectrum[maxN - 1] / spectrum[maxN];
    // 	    var dR = spectrum[maxN + 1] / spectrum[maxN];
    // 	    freqN += 0.5f * (dR * dR - dL * dL);
    // 	}
    // 	pitchValue = freqN * (sampleRate / 2) / SAMPLE_SIZE;	    
    }
    
}
