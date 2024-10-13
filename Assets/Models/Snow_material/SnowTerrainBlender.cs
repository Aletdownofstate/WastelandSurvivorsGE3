using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Terrain))]
public class SnowTerrainBlender : MonoBehaviour
{
    [Header("Snow Texture Properties")]
    [SerializeField] private Texture2D snowAlbedo; // Snow Albedo texture
    [SerializeField] private Texture2D snowNormalMap; // Snow Normal map texture
    [SerializeField] private Texture2D snowAmbientOcclusion; // Snow AO texture
    [SerializeField] private Texture2D snowRoughness; // Snow Roughness texture
    [SerializeField] private Texture2D snowMetallic; // Snow Metallic texture

    [Header("Snow Control")]
    [Range(0f, 1f)]
    [SerializeField] private float snowCoverage = 0f; // Control for snow coverage

    private Material terrainMaterial;

    void Start()
    {
        // Get the terrain material
        Terrain terrain = GetComponent<Terrain>();
        terrainMaterial = terrain.materialTemplate;

        if (terrainMaterial != null)
        {
            ApplySnowProperties(); // Apply snow properties to the terrain material
        }
        else
        {
            Debug.LogWarning("No material assigned to the terrain. Make sure you have a material set up for the terrain.");
        }
    }

    void Update()
    {
        if (terrainMaterial != null)
        {
            // Update the snow coverage in the shader
            terrainMaterial.SetFloat("_SnowCoverage", snowCoverage); // Pass snow coverage to shader
        }
    }

    private void ApplySnowProperties()
    {
        // Set the textures for the snow material
        if (snowAlbedo != null)
        {
            terrainMaterial.SetTexture("_SnowAlbedo", snowAlbedo); // Set snow albedo texture
        }

        if (snowNormalMap != null)
        {
            terrainMaterial.SetTexture("_SnowNormalMap", snowNormalMap); // Set snow normal map texture
        }

        if (snowAmbientOcclusion != null)
        {
            terrainMaterial.SetTexture("_SnowAO", snowAmbientOcclusion); // Set snow ambient occlusion texture
        }

        if (snowRoughness != null)
        {
            terrainMaterial.SetTexture("_SnowRoughness", snowRoughness); // Set snow roughness texture
        }

        if (snowMetallic != null)
        {
            terrainMaterial.SetTexture("_SnowMetallic", snowMetallic); // Set snow metallic texture
        }
    }
}
