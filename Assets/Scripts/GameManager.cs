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
    [SerializeField] private bool CanWinOrLose;
    [SerializeField] private int PlayerStartLifesCount;
    public int PlayerLifesCount {  get; private set; }
    public UnityEvent OnPause;
    public UnityEvent OnUnPause;
    public UnityEvent OnPlWin;
    public UnityEvent OnPlLose;
    private bool GamePaused;

    private void Start()
    {
        PlayerLifesCount = PlayerStartLifesCount;
    }
    public void PlayerRespawn()
    {
        List<GameObject> Spawns = GameObject.FindGameObjectsWithTag("Spawn").ToList();
        Transform SpawnPos = Spawns[Random.Range(0, Spawns.Count)].transform;
        GameObject Pl = Instantiate(PlayerPrefab, SpawnPos.position, SpawnPos.rotation);
        Pl.GetComponent<Player>().CameraTransform = MainCamera;
    }

    public void OnPlayerDied() => PlayerLifesCount -= 1;
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPause.Invoke();
            PauseToggle();
        }
        if (CanWinOrLose)
        {
            if (PlayerLifesCount <= 0)
            {
                OnPlLose?.Invoke();
            }
            if (FindObjectsOfType<Enemy>().Where(Enemy => Enemy.enabled).ToArray().Length <= 0) {
                OnPlWin?.Invoke();
                FindAnyObjectByType<Player>().enabled = false;
                Cursor.lockState = CursorLockMode.Confined;
            }
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
