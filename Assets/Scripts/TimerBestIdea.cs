using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerBestIdea : MonoBehaviour
{
    public static TimerBestIdea Instance;

    private bool _pause;
    private Text _textComponent;
    private TimeSpan _timeSpan;

    public TimeSpan TimeSpan
    {
        get { return _timeSpan; }
    }

    // Use this for initialization
    void Start()
    {
        Instance = this;
        _pause = true;
        _textComponent = GetComponent<Text>();
        _timeSpan = new TimeSpan();
    }

    // Update is called once per frame
    void Update()
    {
         if (_pause) return;
        _timeSpan = TimeSpan + new TimeSpan(0, 0, 0, 0, (int)(1000f * Time.deltaTime));
        if (_textComponent != null) _textComponent.text = string.Format("{0:d2}:{1:d2}", TimeSpan.Minutes, TimeSpan.Seconds);
    }

    public void ContinueTimer()
    {
        _pause = false;
    }
    
    public void StopTimer()
    {
        _pause = true;
    }

    public void ResetTimer()
    {
        _timeSpan = new TimeSpan();
        ContinueTimer();
    }
}
