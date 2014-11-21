using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class HintOnOff : MonoBehaviour
{
    public static Action OnHintOn;

    string[] hints = {"Help!", "I can't find it, really!!!", "OMG!!!", "Hint, please..." };
    Text textObject;
    int nextHint = 0;

    void OnEnable() {
        textObject = gameObject.GetComponentInChildren<Text>();
        if (textObject != null) textObject.text = hints[nextHint++];
        else Debug.LogError("textObject == null");
        if (nextHint == hints.Length) nextHint = 0;
    }

    public void OnClick()
    {
        var handleOn = OnHintOn;
        if (handleOn != null) handleOn();
//        AdMobController.Instance.OnClick();
        TimerBestIdea.Instance.StopTimer();
    }
}
