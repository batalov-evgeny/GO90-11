using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Result90 : MonoBehaviour
{
    private static readonly string[] _result = { "You have a really quick eye!", "Very good observancy!", "Your watchfulness is moderate.", "You have below average observancy, but amazing patience!" };
    
    private void OnEnable()
    {
        var _textComponent = GetComponent<Text>();
        if (_textComponent == null) return;
        if (TimerBestIdea.Instance.TimeSpan.TotalMinutes < 10) _textComponent.text = _result[0];
        else
        {
            if (TimerBestIdea.Instance.TimeSpan.TotalMinutes < 15) _textComponent.text = _result[1];
            else
            {
                _textComponent.text = TimerBestIdea.Instance.TimeSpan.TotalMinutes < 20 ? _result[2] : _result[3];
            }

        }

    }
}
