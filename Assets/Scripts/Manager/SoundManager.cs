using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
                if (instance != null)
                {
                    GameObject obj = new GameObject("SoundManager");
                    instance = obj.AddComponent<SoundManager>();
                }
            }
            return instance;
        }
    }

    [SerializeField] private List<Button> buttons;

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private AudioClip clickSoundClip;

    [SerializeField] private float bgmVolume;
    [SerializeField] private float sfxVolume;

    static SoundManager()
    {
      
    }

    private void LoadSavedSound()
    {
        if (SaveLoadManager.LoadBase())
        {
            bgmVolume = SaveLoadManager.BaseData.bgmVolume;
            sfxVolume = SaveLoadManager.BaseData.sfxVolume;
        }
    }

    public float BgmVolume
    {
        get { return bgmVolume; }
        set
        {
            bgmVolume = Mathf.Clamp01(value);
            bgmSource.volume = bgmVolume;
        }
    }
    public float SfxVolume
    {
        get { return sfxVolume; }
        set
        {
            sfxVolume = Mathf.Clamp01(value);
            sfxSource.volume = sfxVolume;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        LoadSavedSound();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        if (bgmClip != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;
            bgmSource.volume = bgmVolume;
            bgmSource.Play();
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (buttons == null)
        {
            buttons = new List<Button>();
        }
        buttons.Clear();
        buttons = FindObjectsOfType<Button>(true).ToList();
        foreach (Button btn in buttons)
        {
            btn.onClick.RemoveListener(PlayClickSound);
            btn.onClick.AddListener(PlayClickSound);
        }
        var npcs = FindObjectsOfType<NpcButton>(true).ToList();
        foreach (var npc in npcs)
        {
            npc.onClick.RemoveListener(PlayClickSound);
            npc.onClick.AddListener(PlayClickSound);
        }
    }

    private void PlayClickSound()
    {
        if (clickSoundClip != null)
        {
            sfxSource.PlayOneShot(clickSoundClip, SfxVolume);
        }
    }

    public void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip, SfxVolume);
    }
}