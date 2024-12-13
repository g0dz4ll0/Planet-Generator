using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    Planet _planet;
    Editor _shapeEditor;
    Editor _colorEditor;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                _planet.GeneratePlanet();
            }
        }

        if (GUILayout.Button("Generate Planet"))
        {
            _planet.GeneratePlanet();
        }

        DrawSettingsEditor(_planet.shapeSettings, _planet.OnShapeSettingsUpdated, ref _planet.shapeSettingsFoldout, ref _shapeEditor);
        DrawSettingsEditor(_planet.colorSettings, _planet.OnColorSettingsUpdated, ref _planet.colorSettingsFoldout, ref _colorEditor);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldout)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();

                    if (check.changed)
                    {
                        if (onSettingsUpdated != null)
                        {
                            onSettingsUpdated();
                        }
                    }
                }
            }
        }
    }

    void OnEnable()
    {
        _planet = (Planet)target;
    }
}
