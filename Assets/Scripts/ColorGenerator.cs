using UnityEngine;

public class ColorGenerator
{ 
    ColorSettings _settings;
    Texture2D _texture;
    const int _textureResolution = 50;

    public void UpdateSettings(ColorSettings settings)
    {
        _settings = settings;
        if (_texture == null)
        {
            _texture = new Texture2D(_textureResolution, 1);
        }
    }

    public void UpdateElevation(MinMax elevationMinMax)
    {
        _settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    public void UpdateColors()
    {
        Color[] colors = new Color[_textureResolution];
        for (int i = 0; i < _textureResolution; i++)
        {
            colors[i] = _settings.gradient.Evaluate(i / (_textureResolution - 1f));
        }
        _texture.SetPixels(colors);
        _texture.Apply();
        _settings.planetMaterial.SetTexture("_texture", _texture);
    }
}
