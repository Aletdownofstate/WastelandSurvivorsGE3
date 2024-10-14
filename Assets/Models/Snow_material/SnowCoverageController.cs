using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class SnowCoverageController : MonoBehaviour
{
    [SerializeField]
    private Material terrainMaterial;
    [Range(0f, 1f)]
    public float snowCoverage = 0f;

    [Range(0f, 1f)]
    public float snowBlendStrength = 0.5f;

    public Texture2D snowAlbedo;
    public Texture2D snowNormal;

    private void Start()
    {
        // Get the terrain component and its material
        Terrain terrain = GetComponent<Terrain>();
        terrainMaterial = terrain.materialTemplate;

        // Make sure the terrain material is assigned
        if (terrainMaterial == null)
        {
            Debug.LogError("Terrain material is not assigned. Please assign a material using the custom terrain shader.");
        }

        // Set initial snow textures if they are assigned
        if (snowAlbedo != null)
        {
            terrainMaterial.SetTexture("_SnowAlbedo", snowAlbedo);
        }

        if (snowNormal != null)
        {
            terrainMaterial.SetTexture("_SnowNormal", snowNormal);
        }
    }

    private void Update()
    {
        // Update the snow coverage and blend strength in the material
        if (terrainMaterial != null)
        {
            terrainMaterial.SetFloat("_SnowCoverage", snowCoverage);
            terrainMaterial.SetFloat("_SnowBlendStrength", snowBlendStrength);
        }
    }
}

