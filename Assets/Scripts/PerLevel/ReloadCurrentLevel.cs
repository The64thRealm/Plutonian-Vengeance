using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using USCG.Core;

public class ReloadCurrentLevel : MonoBehaviour
{
    [Tooltip("Pressing the combination of these keys will reload all currently open scenes.")]
    [SerializeField]
    private List<KeyCode> KeyCodes = new()
        {
            KeyCode.LeftCommand,
            KeyCode.LeftShift,
            KeyCode.R
        };

    // This is the Singleton design pattern. Ensure there's only ever one of these components.
    private static ReloadCurrentLevel instance = null;

    // Only one reload operation should happen at a time because the unload/load operatins
    // are asynchronous.
    private static bool bIsReloadingScenes = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            // The reload process may create a new GameObject with this component. If that
            // happens, just destroy this component.
            DestroyImmediate(this);
        }
    }

    private void Update()
    {
        if (bIsReloadingScenes || KeyCodes.Count == 0)
        {
            return;
        }

        bool bAreKeyCodesPressed = true;
        foreach (KeyCode key in KeyCodes)
        {
            bAreKeyCodesPressed &= Input.GetKey(key);
        }

        if (bAreKeyCodesPressed)
        {
            ReloadLevel();
        }
    }


    public static void ReloadLevel()
    {
        instance.ReloadLevelInstance();
    }
    private void ReloadLevelInstance()
    {
        bIsReloadingScenes = true;
        StartCoroutine(ReloadAllOpenScenes());
    }
    
    private IEnumerator ReloadAllOpenScenes()
    {
        Debug.Log("Reloading scenes...");

        // Only one scene can be the active scene. Remember that scene, and record
        // the other scenes that are open.
        Debug.Log(GameManager.instance.currentLevelBuildID);
        string mostRecentLevel = SceneUtility.GetScenePathByBuildIndex(GameManager.instance.currentLevelBuildID);
        //List<string> nonActiveScenePaths = new();
        //for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; ++sceneIndex)
        //{
        //    string scenePath = SceneManager.GetSceneAt(sceneIndex).path;
        //    if (scenePath != mostRecentLevel)
        //    {
        //        nonActiveScenePaths.Add(scenePath);
        //    }
        //}

        //// Unload all scenes except the active scene.
        //Debug.Log("Unloadng scenes...");
        //foreach (string scenePath in nonActiveScenePaths)
        //{
        //    Debug.Log($"\tUnloading scene {scenePath}...");
        //    yield return SceneManager.UnloadSceneAsync(scenePath);
        //}

        // Reload all scenes. Load the active scene with the single mode, and load
        // all other scenes additively.
        Debug.Log("Loading scenes...");
        Debug.Log($"\tReloading active scene {mostRecentLevel}...");
        yield return SceneManager.LoadSceneAsync(mostRecentLevel, LoadSceneMode.Single);
        //foreach (string scenePath in nonActiveScenePaths)
        //{
        //    Debug.Log($"\tLoading scene {scenePath}...");
        //    yield return SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);
        //}

        Debug.Log("Finished reloading scenes!");

        // Update state to make sure we can reload again.
        bIsReloadingScenes = false;
    }
}
