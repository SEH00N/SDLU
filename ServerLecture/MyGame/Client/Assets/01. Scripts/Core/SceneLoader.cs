using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance = null;
    
    public void LoadSceneAsync(string sceneName, Action onCompleted = null)
    {
        StartCoroutine(LoadAsyncCoroutine(sceneName, onCompleted));
    }

    private IEnumerator LoadAsyncCoroutine(string sceneName, Action onCompleted)
    {
        AsyncOperation oper = SceneManager.LoadSceneAsync(sceneName);
        YieldInstruction delay = new WaitForSeconds(0.1f);
        
        while(true)
        {
            if(oper.isDone)
                break;

            yield return delay;
        }

        onCompleted?.Invoke();
    }
}
