using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AsyncLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Slider loadingSlider;

    public void LoadGame(string level)
    {
        mainMenu.SetActive(false);
        loadingMenu.SetActive(true);
        StartCoroutine(LoadGameAsync(level));
    }

    IEnumerator LoadGameAsync(string level)
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(level);
        
        while(!loading.isDone)
        {
            float loadingVal = Mathf.Clamp01(loading.progress / 0.9f);
            loadingSlider.value = loadingVal;
            yield return null;
        }
    }
}
