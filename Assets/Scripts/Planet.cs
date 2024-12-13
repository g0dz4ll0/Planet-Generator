using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(2, 256)]
    [SerializeField] int resolution = 10;
    [SerializeField] bool autoUpdate = true;

    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;

    [HideInInspector]
    public bool shapeSettingsFoldout;
    [HideInInspector]
    public bool colorSettingsFoldout;

    ShapeGenerator _shapeGenerator;

    [SerializeField, HideInInspector]
    MeshFilter[] _meshFilters;
    TerrainFace[] _terrainFaces;

    void Initialize()
    {
        _shapeGenerator = new ShapeGenerator(shapeSettings);

        if (_meshFilters == null || _meshFilters.Length == 0)
        {
            _meshFilters = new MeshFilter[6];
        }
        _terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (_meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                _meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                _meshFilters[i].sharedMesh = new Mesh();
            }

            _terrainFaces[i] = new TerrainFace(_shapeGenerator, _meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }

    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    public void OnColorSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColors();
        }
    }

    void GenerateMesh()
    {
        foreach (TerrainFace face in _terrainFaces)
        {
            face.ConstructMesh();
        }
    }

    void GenerateColors()
    {
        foreach (MeshFilter m in _meshFilters)
        {
            m.GetComponent<MeshRenderer>().sharedMaterial.color = colorSettings.planetColor;
        }
    }
}
