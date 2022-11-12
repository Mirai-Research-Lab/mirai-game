using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;
    [SerializeField] private GameObject currentSceneCanvas;
    [SerializeField] private GameObject sceneLoadingCanvas;
    [SerializeField] private Slider loadingSlider;
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        instance = this;
        DontDestroyOnLoad(this);
    }

    public async void LoadNextSceneAsync()
    {
        var scene = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        scene.allowSceneActivation = false;
        currentSceneCanvas.SetActive(false);
        sceneLoadingCanvas.SetActive(true);
        do
        {
            await Task.Delay(100);
            loadingSlider.value = scene.progress;
        } while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        sceneLoadingCanvas.SetActive(false);
    }
}
