using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using Cinemachine;
using Minimap;
public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] List<Character> characterProfiles = new List<Character>();
    [SerializeField] Transform spawnPosition;
    [SerializeField] CharacterController2D playerPrefab;
    [SerializeField] Camera playerCamera;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] HUDManager hudManager;
    CharacterController2D playerInstance;

    void Start(){
    }
    public void InitPlayer(CharacterName name)
    {

        playerInstance = Instantiate(playerPrefab, spawnPosition.position, Quaternion.identity);
        playerInstance.cam = playerCamera;
        virtualCamera.Follow = playerInstance.transform;
        StartCoroutine(PlayFX());
        var profile = characterProfiles.First(character => character.characterName.Equals(name));
        if (profile != null)
        {
            GameObject gfx = Instantiate(profile.spritePrefab,playerInstance.transform);
            playerInstance.GetComponent<CharacterController2D>().characterGFX = gfx.transform;
            hudManager.UpdatePlayerProfile(profile.profileSprite);
            playerInstance.transform.GetComponentInChildren<MinimapIcon>().GetComponent<SpriteRenderer>().sprite = profile.profileSprite;
        }

    }
    
    IEnumerator PlayFX()
    {
        GameObject sfxInstance = FindObjectsOfType<EffectPooler>()
                  .First(pooler => pooler.effectType == EffectType.Vortex)
                  .GetPooledObject();
        sfxInstance.transform.position = playerInstance.transform.position;
        sfxInstance.SetActive(true);

        yield return new WaitForSeconds(1f);
        sfxInstance.SetActive(false);
    }
    Character FindCharacter(CharacterName name)
    {

        return characterProfiles.First(character => character.name.Equals(name));


    }
}
