using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform MainCamera;
    [SerializeField] private GameObject PlayerPrefab;
    public UnityEvent OnPause;
    public UnityEvent OnUnPause;
    private bool GamePaused;
    public void PlayerRespawn()
    {
        List<GameObject> Spawns = GameObject.FindGameObjectsWithTag("Spawn").ToList();
        Transform SpawnPos = Spawns[Random.Range(0, Spawns.Count)].transform;
        GameObject Pl = Instantiate(PlayerPrefab, SpawnPos.position, SpawnPos.rotation);
        Pl.GetComponent<Player>().CameraTransform = MainCamera;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPause.Invoke();
            PauseToggle();
        }
    }

    public void PauseToggle()
    {
        GamePaused = !GamePaused;
        Time.timeScale = GamePaused ? 0f : 1f;
        if (GamePaused) {
            OnPause?.Invoke();
            FindAnyObjectByType<Player>().enabled = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else {
            OnUnPause?.Invoke();
            FindAnyObjectByType<Player>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void LoadScene(string Name)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(Name, LoadSceneMode.Single);
    }
    public void Quit() => Application.Quit();

}
