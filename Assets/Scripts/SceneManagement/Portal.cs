using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour, IPlayerTriggerable

{

    [SerializeField] int sceneToLoad = -1;
    [SerializeField] Transform spawnPoint;
    [SerializeField] int portalIndex = 0;
    [SerializeField] int portalToBeTeleported;
    PlayerController player;
    
    public void OnPlayerTriggered(PlayerController player)
    {
        this.player = player;
        StartCoroutine(SwitchScene(sceneToLoad));
    }
    IEnumerator SwitchScene(int sceneToLoad)
    {
        DontDestroyOnLoad(gameObject);
        yield return SceneManager.LoadSceneAsync(sceneToLoad);
        var scenePortals = FindObjectsOfType<Portal>();
        var destinationPortal = scenePortals[0];
        foreach(var scenePortal in scenePortals)
        {
        if (scenePortal.portalIndex == portalToBeTeleported)
            destinationPortal = scenePortal;
        break;
        }
        player.transform.position = destinationPortal.SpawnPoint.position;
        Destroy(gameObject);
    }

    public Transform SpawnPoint => spawnPoint;

}
