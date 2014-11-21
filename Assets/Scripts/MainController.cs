using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MainController : MonoBehaviour
{
    public static MainController Instance;
    public GameObject FinishScreen;
    public GameObject ResumeButton;
    public GameObject PauseButton;
    public GameObject Hint;
    public int HintEnableSeconds;
    public AudioSource TapAudioSource;
    public AudioSource HaHaAudioSource; 
    public AudioSource GiggleAudioSource;
    public GameObject TextGameObject;
    public string ButtonsTag;
    TimeSpan hintTimer;
    // Use this for initialization
    void OnEnable()
    {
        Instance = this;
        hintTimer = new TimeSpan();
        NextOrAds.CurrentNumberAction += (s) => hintTimer = new TimeSpan();
        if (FinishScreen == null)
            Debug.LogError("FinishScreen");
        if (ResumeButton == null)
            Debug.LogError("ResumeButton");
        if (PauseButton == null)
            Debug.LogError("PauseButton");
        if (Hint == null)
            Debug.LogError("Hint");
        if (TapAudioSource == null)
            Debug.LogError("TapAudioSource");
        if (HaHaAudioSource == null)
            Debug.LogError("HaHaAudioSource");
        if (GiggleAudioSource == null)
            Debug.LogError("GiggleAudioSource");
        if (TextGameObject == null)
            Debug.LogError("Text");
        if (ButtonsTag.Length == 0)
            Debug.LogError("Tag");

        HintOnOff.OnHintOn += NextOrAds.ShowNextNumber;
        StartCoroutine(SetNumbers());
    }

    private IEnumerator SetNumbers()
    {
        var buttons = GameObject.FindGameObjectsWithTag(ButtonsTag);
        var set = new List<int>();

        for (var i = 0; i < buttons.Length; i++) set.Add(i);

        for (var i = 0; i < buttons.Length; i++)
        {
            var index = UnityEngine.Random.Range(0, set.Count);
            NextOrAds.Buttons[i] = buttons[i].GetComponent<NextOrAds>();
            NextOrAds.Buttons[i].SetNumber(set[index]);
            set.RemoveAt(index);

            yield return new WaitForSeconds(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        hintTimer += new TimeSpan(0, 0, 0, 0, (int)(1000f * Time.deltaTime));
        if (hintTimer.Seconds > HintEnableSeconds)
            Hint.SetActive(true);
    }

    public void PlayCorrect()
    {
        TapAudioSource.Play();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PlayWrong()
    {
        HaHaAudioSource.Play();
    }

    public void Giggle()
    {
        GiggleAudioSource.Play();
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("Found", NextOrAds.Found);
    }
    public void LoadGame()
    {
        NextOrAds.Found = PlayerPrefs.GetInt("Found", 0);

        var handler = NextOrAds.CurrentNumberAction;
        if (handler != null) handler(NextOrAds.Found.ToString());
    }
}
