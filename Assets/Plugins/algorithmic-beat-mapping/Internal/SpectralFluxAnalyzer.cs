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
        float previousPrunedSpectralFlux = previous.PrunedSpectralFlux(thresholdMultiplier);
        float nextPrunedSpectralFlux = next.PrunedSpectralFlux(thresholdMultiplier);
        return
             currentPrunedSpectralFlux > 0f &&
             currentPrunedSpectralFlux > nextPrunedSpectralFlux &&
             currentPrunedSpectralFlux > previousPrunedSpectralFlux;
    }

    public bool IsPeak()
    {
        if (previous == null || next == null)
            return false;

        float currentPrunedSpectralFlux = PrunedSpectralFlux();
        float previousPrunedSpectralFlux = previous.PrunedSpectralFlux();
        float nextPrunedSpectralFlux = next.PrunedSpectralFlux();
        return
             currentPrunedSpectralFlux > 0f &&
             currentPrunedSpectralFlux > nextPrunedSpectralFlux &&
             currentPrunedSpectralFlux > previousPrunedSpectralFlux;
    }
}

public class SpectralFluxAnalyzer {
	int numSamples = 1024;
    public int NumSamples { get { return numSamples; } set { numSamples = Mathf.Max(0, value); } }

	// Sensitivity multiplier to scale the average threshold.
	// In this case, if a rectified spectral flux sample is > 1.5 times the average, it is a peak
	float thresholdMultiplier = 1.5f;
    public float ThresholdMultiplier { get { return thresholdMultiplier; } set { thresholdMultiplier = Mathf.Max(0f, value); } }

	// Number of samples to average in our window
	int thresholdWindowSize = 50;
    public int ThresholdWindowSize { get { return thresholdWindowSize; } set { thresholdWindowSize = Mathf.Max(0, value); } }

	public List<SpectralFluxInfo> spectralFluxSamples;

	float[] curSpectrum;
	float[] prevSpectrum;

	int indexToProcess;

	public SpectralFluxAnalyzer () {
		spectralFluxSamples = new List<SpectralFluxInfo> ();

        // Start processing from middle of first window and increment by 1 from there
        indexToProcess = this.thresholdWindowSize / 2;

		curSpectrum = new float[numSamples];
		prevSpectrum = new float[numSamples];
	}

    public void setCurSpectrum(float[] spectrum) {
		curSpectrum.CopyTo (prevSpectrum, 0);
		spectrum.CopyTo (curSpectrum, 0);
	}
		
	public void analyzeSpectrum(float[] spectrum, float time) {
		// Set spectrum
		setCurSpectrum(spectrum);

		// Get current spectral flux from spectrum
		SpectralFluxInfo curInfo = new SpectralFluxInfo();
		curInfo.time = time;
		curInfo.spectralFlux = calculateRectifiedSpectralFlux ();
		spectralFluxSamples.Add (curInfo);

		// We have enough samples to detect a peak
		if (spectralFluxSamples.Count >= thresholdWindowSize) {
			// Get Flux threshold of time window surrounding index to process
			spectralFluxSamples[indexToProcess].unscaledThreshold = getUnscaledFluxThreshold (indexToProcess);

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

	float calculateRectifiedSpectralFlux() {
		float sum = 0f;

		// Aggregate positive changes in spectrum data
		for (int i = 0; i < numSamples; i++) {
			sum += Mathf.Max (0f, curSpectrum [i] - prevSpectrum [i]);
		}
		return sum;
	}

    float getUnscaledFluxThreshold(int spectralFluxIndex)
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