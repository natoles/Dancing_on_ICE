// Adapted from algorithmic-beat-mapping-unity by jesse-scam (https://github.com/jesse-scam/algorithmic-beat-mapping-unity)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

using System.Numerics;
using DSPLib;

public class BeatAnalyzer
{
    readonly AudioSource audioSource;
    readonly int numChannels;
	readonly int numTotalSamples;
	readonly int sampleRate;
	readonly float clipLength;
	readonly float[] multiChannelSamples;
	SpectralFluxAnalyzer fluxAnalyzer;

    Thread bgThread;
    bool completed = false;
    bool crashed = false;

    public float PeaksFactor { get; set; } = 5f;

    public float ThresholdMultiplier { get; private set; } = 1;

    public List<SpectralFluxInfo> SpectralFluxSamples
    {
        get
        {
            return fluxAnalyzer?.spectralFluxSamples;
        }
    }

    public bool Completed
    {
        get
        {
            if (completed)
                bgThread.Join();
            return completed;
        }
    }

    public bool Crashed
    {
        get
        {
            if (crashed)
                bgThread.Join();
            return crashed;
        }
    }

    public BeatAnalyzer(AudioSource audioSource, float thresholdMultipler = 1.5f, int thresholdWindowSize = 50)
    {
        this.audioSource = audioSource;

        fluxAnalyzer = new SpectralFluxAnalyzer
        {
            ThresholdMultiplier = thresholdMultipler,
            ThresholdWindowSize = thresholdWindowSize
        };

        // Need all audio samples.  If in stereo, samples will return with left and right channels interweaved
        // [L,R,L,R,L,R]
        multiChannelSamples = new float[audioSource.clip.samples * audioSource.clip.channels];
		numChannels = audioSource.clip.channels;
		numTotalSamples = audioSource.clip.samples;
		clipLength = audioSource.clip.length;

		// We are not evaluating the audio as it is being played by Unity, so we need the clip's sampling rate
		this.sampleRate = audioSource.clip.frequency;

		audioSource.clip.GetData(multiChannelSamples, 0);
		//Debug.Log ("GetData done");

		bgThread = new Thread (this.getFullSpectrumThreaded);

	}

    public void Start()
    {
        Debug.Log("Starting Background Thread");
        bgThread.Start();
    }

	public int SampleIndex(float time)
    {
		return getIndexFromTime (time) / 1024;
	}

	int getIndexFromTime(float curTime)
    {
		float lengthPerSample = this.clipLength / (float)this.numTotalSamples;

		return Mathf.FloorToInt (curTime / lengthPerSample);
	}

	float getTimeFromIndex(int index)
    {
		return ((1f / (float)this.sampleRate) * index);
	}

	void getFullSpectrumThreaded()
    {
		try {
			// We only need to retain the samples for combined channels over the time domain
			float[] preProcessedSamples = new float[this.numTotalSamples];

			int numProcessed = 0;
			float combinedChannelAverage = 0f;
			for (int i = 0; i < multiChannelSamples.Length; i++) {
				combinedChannelAverage += multiChannelSamples [i];

				// Each time we have processed all channels samples for a point in time, we will store the average of the channels combined
				if ((i + 1) % this.numChannels == 0) {
					preProcessedSamples[numProcessed] = combinedChannelAverage / this.numChannels;
					numProcessed++;
					combinedChannelAverage = 0f;
				}
			}

			// Once we have our audio sample data prepared, we can execute an FFT to return the spectrum data over the time domain
			int spectrumSampleSize = 1024;
			int iterations = preProcessedSamples.Length / spectrumSampleSize;

			FFT fft = new FFT ();
			fft.Initialize ((UInt32)spectrumSampleSize);

			//Debug.Log (string.Format("Processing {0} time domain samples for FFT", iterations));
			double[] sampleChunk = new double[spectrumSampleSize];
			for (int i = 0; i < iterations; i++) {
				// Grab the current 1024 chunk of audio sample data
				Array.Copy (preProcessedSamples, i * spectrumSampleSize, sampleChunk, 0, spectrumSampleSize);

				// Apply our chosen FFT Window
				double[] windowCoefs = DSP.Window.Coefficients (DSP.Window.Type.Hanning, (uint)spectrumSampleSize);
				double[] scaledSpectrumChunk = DSP.Math.Multiply (sampleChunk, windowCoefs);
				double scaleFactor = DSP.Window.ScaleFactor.Signal (windowCoefs);

				// Perform the FFT and convert output (complex numbers) to Magnitude
				Complex[] fftSpectrum = fft.Execute (scaledSpectrumChunk);
				double[] scaledFFTSpectrum = DSPLib.DSP.ConvertComplex.ToMagnitude (fftSpectrum);
				scaledFFTSpectrum = DSP.Math.Multiply (scaledFFTSpectrum, scaleFactor);

				// These 1024 magnitude values correspond (roughly) to a single point in the audio timeline
				float curSongTime = getTimeFromIndex(i) * spectrumSampleSize;

				// Send our magnitude data off to our Spectral Flux Analyzer to be analyzed for peaks
				fluxAnalyzer.analyzeSpectrum (Array.ConvertAll (scaledFFTSpectrum, x => (float)x), curSongTime);
			}

            Debug.Log("Selecting peaks");
            List<SpectralFluxInfo> peaks = fluxAnalyzer.spectralFluxSamples.FindAll((SpectralFluxInfo sfi) => sfi.IsPeak());
            Debug.Log($"Sorting peaks ({peaks.Count} found)");
            peaks.Sort((sfi1, sfi2) => - Comparer<float>.Default.Compare(sfi1.PrunedSpectralFlux(), sfi2.PrunedSpectralFlux())); // the minus is for sorting by decreasing order
            int effectivePeakCount = Mathf.Min(Mathf.RoundToInt(PeaksFactor * clipLength), peaks.Count - 1);
            ThresholdMultiplier = peaks[effectivePeakCount].MultiplierToZero;
            Debug.Log($"Multiplier used: {ThresholdMultiplier} for {effectivePeakCount} peaks");
            peaks = null;

            Debug.Log ("Background Thread Completed");
            completed = true;
				
		} catch (Exception e) {
			// Catch exceptions here since the background thread won't always surface the exception to the main thread
			Debug.Log (e.ToString ());
            crashed = true;
		}
	}
}