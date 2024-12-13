using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
    NoiseSettings.SimpleNoiseSettings _settings;
    Noise _noise = new Noise();

    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings)
    {
        _settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = _settings.baseRoughness;
        float amplitude = 1;

        for (int i = 0; i < _settings.numLayers; i++)
        {
            float v = _noise.Evaluate(point * frequency + _settings.center);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency *= _settings.roughness;
            amplitude *= _settings.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - _settings.minValue);
        return noiseValue * _settings.strength;
    }
}
