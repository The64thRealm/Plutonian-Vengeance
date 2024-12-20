using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(AudioSource))]
public class SoundsContainer : MonoBehaviour
{
    public static SoundsContainer instance;

    [SerializeField] private float musicVolume = 1;
    [SerializeField] private float sfxVolume = 0.5f;
    [SerializeField] private SoundsList[] soundLists;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private GameObject loopingAudioPrefab;

    private Dictionary<string, SoundsList> nameToPresetMap;

    private Dictionary<SoundSpecifierStrings, LoopedSoundItems> audioSourceMaps;

    private struct SoundSpecifierStrings
    {
        public string soundType;
        public string soundListName;

        public SoundSpecifierStrings(string soundType, string soundListName)
        {
            this.soundType = soundType;
            this.soundListName = soundListName;
        }
    }

    private struct LoopedSoundItems
    {
        public GameObject loopingAudioGameObject;
        public Coroutine loopingCoroutine;

        public LoopedSoundItems(GameObject loopingAudioGameObject, Coroutine loopingCoroutine)
        {
            this.loopingAudioGameObject = loopingAudioGameObject;
            this.loopingCoroutine = loopingCoroutine;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        transform.parent = null;
        DontDestroyOnLoad(gameObject);

        nameToPresetMap = new();
        foreach (var preset in soundLists)
        {
            nameToPresetMap[preset.name] = preset;
        }

        audioSourceMaps = new();
    }

    public void PlaySound(string soundType, string soundListName, float volume = -1)
    {
        if (volume < 0)
        {
            volume = (soundType == "Music") ? musicVolume : sfxVolume;
        }
        AudioClip clip = GetRandomSound(soundType, soundListName);
        if (clip != null)
        {
            audioSource.PlayOneShot(clip, volume);
        } else
        {
            Debug.LogWarning($"{soundType}.{soundListName} is not a valid sound");
        }
    }

    private AudioClip GetRandomSound(string soundType, string soundListName)
    {
        List<AudioClip> audioClipsToChooseFrom = nameToPresetMap[soundType].TryGetValue(soundListName);
        return audioClipsToChooseFrom == null ? null : audioClipsToChooseFrom[Random.Range(0, audioClipsToChooseFrom.Count)];
    }

    public void PlayLooping(string soundType, string soundListName, float volume = -1)
    {
        if (volume < 0)
        {
            volume = (soundType == "Music") ? musicVolume : sfxVolume;
        }

        SoundSpecifierStrings soundSpecifier = new SoundSpecifierStrings(soundType, soundListName);

        GameObject audioGameObject = Instantiate(loopingAudioPrefab);
        audioGameObject.transform.parent = transform;
        if (!audioSourceMaps.ContainsKey(soundSpecifier))
        {
            audioSourceMaps.Add(soundSpecifier, new LoopedSoundItems(audioGameObject, StartCoroutine(LoopSound(soundType, soundListName, audioGameObject.GetComponent<AudioSource>(), volume))));
        } //else
        //{
        //    StopExternalAudioSource(audioSourceMaps[soundSpecifier].loopingAudioGameObject);
        //}
    }

    public void StopLooping(string soundType, string soundListName)
    {
        SoundSpecifierStrings soundSpecifier = new SoundSpecifierStrings(soundType, soundListName);
        if (audioSourceMaps.ContainsKey(soundSpecifier))
        {
            LoopedSoundItems loopedSoundItems = audioSourceMaps[soundSpecifier];
            StopCoroutine(loopedSoundItems.loopingCoroutine);
            StopExternalAudioSource(loopedSoundItems.loopingAudioGameObject);

            audioSourceMaps.Remove(soundSpecifier);
            //List<Coroutine> coroutines = audioSourceMaps[soundSpecifier];
            //StopCoroutine(coroutines[coroutines.Count - 1]);
            //if (coroutines.Count <= 1)
            //{
            //    audioSourceMaps.Remove(soundSpecifier);
            //} else
            //{
            //    audioSourceMaps[soundSpecifier].RemoveAt(coroutines.Count - 1);
            //}
        }
    }

    private void StopExternalAudioSource(GameObject audioObject)
    {
        audioObject.GetComponent<AudioSource>().Stop();
        Destroy(audioObject);
    }

    private IEnumerator LoopSound(string soundType, string soundListName, AudioSource sourceToPlayFrom, float volume = 1)
    {
        // Wish I didn't need a coroutine, but to be able to choose a random sound every time, I need to use one
        AudioClip sound;
        sourceToPlayFrom.volume = volume;
        while (true)
        {
            sound = GetRandomSound(soundType, soundListName);
            if (sound == null)
            {
                Debug.LogWarning($"{soundType}.{soundListName} is not a valid sound");
                break;
            } else
            {
                sourceToPlayFrom.clip = sound;
                sourceToPlayFrom.Play();
            }
            yield return new WaitForSeconds(sound.length);
        }
    }
}