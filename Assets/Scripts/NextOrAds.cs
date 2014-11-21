using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngineInternal;

public class NextOrAds : MonoBehaviour
{
    static int _currentButtonNumber;

    public const int TotalNumbers = 90;	// у нас ровно 90 чисел в таблице
    public static NextOrAds[] Buttons = new NextOrAds[TotalNumbers];
    public static Color ColorCorrect = new Color(0, 1, 0, 0.5f);
    public static Color ColorWrong = new Color(1, 0, 0, 0.5f);
    public static Color Invisible = new Color(0, 0, 0, 0f);
    public static Action<string> CurrentNumberAction; // передает событие о смене текущего числа

    public GameObject MyNumberGameObject;
    int _myNumber;

    Image _buttonImage; // кнопка статична, привязана к графике

    void Start()
    {
        _buttonImage = gameObject.GetComponent<Image>();
        var button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(OnClick);

    }

    public static int Found
    {
        get { return _currentButtonNumber; }
        set { _currentButtonNumber = value; }
    }
    void CreateText()
    {
        MyNumberGameObject = Instantiate(MainController.Instance.TextGameObject) as GameObject;
        if (MyNumberGameObject == null) { Debug.LogError("Can't Instantiate"); return; }
        MyNumberGameObject.transform.SetParent(transform);
        var myTr = transform as RectTransform;
        var tr = MyNumberGameObject.transform as RectTransform;
        tr.localPosition = Vector3.zero;
        var square = Mathf.Min(myTr.rect.width, myTr.rect.height);
        tr.sizeDelta = new Vector2(square, square);
        float x = 1f, y = 1f;
        if (myTr.rect.width > myTr.rect.height) x = myTr.rect.width / myTr.rect.height;
        else y = myTr.rect.height / myTr.rect.width;
        tr.localScale = new Vector3(x, y, 1f);
        var textComponent = MyNumberGameObject.GetComponent<Text>();
        textComponent.text = string.Format("{0}", _myNumber + 1);
    }

    public static void ShowNextNumber()
    {
        foreach (var button in Buttons.Where(button => button._myNumber == _currentButtonNumber))
        {
            button.AnimateNumber();
            break;
        }
    }

    public void AnimateNumber()
    {
        StartCoroutine(AnimateNumberCoroutine());
    }

    private IEnumerator AnimateNumberCoroutine()
    {
        MainController.Instance.Giggle();

        var textComponent = MyNumberGameObject.GetComponent<Text>();
        var scale = MyNumberGameObject.transform.localScale;
        var tr = MyNumberGameObject.transform as RectTransform;
        for (var i = 0; i < 21; i++)
        {
            textComponent.color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f),
                UnityEngine.Random.Range(0f, 1f));
            MyNumberGameObject.transform.localPosition += new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f)) * tr.rect.width;
            MyNumberGameObject.transform.localScale *= UnityEngine.Random.Range(0.9f, 1.1f);
            yield return new WaitForSeconds(0.01f);
        }
        textComponent.color = Color.black;
        MyNumberGameObject.transform.localPosition = Vector3.zero;
        MyNumberGameObject.transform.localScale = scale;
    }

    public void SetNumber(int n)
    {
        _myNumber = n;
        if (MyNumberGameObject == null) CreateText();
    }

    public void OnClick()
    {
        StartCoroutine(Tapped());
        if (_myNumber == _currentButtonNumber)
        {
            var handler = CurrentNumberAction;
            if (handler != null) handler((_myNumber + 1).ToString());

            TimerBestIdea.Instance.ContinueTimer();
            MainController.Instance.Hint.SetActive(false);
            MainController.Instance.PlayCorrect();

            _currentButtonNumber++;

            if (_myNumber == TotalNumbers - 1)
            {
                _currentButtonNumber = 0;
                MainController.Instance.FinishScreen.SetActive(true);
                MainController.Instance.ResumeButton.SetActive(false);
                MainController.Instance.PauseButton.SetActive(false);
                TimerBestIdea.Instance.StopTimer();
            }
        }
        else
        {
            TimerBestIdea.Instance.StopTimer();
            StartCoroutine(SoundWrongThanAds());
        }
    }

    private IEnumerator Tapped()
    {
        _buttonImage.color = _myNumber == _currentButtonNumber ? ColorCorrect : ColorWrong;
        yield return new WaitForSeconds(1);
        _buttonImage.color = Invisible;
    }

    private IEnumerator SoundWrongThanAds()
    {
        MainController.Instance.PlayWrong();
        yield return new WaitForSeconds(MainController.Instance.HaHaAudioSource.clip.length);
        AdMobController.Instance.OnClick();
    }
}
