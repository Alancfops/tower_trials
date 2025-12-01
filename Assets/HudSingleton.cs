using UnityEngine;

public class HUDSingleton : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] h = GameObject.FindGameObjectsWithTag("HUD");

        if (h.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
