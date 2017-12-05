using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DelayUtil : MonoBehaviour
{
    private static DelayUtil _instance;

    static DelayUtil()
    {
        new GameObject("~DelayUtil", typeof(DelayUtil));
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    /// <summary>
    /// Wait until next frame and then execute callback.
    /// </summary>
    /// <param name="callback">Callback action</param>
    public static void WaitForFrame(Action callback)
    {
        _instance.StartCoroutine(_instance.WaitForFramesInternal(1, callback));
    }


    /// <summary>
    /// Wait for x frames and then execute callback.
    /// </summary>
    /// <param name="frames">Time to wait in frames</param>
    /// <param name="callback">Callback action</param>
    public static void WaitForFrames(int frames, Action callback)
    {
        _instance.StartCoroutine(_instance.WaitForFramesInternal(frames, callback));
    }

    /// <summary>
    /// Wait for x seconds and then execute callback.
    /// </summary>
    /// <param name="delay">Time to wait in seconds</param>
    /// <param name="callback">Callback action</param>
    public static void WaitForSeconds(float delay, Action callback)
    {
        _instance.StartCoroutine(_instance.WaitForSecondsInternal(delay, callback));
    }

    /// <summary>
    /// Load a scene with intermediate callbacks.
    /// </summary>
    /// <param name="sceneName">Scene name to load</param>
    /// <param name="loadSceneMode">Mode to load scene in</param>
    /// <param name="waitWhilePredicateBeforeStart">Wait while predicate is true before starting the scene</param>
    /// <param name="onStart">Start callback action</param>
    /// <param name="minimalLoadTime">Minimal loadtime</param>
    /// <param name="waitWhilePredicateBeforeComplete">Wait while predicate is true before completing</param>
    /// <param name="onComplete">Completion callback action</param>
    public static void LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode,
                                      Func<bool> waitWhilePredicateBeforeStart = null, Action onStart = null,
                                      float minimalLoadTime = 0,
                                      Func<bool> waitWhilePredicateBeforeComplete = null, Action onComplete = null)
    {
        _instance.StartCoroutine(_instance.LoadSceneAsyncInternal(SceneManager.GetSceneByName(sceneName), loadSceneMode,
                                                                  waitWhilePredicateBeforeStart, onStart,
                                                                  minimalLoadTime,
                                                                  waitWhilePredicateBeforeComplete, onComplete));
    }

    /// <summary>
    /// Load a scene with intermediate callbacks.
    /// </summary>
    /// <param name="sceneId">Scene id to load</param>
    /// <param name="loadSceneMode">Mode to load scene in</param>
    /// <param name="waitWhilePredicateBeforeStart">Wait while predicate is true before starting the scene</param>
    /// <param name="onStart">Start callback action</param>
    /// <param name="minimalLoadTime">Minimal loadtime</param>
    /// <param name="waitWhilePredicateBeforeComplete">Wait while predicate is true before completing</param>
    /// <param name="onComplete">Completion callback action</param>
    public static void LoadSceneAsync(int sceneId, LoadSceneMode loadSceneMode,
                                      Func<bool> waitWhilePredicateBeforeStart = null, Action onStart = null,
                                      float minimalLoadTime = 0,
                                      Func<bool> waitWhilePredicateBeforeComplete = null, Action onComplete = null)
    {
        _instance.StartCoroutine(_instance.LoadSceneAsyncInternal(SceneManager.GetSceneByBuildIndex(sceneId), loadSceneMode,
                                                                  waitWhilePredicateBeforeStart, onStart,
                                                                  minimalLoadTime,
                                                                  waitWhilePredicateBeforeComplete, onComplete));
    }

    /// <summary>
    /// Unload a scene and then execute callback.
    /// </summary>
    /// <param name="sceneName">Scenename to unload</param>
    /// <param name="callback">Callback action</param>
    public static void UnloadSceneAsync(string sceneName, Action callback)
    {
        _instance.StartCoroutine(_instance.UnloadSceneAsyncInternal(SceneManager.GetSceneByName(sceneName), callback));
    }

    /// <summary>
    /// Unload a scene and then execute callback.
    /// </summary>
    /// <param name="sceneName">Scenename to unload</param>
    /// <param name="callback">Callback action</param>
    public static void UnloadSceneAsync(int sceneId, Action callback)
    {
        _instance.StartCoroutine(_instance.UnloadSceneAsyncInternal(SceneManager.GetSceneByBuildIndex(sceneId), callback));
    }

    /// <summary>
    /// Evaluate predicate until true and then execute callback.
    /// </summary>
    /// <param name="predicate">Predicate to evaluate</param>
    /// <param name="callback">Callback action</param>
    public static void WaitUntil(Func<bool> predicate, Action callback)
    {
        _instance.StartCoroutine(_instance.WaitUntilInternal(predicate, callback));
    }

    /// <summary>
    /// Evaluate predicate while true and then execute callback.
    /// </summary>
    /// <param name="predicate">Predicate to evaluate</param>
    /// <param name="callback">Callback action</param>
    public static void WaitWhile(Func<bool> predicate, Action callback)
    {
        _instance.StartCoroutine(_instance.WaitWhileInternal(predicate, callback));
    }

    private IEnumerator WaitForFramesInternal(int frames, Action callback)
    {
        while (frames > 0)
        {
            yield return null;
            frames--;
        }

        if (callback != null)
            callback.Invoke();
    }

    private IEnumerator WaitForSecondsInternal(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);

        if (callback != null)
            callback.Invoke();
    }

    private IEnumerator LoadSceneAsyncInternal(Scene scene, LoadSceneMode loadSceneMode,
                                               Func<bool> waitWhilePredicateBeforeStart, Action onStart,
                                               float minimalLoadTime,
                                               Func<bool> waitWhilePredicateBeforeComplete, Action onComplete)
    {
        // Load up the scene.
        AsyncOperation async = SceneManager.LoadSceneAsync(scene.buildIndex, LoadSceneMode.Additive);
        async.allowSceneActivation = false;

        // Wait for the predicate before starting the timer. Invoke onStart callback action.
        if (waitWhilePredicateBeforeStart != null)
            yield return WaitWhileInternal(waitWhilePredicateBeforeStart, null);
        if (onStart != null)
            onStart.Invoke();

        // Set start time.
        double startTime = new TimeSpan(DateTime.Now.Ticks).TotalMilliseconds;

        // Wait until preparing of scene is ready (awake/start still has to be executed).
        while (async.progress < 0.9f)
            yield return null;

        // Start final loading of scene.
        async.allowSceneActivation = true;

        // Wait until scene fully loaded.
        while (!async.isDone)
            yield return async;

        // Set end time.
        double endTime = new TimeSpan(DateTime.Now.Ticks).TotalMilliseconds;

        // Make sure logos have been visible for at least a second.
        if (endTime - startTime < minimalLoadTime)
            yield return new WaitForSeconds(1 - (float)(endTime - startTime) / minimalLoadTime);

        // Wait for the predicate before invoking the onComplete callback action.
        if (waitWhilePredicateBeforeComplete != null)
            yield return WaitWhileInternal(waitWhilePredicateBeforeComplete, null);
        if (onComplete != null)
            onComplete.Invoke();
    }

    private IEnumerator UnloadSceneAsyncInternal(Scene scene, Action callback)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(scene);

        while (!ao.isDone)
            yield return null;

        if (callback != null)
            callback();
    }

    private IEnumerator WaitUntilInternal(Func<bool> predicate, Action callback)
    {
        yield return new WaitUntil(predicate);

        if (callback != null)
            callback();
    }

    private IEnumerator WaitWhileInternal(Func<bool> predicate, Action callback)
    {
        yield return new WaitWhile(predicate);

        if (callback != null)
            callback();
    }
}
