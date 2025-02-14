using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;
    public Transform[] spawnPoints;
    public int maxNPCs = 4;
    public Camera playerCamera;
    public bool spawningEnabled = true; // Controla si los NPCs pueden aparecer

    private List<GameObject> activeNPCs = new List<GameObject>();
    private Transform lastUsedSpawnPoint = null;

    public AudioClip spawnSound;
    private AudioSource audioSource;

    public Image npcMarkerPrefab;
    private Dictionary<GameObject, Image> npcMarkers = new Dictionary<GameObject, Image>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No se encontró un AudioSource en el GameObject del NPCSpawner.");
        }

        for (int i = 0; i < maxNPCs; i++)
        {
            SpawnNPC();
        }
    }

    void LateUpdate()
    {
        UpdateNPCMarkers();
    }

    void SpawnNPC()
    {
        if (!spawningEnabled || activeNPCs.Count >= maxNPCs) return;

        List<Transform> availableSpawns = new List<Transform>(spawnPoints);
        if (lastUsedSpawnPoint != null) availableSpawns.Remove(lastUsedSpawnPoint);

        Transform selectedSpawn = availableSpawns[Random.Range(0, availableSpawns.Count)];

        GameObject npc = Instantiate(npcPrefab, selectedSpawn.position, Quaternion.identity);
        activeNPCs.Add(npc);
        lastUsedSpawnPoint = selectedSpawn;

        if (spawnSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(spawnSound);
        }

        if (npcMarkerPrefab != null)
        {
            Image marker = Instantiate(npcMarkerPrefab, FindObjectOfType<Canvas>().transform);
            npcMarkers[npc] = marker;
        }

        npc.GetComponent<NPC>().onInteract += () => HandleNPCInteraction(npc);
    }

    void HandleNPCInteraction(GameObject npc)
    {
        activeNPCs.Remove(npc);
        if (npcMarkers.ContainsKey(npc))
        {
            Destroy(npcMarkers[npc].gameObject);
            npcMarkers.Remove(npc);
        }
        Destroy(npc);
        SpawnNPC();
    }

    public void StopSpawning()
    {
        spawningEnabled = false;

        foreach (var npc in activeNPCs)
        {
            if (npc != null) Destroy(npc);
        }
        activeNPCs.Clear();

        foreach (var marker in npcMarkers.Values)
        {
            if (marker != null) Destroy(marker.gameObject);
        }
        npcMarkers.Clear();

        Debug.Log("El spawner ha sido desactivado.");
    }

    void UpdateNPCMarkers()
    {
        if (npcMarkerPrefab == null || playerCamera == null) return;

        foreach (var npc in activeNPCs)
        {
            if (npc == null) continue;
            if (!npcMarkers.ContainsKey(npc) || npcMarkers[npc] == null) continue;

            Image marker = npcMarkers[npc];
            Vector3 viewportPos = playerCamera.WorldToViewportPoint(npc.transform.position);

            if (viewportPos.z < 0)
            {
                marker.enabled = false;
                continue;
            }
            else
            {
                marker.enabled = true;
            }

            Vector3 screenPos = playerCamera.WorldToScreenPoint(npc.transform.position);
            screenPos.x = Mathf.Clamp(screenPos.x, 20, Screen.width - 20);
            screenPos.y = Mathf.Clamp(screenPos.y, 20, Screen.height - 20);
            marker.transform.position = Vector3.Lerp(marker.transform.position, screenPos, Time.deltaTime * 10);
        }
    }
}
