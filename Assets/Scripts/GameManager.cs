using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Players location and game object
    public GameObject player;
    // Where the aliens will spawn
    public GameObject[] spawnPoints;
    // The aliens prefab
    public GameObject alien;
    // The maximum aliens that can appear
    public int maxAliensOnScreen;
    // How many aliens need to die for the player to win
    public int totalAliens;
    // Both control the spawn rates
    public float minSpawnTime;
    public float maxSpawnTime;
    // How many aliens spawn during the spawn event
    public int aliensPerSpawn;

    public GameObject upgradePrefab;
    public Gun gun;
    public float upgradeMaxTimeSpawn = 7.5f;

    private bool spawnedUpgrade = false;
    private float actualUpgradeTime = 0;
    private float currentUpgradeTime = 0;

    // Tracks # of aliens displayed at current time - Used to tell gamemanager if it needs to spawn more
    private int aliensOnScreen = 0;
    // Tracks time between spawning events - Fully random
    private float generatedSpawnTime = 0;
    // Tracks milliseconds since last spawn event
    private float currentSpawnTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        actualUpgradeTime = Random.Range(upgradeMaxTimeSpawn - 3.0f,
        upgradeMaxTimeSpawn);
        actualUpgradeTime = Mathf.Abs(actualUpgradeTime);
    }

    // Update is called once per frame
    void Update()
    {
        currentUpgradeTime += Time.deltaTime;
        currentSpawnTime += Time.deltaTime;

        if (currentUpgradeTime > actualUpgradeTime)
        {
            // 1
            if (!spawnedUpgrade)
            {
                // 2
                int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                GameObject spawnLocation = spawnPoints[randomNumber];
                // 3
                GameObject upgrade = Instantiate(upgradePrefab) as GameObject;
                Upgrade upgradeScript = upgrade.GetComponent<Upgrade>();
                upgradeScript.gun = gun;
                upgrade.transform.position = spawnLocation.transform.position;
                // 4
                spawnedUpgrade = true;
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.powerUpAppear);
            }
        }

        if (currentSpawnTime > generatedSpawnTime)
        {
            // Resets it so that aliens can spawn again
            currentSpawnTime = 0;

            // Spawn time randomizer
            generatedSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);

            if (aliensPerSpawn > 0 && aliensOnScreen < totalAliens)
            {
                // Creates an array to keep track of where enemies are spawned each wave
                List<int> previousSpawnLocations = new List<int>();

                // Limits aliens spawn by # of spawn points
                if (aliensPerSpawn > spawnPoints.Length)
                {
                    aliensPerSpawn = spawnPoints.Length - 1;
                }

                // Hard cap to prevent alien spawns exceeding spawners
                aliensPerSpawn = (aliensPerSpawn > totalAliens) ?
                aliensPerSpawn - totalAliens : aliensPerSpawn;

                // Spawning the aliens
                for (int i = 0; i < aliensPerSpawn; i++)
                {
                    if (aliensOnScreen < maxAliensOnScreen)
                    {

                        aliensOnScreen += 1;

                        // Loop runs until it finds spawn point
                        int spawnPoint = -1;
                        while (spawnPoint == -1)
                        {
                            // Calling upon the randomizer
                            int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                            if (!previousSpawnLocations.Contains(randomNumber))
                            {
                                previousSpawnLocations.Add(randomNumber);
                                spawnPoint = randomNumber;
                            }
                        }

                        GameObject spawnLocation = spawnPoints[spawnPoint];

                        GameObject newAlien = Instantiate(alien) as GameObject;

                        newAlien.transform.position = spawnLocation.transform.position;

                        Alien alienScript = newAlien.GetComponent<Alien>();

                        alienScript.target = player.transform;

                        Vector3 targetRotation = new Vector3(player.transform.position.x,
                        newAlien.transform.position.y, player.transform.position.z);
                        newAlien.transform.LookAt(targetRotation);
                    }
                }
            }
        }

    }
}
