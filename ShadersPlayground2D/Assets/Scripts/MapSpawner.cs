using UnityEngine;
using System.Collections.Generic;

public class MapSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject[] mapPrefabs;
    [SerializeField] private float mapArea = 50f;
    [SerializeField] private int maxIslands = 10;
    [SerializeField] private float distanceBetweenIslands = 5f;
    [SerializeField] private float spawnInterval = 1f;

    private List<Vector2> spawnedPositions = new List<Vector2>();
    private int currentIslands = 0;
    private float spawnTimer = 0f;


    private void Update()
    {
        spawnTimer += Time.deltaTime;
        
        if(spawnTimer >= spawnInterval && currentIslands < maxIslands)
        {
            SpawnMap();
            spawnTimer = 0f;
        }
    }

    private void SpawnMap()
    {
        if (mapPrefabs.Length <= 0) return;

        Vector2 spawnPosition;

        int maxAttempts = 10;

        do
        {
            spawnPosition = new Vector2(Random.Range(-mapArea, mapArea), Random.Range(-mapArea, mapArea));

            maxAttempts--;
        } while (!IsPositionValid(spawnPosition) && maxAttempts > 0);

        if(maxAttempts > 0)
        {
            GameObject islandPrefab = mapPrefabs[Random.Range(0, mapPrefabs.Length)];
            Instantiate(islandPrefab, spawnPosition, Quaternion.identity);
            spawnedPositions.Add(spawnPosition);
            currentIslands++;
        }

    }

    private bool IsPositionValid(Vector2 newPosition)
    {
        foreach ( Vector2 existingPosition in spawnedPositions)
        {
            if (Vector2.Distance(existingPosition, newPosition) < distanceBetweenIslands)
            {
                return false;
            }
        }
        return true;
    }
}
