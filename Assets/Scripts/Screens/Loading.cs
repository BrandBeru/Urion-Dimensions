using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string levelToLoad = Loader.nextLevel;
        StartCoroutine(this.StartLoadLevel(levelToLoad));
    }
    IEnumerator StartLoadLevel(string level)
    {
        AsyncOperation op =  SceneManager.LoadSceneAsync(level);
        while(!op.isDone)
            yield return new WaitForEndOfFrame();
    }
}
