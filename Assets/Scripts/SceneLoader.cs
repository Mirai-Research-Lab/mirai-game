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
    private float target;
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        instance = this;
        DontDestroyOnLoad(this);
    }

    public async void LoadNextSceneAsync()
    {
        Time.timeScale = 1f;
        target = 0f;
        loadingSlider.value = 0f;
        var scene = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        scene.allowSceneActivation = false;
        currentSceneCanvas.SetActive(false);
        sceneLoadingCanvas.SetActive(true);
        do
        {
            await Task.Delay(100);
            target = scene.progress;
        } while (scene.progress < 0.9f);
        await Task.Delay(1000);
        scene.allowSceneActivation = true;
        sceneLoadingCanvas.SetActive(false);
    }

    public async void LoadSceneAsync(int sceneIndex)
    {
        Time.timeScale = 1f;
        target = 0f;
        loadingSlider.value = 0f;
        var scene = SceneManager.LoadSceneAsync(sceneIndex);
        scene.allowSceneActivation = false;
        currentSceneCanvas.SetActive(false);
        sceneLoadingCanvas.SetActive(true);
        do
        {
            await Task.Delay(100);
            target = scene.progress;
        } while (scene.progress < 0.9f);
        await Task.Delay(1000);
        scene.allowSceneActivation = true;
        sceneLoadingCanvas.SetActive(false);
    }

    public async void LoadMenuAsync()
    {
        Time.timeScale = 1f;
        target = 0f;
        loadingSlider.value = 0f;
        var scene = SceneManager.LoadSceneAsync("MenuPage");
        scene.allowSceneActivation = false;
        currentSceneCanvas.SetActive(false);
        sceneLoadingCanvas.SetActive(true);
        do
        {
            await Task.Delay(100);
            target = scene.progress;
        } while (scene.progress < 0.9f);
        await Task.Delay(1000);
        scene.allowSceneActivation = true;
        sceneLoadingCanvas.SetActive(false);
    }

    public void QuitGame()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
    private void Update()
    {
        loadingSlider.value = Mathf.MoveTowards(loadingSlider.value, target, 2f * Time.deltaTime);
    }
}
