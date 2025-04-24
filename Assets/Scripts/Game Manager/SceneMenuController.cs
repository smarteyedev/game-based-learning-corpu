using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMenuController : MonoBehaviour
{
    [SerializeField] private GameObject sceneCanvas;

    private void Start()
    {
        // Debug.Log($"start from: ");
    }

    public void ChangeScene(int sceneId)
    {
        sceneCanvas.SetActive(false);
        GameManager.instance.LoadScene();
    }
}
