// Adapted from algorithmic-beat-mapping-unity by jesse-scam (https://github.com/jesse-scam/algorithmic-beat-mapping-unity)

using System.Collections.Generic;
using UnityEngine;

public class SpectralFluxInfoLight
{
    public float spectralFlux;
    public float unscaledThreshold;

    internal float PrunedSpectralFlux (float thresholdMultiplier)
    {
        return spectralFlux - unscaledThreshold * thresholdMultiplier;
    }

    public float PrunedSpectralFlux()
    {
        return spectralFlux - unscaledThreshold;
    }

    public float MultiplierToZero
    {
        get
        {
            return spectralFlux / unscaledThreshold;
        }
    }
}

public class SpectralFluxInfo : SpectralFluxInfoLight {
	public float time;
    public SpectralFluxInfoLight previous;
    public SpectralFluxInfoLight next;

    public bool IsPeak(float thresholdMultiplier)
    {
        if (previous == null || next == null)
            return false;

        float currentPrunedSpectralFlux = PrunedSpectralFlux(thresholdMultiplier);
        return
             currentPrunedSpectralFlux > 0f &&
             currentPrunedSpectralFlux > next.PrunedSpectralFlux(thresholdMultiplier) &&
             currentPrunedSpectralFlux > previous.PrunedSpectralFlux(thresholdMultiplier);
    }

    public bool IsPeak()
    {
        if (previous == null || next == null)
            return false;

        float currentPrunedSpectralFlux = PrunedSpectralFlux();
        return
             currentPrunedSpectralFlux > 0f &&
             currentPrunedSpectralFlux > next.PrunedSpectralFlux() &&
             currentPrunedSpectralFlux > previous.PrunedSpectralFlux();
    }
}

public class SpectralFluxAnalyzer
{
    // Size of sample arrays passed to the analyzer
	private readonly int numSamples = 1024;

	// Number of samples to average in our window
	private readonly int thresholdWindowSize = 50;

	public List<SpectralFluxInfo> spectralFluxSamples;

	private float[] curSpectrum;
	private float[] prevSpectrum;

	private int indexToProcess;

	public SpectralFluxAnalyzer (int numSamples, int thresholdWindowSize) {
		spectralFluxSamples = new List<SpectralFluxInfo> ();

        this.numSamples = numSamples;
        this.thresholdWindowSize = thresholdWindowSize;

        // Start processing from middle of first window and increment by 1 from there
        indexToProcess = this.thresholdWindowSize / 2;

		curSpectrum = new float[numSamples];
		prevSpectrum = new float[numSamples];
	}

    public void SetCurSpectrum(float[] spectrum) {
		curSpectrum.CopyTo (prevSpectrum, 0);
		spectrum.CopyTo (curSpectrum, 0);
	}
		
	public void AnalyzeSpectrum(float[] spectrum, float time) {
		// Set spectrum
		SetCurSpectrum(spectrum);

		// Get current spectral flux from spectrum
		SpectralFluxInfo curInfo = new SpectralFluxInfo();
		curInfo.time = time;
		curInfo.spectralFlux = CalculateRectifiedSpectralFlux ();
		spectralFluxSamples.Add (curInfo);

		// We have enough samples to detect a peak
		if (spectralFluxSamples.Count >= thresholdWindowSize) {
			// Get Flux threshold of time window surrounding index to process
			spectralFluxSamples[indexToProcess].unscaledThreshold = GetUnscaledFluxThreshold (indexToProcess);

			// Now that we are processed at n, n-1 has neighbors (n-2, n) to determine peak
			int indexToDetectPeak = indexToProcess - 1;

            spectralFluxSamples[indexToDetectPeak].previous = spectralFluxSamples[indexToDetectPeak - 1];
            spectralFluxSamples[indexToDetectPeak].next     = spectralFluxSamples[indexToDetectPeak + 1];

			indexToProcess++;
		}
		else {
			//Debug.Log(string.Format("Not ready yet.  At spectral flux sample size of {0} growing to {1}", spectralFluxSamples.Count, thresholdWindowSize));
		}
	}

	float CalculateRectifiedSpectralFlux() {
		float sum = 0f;

		// Aggregate positive changes in spectrum data
		for (int i = 0; i < numSamples; i++) {
			sum += Mathf.Max (0f, curSpectrum [i] - prevSpectrum [i]);
		}
		return sum;
	}

    float GetUnscaledFluxThreshold(int spectralFluxIndex)
    {
        // How many samples in the past and future we include in our average
        int windowStartIndex = Mathf.Max(0, spectralFluxIndex - thresholdWindowSize / 2);
        int windowEndIndex = Mathf.Min(spectralFluxSamples.Count - 1, spectralFluxIndex + thresholdWindowSize / 2);

        // Add up our spectral flux over the window
        float sum = 0f;
        for (int i = windowStartIndex; i < windowEndIndex; i++)
        {
            sum += spectralFluxSamples[i].spectralFlux;
        }

        // Return the average multiplied by our sensitivity multiplier
        return sum / (windowEndIndex - windowStartIndex);
    }
}