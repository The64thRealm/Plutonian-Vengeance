using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFX : MonoBehaviour
{
    public GameObject gameObjectToDisable;

    public GameObject deathMessage;

    [Header("Camera Zoom Settings")]

    public Camera cam;
    public float scale;
    public float speed;

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void DeactivateObject()
    {
        gameObjectToDisable.SetActive(false);
    }

    public void CameraPanOut()
    {
        StartCoroutine(CameraPanTime(5.0f, scale, speed));
    }

    //copied from https://discussions.unity.com/t/orthographic-camera-size-lerping/180742
    private IEnumerator CameraPanTime(float oldSize, float newSize, float time)
    {
        float elapsed = 0;
        while (elapsed <= time)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / time);

            cam.orthographicSize = Mathf.Lerp(oldSize, newSize, t);
            yield return null;
        }
    }

    public void DeathMessage()
    {
        deathMessage.SetActive(true);
    }

}
