using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisual : MonoBehaviour
{
    public AudioSource audioSource = null;
    float[] data = null;

    int index = 0;
    int previous = 0;
    float angle = 0;
    Bounds bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(35 , 20, 1));

    void NormalizeValues(ref float[] array)
    {
        float maxval = Mathf.Max(data);
        for (int i = 0; i < data.Length; ++i)
            data[i] /= maxval;
    }

    void AdjustValues(ref float[] array)
    {
        float a = 12f;
        float b = 6f;
        float c = 0.7f;
        for (int i = 0; i < array.Length; ++i)
            array[i] = Mathf.Sign(array[i]) / Mathf.Pow((1f + Mathf.Exp(-a * Mathf.Abs(array[i]) + b)), c);
    }

    float Mean(in float[] array, int begin, int end, int step = 1)
    {
        float sum = 0;
        int start = Mathf.Max(0, begin);
        int stop = Mathf.Min(array.Length, end);
        int count = 0;
        for (int i = start; i <= stop; i += step)
        {
            sum += array[i];
            count++;
        }
        return sum / count;
    }

    void ComputePoints(in float[] data, out float[] result, int channels, int step, int width)
    {
        result = new float[data.Length / step];
        int align = 0;
        float r = width / 2f;
        int left = Mathf.FloorToInt(r);
        int right = Mathf.CeilToInt(r);
        int i, c, index, begin, end;
        for (i = 0; i < result.Length; i += channels)
        {
            for (c = 0; c < channels; ++c)
            {
                index = align + c;
                begin = index - left;
                end = index + right;
                result[i + c] = Mean(data, begin, end, channels);
            }
            align += step;
        }
    }

    private void Update()
    {
        if (audioSource.isPlaying)
        {
            if (data == null)
            {
                if (audioSource.clip != null)
                {
                    int channels = audioSource.clip.channels;
                    int freq = audioSource.clip.frequency;
                    int samples = audioSource.clip.samples;

                    data = new float[Mathf.CeilToInt(channels * samples)];
                    audioSource.clip.GetData(data, 0);
                    NormalizeValues(ref data);
                    AdjustValues(ref data);
                    ComputePoints(data, out float[] points, channels, freq, 4 * freq);
                    data = points;
                    NormalizeValues(ref data);

                    //int sz = 100;
                    //int step = freq / 6;
                    //Vector3[] pos = new Vector3[sz];
                    //for (int i = 0; i < sz; ++i)
                    //{
                    //    pos[i] = new Vector3(-17f + 34f * i / sz, CenteredMean(data, channels * i * step, step / 10, channels) * 10f, -0.1f);
                    //}

                    //LineRenderer line = GetComponent<LineRenderer>();
                    //line.positionCount = pos.Length;
                    //line.SetPositions(pos);
                }
            }

            if (data != null)
            {
                previous = index;
                index = audioSource.timeSamples / audioSource.clip.frequency;

                if (index != previous)
                {
                    angle = (angle + data[index] * 180f) % 360f;
                }
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 10f * Time.deltaTime);

                Vector3 translation = transform.up * 10f * Time.deltaTime;
                Vector3 tmpPos = transform.position;
                while (!bounds.Contains(tmpPos + translation))
                {
                    Vector3 clampedX = translation;
                    if (translation.x != 0)
                    {
                        float coeffdir = translation.y / translation.x;
                        float clampX = Mathf.Clamp(tmpPos.x + translation.x, bounds.min.x, bounds.max.x) - tmpPos.x;
                        clampedX = new Vector3(clampX, clampX * coeffdir);
                    }

                    Vector3 clampedY = translation;
                    if (translation.y != 0)
                    {
                        float coeffdir = translation.x / translation.y;
                        float clampY = Mathf.Clamp(tmpPos.y + translation.y, bounds.min.y, bounds.max.y) - tmpPos.y;
                        clampedY = new Vector3(clampY * coeffdir, clampY);
                        Debug.Log($"({translation.x},{translation.x}) -> ({clampedY.x},{clampedY.y})");
                    }

                    if (clampedX.magnitude <= clampedY.magnitude)
                    {
                        tmpPos += clampedX;
                        translation = Vector3.Scale(translation - clampedX, new Vector3(-1, 1));
                    }
                    else
                    {
                        tmpPos += clampedY;
                        translation = Vector3.Scale(translation - clampedY, new Vector3(1, -1));
                    }
                }

                transform.position = tmpPos + translation;
                float deviation = Vector3.Angle(transform.up, translation);
                angle = (angle + deviation) % 360f;
                transform.Rotate(transform.forward, deviation, Space.World);

                //transform.position = bounds.ClosestPoint(transform.position);

                //previous = index;
                //index = audioSource.timeSamples;
                //if (previous != index)
                //{
                //    float value = CenteredMean(data, 2 * index, 4 * (index - previous), 2) * 180f;
                //    angle = (value + angle) % 360f;
                //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 10f * Time.deltaTime);
                //    transform.Translate(transform.up * 10f * Time.deltaTime);
                //    transform.position = bounds.ClosestPoint(transform.position);
                //}
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.up);
    }
}
