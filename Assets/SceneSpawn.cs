using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSpawn : MonoBehaviour
{
    public Transform spawnPoint;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
            player.transform.position = spawnPoint.position;
    }
}
