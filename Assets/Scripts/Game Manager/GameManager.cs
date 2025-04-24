using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private SceneField sceneMenu;
    [SerializeField] private SceneField sceneMainMaps;

    private void StartGame()
    {
        SceneManager.LoadSceneAsync(sceneMenu, LoadSceneMode.Additive);
    }
}
