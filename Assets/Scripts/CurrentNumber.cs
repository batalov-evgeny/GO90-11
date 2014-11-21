using UnityEngine;
using UnityEngine.UI;

public class CurrentNumber : MonoBehaviour
{
    private Text _textComponent;
    // Use this for initialization
    void Start()
    {
        _textComponent = GetComponent<Text>();
        NextOrAds.CurrentNumberAction += s => _textComponent.text = s;
    }
}
