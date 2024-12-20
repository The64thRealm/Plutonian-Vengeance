using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnSpawn : MonoBehaviour
{
    [SerializeField] private string soundString;

    private void OnEnable()
    {
        SoundHandler.PlaySound(soundString);
    }
}
