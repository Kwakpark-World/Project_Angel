using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    private readonly int _isRotatingHash = Animator.StringToHash("IsRotating");

    [SerializeField]
    private Image _fadePanel;
    [SerializeField]
    private Image _loadingCircle;
    [SerializeField]
    private float _fadeDuration;
    public PlayerHUD PlayerHUDProperty { get; private set; }
    public GameOverUI GameOverUIProperty { get; private set; }
    private Dictionary<string, PopupUI> _popups = new Dictionary<string, PopupUI>();
    private Animator _loadingCircleAnimator;

    protected override void Awake()
    {
        base.Awake();

        foreach (PopupUI popup in FindObjectsOfType<PopupUI>())
        {
            _popups.Add(popup.GetType().Name.Replace(typeof(PopupUI).Name, ""), popup);
            popup.TogglePopup(false);
        }

        _loadingCircleAnimator = _loadingCircle.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            TogglePopupUniquely("Inventory");
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePopupUniquely("Pause");
        }
    }

    public void LoadScene(string sceneName)
    {
        _fadePanel.DOFade(1f, _fadeDuration)
            .OnStart(() =>
            {
                _fadePanel.raycastTarget = true;
            })
            .OnComplete(() =>
            {
                StartCoroutine(LoadSceneAsync(sceneName));
            });

        _loadingCircle.DOFade(1f, _fadeDuration)
            .OnStart(() =>
            {
                _loadingCircleAnimator.SetBool(_isRotatingHash, true);
            });
    }

    public void TogglePopupUniquely(string popupName)
    {
        foreach (var popup in _popups)
        {
            bool popupToggleValue = popup.Key == popupName && !popup.Value.gameObject.activeInHierarchy;

            popup.Value.TogglePopup(popupToggleValue);

            if (GameManager.Instance.HasPlayer)
            {
                if (popupToggleValue)
                {
                    GameManager.Instance.PlayerInstance.StopImmediately(true);
                }

                if (popup.Key == popupName)
                {
                    if (popupToggleValue)
                    {
                        GameManager.Instance.PlayerInstance.IsPlayerStop = PlayerControlEnum.Stop;
                    }
                    else
                    {
                        if (!GameManager.Instance.PlayerInstance.BuffCompo.GetBuffState(BuffType.Potion_Paralysis))
                        {
                            GameManager.Instance.PlayerInstance.IsPlayerStop = PlayerControlEnum.Move;
                        }
                    }

                    CameraManager.Instance._currentCam.IsCamRotateStop = popupToggleValue;

                    CameraManager.Instance.SetCursor(popupToggleValue);
                }
            }
        }
    }

    public void Quit()
    {
        _fadePanel.DOFade(1f, _fadeDuration)
            .OnStart(() =>
            {
                _fadePanel.raycastTarget = true;
            })
            .OnComplete(() =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            });
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;
        float pastTime = 0f;
        float percentage = 0f;

        while (!async.isDone)
        {
            yield return null;

            pastTime += Time.deltaTime;

            if (percentage >= 90f)
            {
                percentage = Mathf.Lerp(percentage, 100, pastTime);

                if (percentage >= 100f)
                {
                    async.allowSceneActivation = true;
                }
            }
            else
            {
                percentage = Mathf.Lerp(percentage, async.progress * 100f, pastTime);

                if (percentage >= 90f)
                {
                    pastTime = 0f;
                }
            }
        }
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        _fadePanel.DOFade(0f, _fadeDuration)
            .OnComplete(() =>
            {
                _fadePanel.raycastTarget = false;
            });

        _loadingCircle.DOFade(0f, _fadeDuration)
            .OnComplete(() =>
            {
                _loadingCircleAnimator.SetBool(_isRotatingHash, false);
            });

        switch (scene.name)
        {
            case "IntroScene":
                SoundManager.Instance.ChangeBGMMode(BGMMode.Intro);

                break;

            case "GameScene":
                SoundManager.Instance.ChangeBGMMode(BGMMode.NonCombat);

                break;

            default:
                break;
        }

        if (GameManager.Instance.HasPlayer)
        {
            PlayerHUDProperty = FindObjectOfType<PlayerHUD>();
            PlayerHUDProperty.PlayerReference = GameManager.Instance.PlayerInstance;
            GameOverUIProperty = FindObjectOfType<GameOverUI>();
        }
    }
}
