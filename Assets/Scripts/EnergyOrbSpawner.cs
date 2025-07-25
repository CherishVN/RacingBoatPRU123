using UnityEngine;

public class EnergyOrbSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject energyOrbPrefab;
    public Material energyOrbMaterial;
    public float spawnInterval = 5f;
    public int maxOrbsActive = 10;

    [Header("Spawn Area")]
    public Vector2 spawnAreaSize = new Vector2(50f, 50f);
    public float waterLevel = 0f;

    private float nextSpawnTime;

    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime && GameObject.FindGameObjectsWithTag("EnergyOrb").Length < maxOrbsActive)
        {
            SpawnEnergyOrb();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnEnergyOrb()
    {
        if (energyOrbPrefab == null) return;

        float randomX = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
        float randomZ = Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f);

        Vector3 spawnPosition = transform.position + new Vector3(randomX, waterLevel, randomZ);

        GameObject newOrb = Instantiate(energyOrbPrefab, spawnPosition, Quaternion.identity);
        newOrb.tag = "EnergyOrb";

        MeshRenderer renderer = newOrb.GetComponent<MeshRenderer>();
        if (renderer != null && energyOrbMaterial != null)
        {
            renderer.material = energyOrbMaterial;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = transform.position + Vector3.up * waterLevel;
        Vector3 size = new Vector3(spawnAreaSize.x, 0.1f, spawnAreaSize.y);
        Gizmos.DrawWireCube(center, size);
    }
}