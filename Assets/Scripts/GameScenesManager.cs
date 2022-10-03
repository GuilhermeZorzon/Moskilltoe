using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameScenesManager : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void LoadSceneAsyncMode(string sceneName)
    {
        StartCoroutine(LoadYourAsyncScene(sceneName));
    }

    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

     public void UnloadSceneAsyncMode(int activeSceneBuildIndex)
    {        
        StartCoroutine(UnloadYourAsyncScene(activeSceneBuildIndex));
    }

    IEnumerator UnloadYourAsyncScene(int activeSceneBuildIndex)
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(activeSceneBuildIndex);
        yield return asyncUnload;
    }
}
