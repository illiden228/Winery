using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    //[SerializeField] private AudioMixer _masterMixer;
    
    [SerializeField] private AudioSource _simpleButton;
    [SerializeField] private AudioSource _barrel;
    [SerializeField] private AudioSource _buy;
    [SerializeField] private AudioSource _car;
    [SerializeField] private AudioSource _fonMusic;
    [SerializeField] private AudioSource _juice;
    [SerializeField] private AudioSource _plantGrow;
    [SerializeField] private AudioSource _sell;
    [SerializeField] private AudioSource _steps;


    public static SoundManager Instance;

    private const string Master = "master";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    //private void Start()
    //{
    //    TurnOnOffAudio(UserData.Instance.SoundOn);
    //}

    //public void TurnOnOffAudio(bool enable)
    //{
    //    _masterMixer.SetFloat(_master, enable ? 0f : -80f);
    //}

    public void PlayBarrel()
    {
        if (_barrel)
            _barrel.Play();
    }

    public void PlaySimpleButton()
    {
        if (_buy)
            _buy.Play();
    }

    public void PlayCar()
    {
        if (_car)
            _car.Play();
    }

    public void PlayJuice()
    {
        if (_juice)
            _juice.Play();
    }

    public void PlayPlantGrow()
    {
        if (_plantGrow)
            _plantGrow.Play();
    }

    public void PlaySell()
    {
        if (_sell)
            _sell.Play();        
    }

    public void ToggleSteps(bool enable)
    {
        if (_steps)
        {
            if (!_steps.isPlaying && enable)
                _steps.Play();
            else if (_steps.isPlaying && !enable)
                _steps.Pause();
        }
    }
}
