using UnityEngine;

public class ColorGenerator
{ 
    ColorSettings _settings;
    Texture2D _texture;
    const int _textureResolution = 50;
    INoiseFilter _biomeNoiseFilter;

    public void UpdateSettings(ColorSettings settings)
    {
        _settings = settings;
        if (_texture == null || _texture.height != settings.biomeColorSettings.biomes.Length)
        {
            _texture = new Texture2D(_textureResolution, settings.biomeColorSettings.biomes.Length);
        }

        _biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(_settings.biomeColorSettings.noise);
    }

    public void UpdateElevation(MinMax elevationMinMax)
    {
        _settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
    {
        float heightPercent = (pointOnUnitSphere.y + 1) / 2f;
        heightPercent += (_biomeNoiseFilter.Evaluate(pointOnUnitSphere) - _settings.biomeColorSettings.noiseOffset) * _settings.biomeColorSettings.noiseStrength;
        float biomeIndex = 0;
        int numBiomes = _settings.biomeColorSettings.biomes.Length;
        float blendRange = _settings.biomeColorSettings.blendAmount / 2f + 0.001f;
        
        for (int i = 0; i < numBiomes; i++)
        {
            float dst = heightPercent - _settings.biomeColorSettings.biomes[i].startHeight;
            float weight = Mathf.InverseLerp(-blendRange, blendRange, dst);
            biomeIndex *= (1 - weight);
            biomeIndex += i * weight;
        }
        
        return biomeIndex / Mathf.Max(1, numBiomes - 1);
    }
    
    public void UpdateColors()
    {
        Color[] colors = new Color[_texture.width * _texture.height];
        int colorIndex = 0;
        foreach (var biome in _settings.biomeColorSettings.biomes)
        {
            for (int i = 0; i < _textureResolution; i++)
            {
                Color gradientCol = biome.gradient.Evaluate(i / (_textureResolution - 1f));
                Color tintCol = biome.tint;
                colors[colorIndex] = gradientCol * (1 - biome.tintPercent) + tintCol * biome.tintPercent;
                colorIndex++;
            }
        }
        
        _texture.SetPixels(colors);
        _texture.Apply();
        _settings.planetMaterial.SetTexture("_texture", _texture);
    }
}
