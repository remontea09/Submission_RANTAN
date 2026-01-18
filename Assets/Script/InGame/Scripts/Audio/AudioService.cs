using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioService : Base.SingletonMonoBehaviour<AudioService> {
    [SerializeField] private GameObject _SEPoolParent;
    [Header("SE")]
    [SerializeField] private AudioClip _walkSE;
    [SerializeField] private AudioClip _attackSE;
    [SerializeField] private AudioClip _enemyDeathSE;
    [SerializeField] private AudioClip _takeDamageSE;
    [SerializeField] private AudioClip _flowerSE;
    [SerializeField] private AudioClip _clearSE;
    [SerializeField] private AudioClip _levelUpSE;
    [Header("BGM")]
    [SerializeField] private AudioSource _BGMSource;
    [SerializeField] private AudioClip _titleBGM;
    [SerializeField] private AudioClip _homeBGM;
    [SerializeField] private AudioClip _storyBGM;
    [SerializeField] private AudioClip _titleClickSE;
    [SerializeField] private AudioClip _homeClickSE;

    [Header("Mixer")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioMixerGroup _BGMGroup;
    [SerializeField] private AudioMixerGroup _SEGroup;

    private ObjectPool<AudioSource> _audioSourcePool;

    //volumeの最低値
    private readonly static float MIN_VOLUME = -70f;
    private readonly static float MAX_VOLUME = 0f;
    private readonly static float FADE_STEP = 5f;
    private readonly static float FADE_INTERVAL = 0.2f;

    private readonly static float OUTGAME_VOLUME = -5f; //アウトゲームではBGMをMAX_VOLUMEに設定するとデカすぎる

    protected override void Awake() {
        base.Awake();

        _audioSourcePool = new ObjectPool<AudioSource>(
            createFunc: () => {
                var audioSource = _SEPoolParent.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.outputAudioMixerGroup = _SEGroup;
                return audioSource;
            },
            actionOnGet: (obj) => {
                obj.enabled = true;
            },
            actionOnRelease: (obj) => {
                obj.clip = null;
                obj.enabled = false;
            }
        );
        _BGMSource.outputAudioMixerGroup = _BGMGroup;
    }


    /// <summary>
    /// フロアによってBGMを変更
    /// </summary>
    public void InitInGameAudio() {
        var bgm = Dao.Master.DungeonMaster[GameSessionService.Instance.DungeonDataId].Bgm;

        if (GameSessionService.Instance.CurrentDungeonType == DungeonType.Normal) {
            _BGMSource.clip = bgm;
        }
        else if (GameSessionService.Instance.CurrentDungeonType == DungeonType.Boss) {
            //temp
            _BGMSource.clip = bgm;
        }

        _audioMixer.SetFloat("BGM", MIN_VOLUME);
        _BGMSource.Play();
        StartCoroutine(FadeIn());
    }

    public void InitTitleAudio() {
        _BGMSource.clip = _titleBGM;

        _audioMixer.SetFloat("BGM", OUTGAME_VOLUME);
        _BGMSource.Play();
    }

    public void InitHomeAudio() {
        _BGMSource.clip = _homeBGM;

        _audioMixer.SetFloat("BGM", OUTGAME_VOLUME);
        _BGMSource.Play();
    }

    public void InitStoryAudio() {
        _BGMSource.clip = _storyBGM;

        _audioMixer.SetFloat("BGM", OUTGAME_VOLUME);
        _BGMSource.Play();
    }


    /// <summary>
    /// 外部からAudioTypeを受け取るだけで音声を再生する
    /// </summary>
    /// <param name="type">音声の種類</param>
    public void PlaySE(AudioType type) {
        switch (type) {
            case AudioType.WalkSE:
                PlaySound(_walkSE);
                break;
            case AudioType.AttackSE:
                PlaySound(_attackSE);
                break;
            case AudioType.TakeDamageSE:
                PlaySound(_takeDamageSE);
                break;
            case AudioType.EnemyDeathSE:
                PlaySound(_enemyDeathSE);
                break;
            case AudioType.TitleClickSE:
                PlaySound(_titleClickSE);
                break;
            case AudioType.HomeClickSE:
                PlaySound(_homeClickSE);
                break;
        }
    }

    public void PlayAcquireFlowerSE() => PlaySound(_flowerSE);
    public void PlayOnClearSE() => PlaySound(_clearSE);
    public void PlayLevelUpSE() => PlaySound(_levelUpSE);


    /// <summary>
    /// 実際に音声を鳴らす
    /// </summary>
    /// <param name="clip">音声の実際のクリップ</param>
    private void PlaySound(AudioClip clip) {
        AudioSource source = _audioSourcePool.Get();
        source.clip = clip;
        source.Play();
        StartCoroutine(ReleaseAudioSource(source));
    }


    /// <summary>
    /// オブジェクトプールに再生を待ってからAudioSourceを返す
    /// </summary>
    /// <param name="source">使われているAudioSource</param>
    private IEnumerator ReleaseAudioSource(AudioSource source) {
        yield return new WaitWhile(() => source.isPlaying);
        _audioSourcePool.Release(source);
    }

    /// <summary>
    /// 音声をフェードインさせる
    /// </summary>
    private IEnumerator FadeIn() {
        var volume = MIN_VOLUME;
        while (volume < MAX_VOLUME) {
            volume += FADE_STEP;
            _audioMixer.SetFloat("BGM", volume);
            yield return new WaitForSeconds(FADE_INTERVAL);
        }
    }
}
