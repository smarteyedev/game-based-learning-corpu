using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneControllerAbstract : MonoBehaviour
{
    [SerializeField] protected GameObject sceneCanvas;

    private void Start()
    {
        // Debug.Log($"start from: ");
    }

    public void ChangeScene(int sceneId)
    {
        sceneCanvas.SetActive(false);
        GameManager.instance.LoadScene(sceneId - 1);
    }
}
