using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Initial loading state shown when game starts - loads main scene
/// </summary>
public class LoadingState : MonoBehaviour
{
    public Slider loadingBar;
    public Text loadingText;
    public string mainSceneName = "Main"; // Name of your main scene
    public float minimumLoadTime = 2.0f;

    private float m_LoadProgress = 0f;

    void Start()
    {
        StartCoroutine(LoadMainScene());
    }

    void Update()
    {
        // Update loading bar
        if (loadingBar != null)
            loadingBar.value = m_LoadProgress;

        if (loadingText != null)
            loadingText.text = $"Loading... {Mathf.FloorToInt(m_LoadProgress * 100)}%";
    }

    IEnumerator LoadMainScene()
    {
        float startTime = Time.time;

        // Start loading the main scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mainSceneName);
        asyncLoad.allowSceneActivation = false;

        // Update progress
        while (!asyncLoad.isDone)
        {
            float sceneProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            float timeProgress = Mathf.Clamp01((Time.time - startTime) / minimumLoadTime);

            // Use the slower of the two to ensure minimum time
            m_LoadProgress = Mathf.Min(sceneProgress, timeProgress);

            // When both are ready, activate the scene
            if (asyncLoad.progress >= 0.9f && (Time.time - startTime) >= minimumLoadTime)
            {
                m_LoadProgress = 1f;
                yield return new WaitForSeconds(0.3f);
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}