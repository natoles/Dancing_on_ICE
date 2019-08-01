// Adapted from algorithmic-beat-mapping-unity by jesse-scam (https://github.com/jesse-scam/algorithmic-beat-mapping-unity)

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

using System.Numerics;
using System.IO;
using DSPLib;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class SpectralFluxData
{
    [JsonProperty(propertyName: "fs")] private readonly List<SpectralFluxInfo> spectralFluxSamples;

    [JsonProperty(propertyName: "ws")] private readonly int windowSize;

    [JsonProperty(propertyName: "cl")] private readonly float clipLength;

    public SpectralFluxData(List<SpectralFluxInfo> spectralFluxSamples, int windowSize, float clipLength)
    {
        this.spectralFluxSamples = spectralFluxSamples;
        this.windowSize = windowSize;
        this.clipLength = clipLength;
    }

    [OnDeserialized]
    private void BindNeightbours(StreamingContext context)
    {
        for (int i = windowSize / 2; i < spectralFluxSamples.Count - (windowSize + 1) / 2; ++i)
        {
            spectralFluxSamples[i - 1].next = spectralFluxSamples[i];
            spectralFluxSamples[i].previous = spectralFluxSamples[i - 1];
        }
    }

    public List<SpectralFluxInfo> SelectPeaks(float wantedPeaksRate)
    {
        float thresholdMultiplier = 1f;
        List<SpectralFluxInfo> peaks = spectralFluxSamples.FindAll((SpectralFluxInfo sfi) => sfi.IsPeak());
        if (peaks.Count > 0)
        {
            peaks.Sort((sfi1, sfi2) => -Comparer<float>.Default.Compare(sfi1.PrunedSpectralFlux(), sfi2.PrunedSpectralFlux())); // the minus is for sorting by decreasing order
            int effectivePeakCount = Mathf.Min(Mathf.RoundToInt(wantedPeaksRate * clipLength), peaks.Count - 1);
            thresholdMultiplier = peaks[effectivePeakCount].MultiplierToZero();
        }
        return spectralFluxSamples.FindAll((SpectralFluxInfo sfi) => sfi.IsPeak(thresholdMultiplier));
    }
}

public class BeatAnalyzer
{
    public static SpectralFluxData AnalyzeAudio(BeatmapContainer bmc, AudioClipData clipData)
    {
        if (!new DirectoryInfo(Application.streamingAssetsPath + "/SpectrumData/").Exists)
        {
            try
            {
                Directory.CreateDirectory(Application.streamingAssetsPath + "/SpectrumData/");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }

        SpectralFluxData spectralFluxData;
        if (new FileInfo(Application.streamingAssetsPath + "/SpectrumData/" + bmc.sourceFile + ".json").Exists)
        {
            try
            {
                StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/SpectrumData/" + RythmGameSettings.BeatmapToLoad.sourceFile + ".json");
                spectralFluxData = JsonConvert.DeserializeObject<SpectralFluxData>(reader.ReadToEnd());
                reader.Close();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }
        else
        {
            BeatAnalyzer analyzer = new BeatAnalyzer(clipData);
            spectralFluxData = analyzer.GetFullSpectrum();
            analyzer = null;

            try
            {
                StreamWriter writer = new StreamWriter(Application.streamingAssetsPath + "/SpectrumData/" + RythmGameSettings.BeatmapToLoad.sourceFile + ".json");
                writer.Write(JsonConvert.SerializeObject(spectralFluxData));
                writer.Close();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        return spectralFluxData;
    }

    private readonly int frequency;
    private readonly int channels;
	private readonly int lengthSamples;
	private readonly int sampleRate;
	private readonly float clipLength;
    private readonly float timePerSample;
	private readonly float[] multiChannelSamples;

    private SpectralFluxAnalyzer fluxAnalyzer;
    private readonly int spectrumSampleSize;
    private readonly int thresholdWindowSize;

    private BeatAnalyzer(AudioClipData clipData, int spectrumSampleSize = 1024, int thresholdWindowSize = 50)
    {
        this.spectrumSampleSize = spectrumSampleSize;
        this.thresholdWindowSize = thresholdWindowSize;
        fluxAnalyzer = new SpectralFluxAnalyzer(spectrumSampleSize, thresholdWindowSize);

        multiChannelSamples = new float[clipData.lengthSamples * clipData.channels];
        frequency = clipData.frequency;
		channels = clipData.channels;
		lengthSamples = clipData.lengthSamples;
		clipLength = (float)clipData.lengthSamples / clipData.frequency;
        timePerSample = 1f / frequency;

        clipData.data.CopyTo(multiChannelSamples, clipData.offsetSamples);
	}

    private float GetTimeFromIndex(int index)
    {
		return timePerSample * index;
	}

	public SpectralFluxData GetFullSpectrum()
    {
		try {
			// We only need to retain the samples for combined channels over the time domain
			float[] preProcessedSamples = new float[this.lengthSamples];

			int numProcessed = 0;
			float combinedChannelAverage = 0f;
			for (int i = 0; i < multiChannelSamples.Length; i++) {
				combinedChannelAverage += multiChannelSamples [i];

				// Each time we have processed all channels samples for a point in time, we will store the average of the channels combined
				if ((i + 1) % this.channels == 0) {
					preProcessedSamples[numProcessed] = combinedChannelAverage / this.channels;
					numProcessed++;
					combinedChannelAverage = 0f;
				}
			}

			// Once we have our audio sample data prepared, we can execute an FFT to return the spectrum data over the time domain
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
				float curSongTime = GetTimeFromIndex(i) * spectrumSampleSize;

				// Send our magnitude data off to our Spectral Flux Analyzer to be analyzed for peaks
				fluxAnalyzer.AnalyzeSpectrum (Array.ConvertAll (scaledFFTSpectrum, x => (float)x), curSongTime);
			}

            return new SpectralFluxData(fluxAnalyzer.spectralFluxSamples, thresholdWindowSize, clipLength);
		}
        catch (Exception e)
        {
			// Catch exceptions here since the background thread won't always surface the exception to the main thread
			Debug.LogException(e);
            return null;
		}
	}
}