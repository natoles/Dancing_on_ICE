// Adapted from algorithmic-beat-mapping-unity by jesse-scam (https://github.com/jesse-scam/algorithmic-beat-mapping-unity)

using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class SpectralFluxInfoLight
{
    [JsonProperty(propertyName: "sf")] public float spectralFlux;

    [JsonProperty(propertyName: "ut")] public float unscaledThreshold;

    public float PrunedSpectralFlux(float thresholdMultiplier)
    {
        return spectralFlux - unscaledThreshold * thresholdMultiplier;
    }

    public float PrunedSpectralFlux()
    {
        return spectralFlux - unscaledThreshold;
    }
    
    public float MultiplierToZero()
    {
        return spectralFlux / unscaledThreshold;
    }

    public SpectralFluxInfoLight Copy()
    {
        return new SpectralFluxInfoLight { spectralFlux = spectralFlux, unscaledThreshold = unscaledThreshold };
    }
}

[JsonObject(MemberSerialization.OptIn)]
public class SpectralFluxInfo : SpectralFluxInfoLight
{
    [JsonProperty(propertyName: "tm")] public float time;
    
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
    private readonly int spectrumSampleSize;

    // Number of samples to average in our window
    private readonly int thresholdWindowSize;

    public List<SpectralFluxInfo> spectralFluxSamples;

    private float[] curSpectrum;
    private float[] prevSpectrum;

    private int indexToProcess;

    public SpectralFluxAnalyzer(int spectrumSampleSize, int thresholdWindowSize)
    {
        spectralFluxSamples = new List<SpectralFluxInfo>();

        this.spectrumSampleSize = spectrumSampleSize;
        this.thresholdWindowSize = thresholdWindowSize;

        // Start processing from middle of first window and increment by 1 from there
        indexToProcess = this.thresholdWindowSize / 2;

        curSpectrum = new float[this.spectrumSampleSize];
        prevSpectrum = new float[this.spectrumSampleSize];
    }

    public void SetCurSpectrum(float[] spectrum)
    {
        curSpectrum.CopyTo(prevSpectrum, 0);
        spectrum.CopyTo(curSpectrum, 0);
    }

    public void AnalyzeSpectrum(float[] spectrum, float time)
    {
        // Set spectrum
        SetCurSpectrum(spectrum);

        // Get current spectral flux from spectrum
        SpectralFluxInfo curInfo = new SpectralFluxInfo();
        curInfo.time = time;
        curInfo.spectralFlux = CalculateRectifiedSpectralFlux();
        spectralFluxSamples.Add(curInfo);

        // We have enough samples to detect a peak
        if (spectralFluxSamples.Count >= thresholdWindowSize)
        {
            // Get Flux threshold of time window surrounding index to process
            spectralFluxSamples[indexToProcess].unscaledThreshold = GetUnscaledFluxThreshold(indexToProcess);

            // Now that we are processed at n, n-1 has neighbors (n-2, n) to determine peak
            int indexToDetectPeak = indexToProcess - 1;

            spectralFluxSamples[indexToDetectPeak].previous = spectralFluxSamples[indexToDetectPeak - 1];
            spectralFluxSamples[indexToDetectPeak].next = spectralFluxSamples[indexToDetectPeak + 1];

            indexToProcess++;
        }
    }

    float CalculateRectifiedSpectralFlux()
    {
        float sum = 0f;

        // Aggregate positive changes in spectrum data
        for (int i = 0; i < spectrumSampleSize; i++)
        {
            sum += Mathf.Max(0f, curSpectrum[i] - prevSpectrum[i]);
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