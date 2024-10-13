using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Terrain))]
public class SnowTerrainBlender : MonoBehaviour
{
    [Header("Snow Texture Properties")]
    [SerializeField] private Texture2D snowAlbedo; 
    [SerializeField] private Texture2D snowNormalMap; 
    [SerializeField] private Texture2D snowAmbientOcclusion; 
    [SerializeField] private Texture2D snowRoughness; 
    [SerializeField] private Texture2D snowMetallic; 

    [Header("Snow Control")]
    [Range(0f, 1f)]
    [SerializeField] private float snowCoverage = 0f; 

    private Material terrainMaterial;

    void Start()
    {
        // Get the terrain material
        Terrain terrain = GetComponent<Terrain>();
        terrainMaterial = terrain.materialTemplate;

        if (terrainMaterial != null)
        {
            ApplySnowProperties(); /
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
            terrainMaterial.SetFloat("_SnowCoverage", snowCoverage); 
        }
    }

    private void ApplySnowProperties()
    {
        // Set the textures for the snow material
        if (snowAlbedo != null)
        {
            terrainMaterial.SetTexture("_SnowAlbedo", snowAlbedo); 
        }

        if (snowNormalMap != null)
        {
            terrainMaterial.SetTexture("_SnowNormalMap", snowNormalMap); 
        }

        if (snowAmbientOcclusion != null)
        {
            terrainMaterial.SetTexture("_SnowAO", snowAmbientOcclusion); 
        }

        if (snowRoughness != null)
        {
            terrainMaterial.SetTexture("_SnowRoughness", snowRoughness); 
        }

        if (snowMetallic != null)
        {
            terrainMaterial.SetTexture("_SnowMetallic", snowMetallic); 
        }
    }
}
