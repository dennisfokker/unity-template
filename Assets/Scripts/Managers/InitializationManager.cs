using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializationManager : MonoBehaviour
{
    private const string InitializationSceneName = "InitGame";
    private static string SceneToReload = "Game";

    void Start()
    {
        LeanTween.init();

        DelayUtil.WaitForFrame(InitializeGame);
    }

    [RuntimeInitializeOnLoadMethod]
    private static void ForceLoadInitializeGameScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName != InitializationSceneName)
            SceneToReload = currentSceneName;

        SceneManager.LoadScene(InitializationSceneName);
    }

    private static void InitializeGame()
    {
        SceneManager.LoadScene(SceneToReload);
    }
}
