using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class ButtonSoundManager : MonoBehaviour
{
    public AudioClip clickSound;
    private AudioSource audioSource;

    void Awake()
    {
        // 싱글턴 패턴 적용(이미 존재하면 파괴)
        if (FindObjectsOfType<ButtonSoundManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnEnable()
    {
        // 씬이 로드될 때마다 버튼들을 찾아 리스너를 추가
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 새로 로드된 씬의 모든 버튼에 클릭 사운드 리스너 추가
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button btn in buttons)
        {
            // 중복 등록을 방지하기 위해 기존 리스너를 제거하고 추가할 수도 있음
            btn.onClick.RemoveListener(PlayClickSound);
            btn.onClick.AddListener(PlayClickSound);
        }
    }

    // 버튼 클릭 시 호출되는 함수
    void PlayClickSound()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
