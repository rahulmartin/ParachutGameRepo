using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class PopupManager : MonoBehaviour
{
    [SerializeField]
    GameObject popupUi;
    [SerializeField]
    Button button;
    [SerializeField]
    TextMeshProUGUI textDisplay;
    [SerializeField]
    TextMeshProUGUI buttonText;

    [SerializeField]
    string introText = "";
    [SerializeField]
    string gameEndText = "";
    [SerializeField]
    string gameEndByWrongClick = "";
    [SerializeField]
    string wonLevel = "";

    public enum PopUpType
    {
        INTRO,
        LOST_BY_LANDING,
        LOST_BY_WRONGANSWER,
        WON_LEVEL
    }

    private string textToShow;
    private UnityEngine.Events.UnityAction _callback;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        _callback?.Invoke();
        HidePopup();
    }

    public void HidePopup()
    {
        popupUi.gameObject.SetActive(false);
    }

    public void ShowPopup(PopUpType type, UnityEngine.Events.UnityAction callback, string buttonName = "", bool showButton = false)
    {
        _callback = callback;
        string text = "";
        switch (type)
        {
            case PopUpType.INTRO:
                text = introText;
                break;
            case PopUpType.LOST_BY_LANDING:
                text = gameEndText;
                break;
            case PopUpType.LOST_BY_WRONGANSWER:
                text = gameEndByWrongClick;
                break;
            case PopUpType.WON_LEVEL:
                text = wonLevel;
                break;
        }
        textToShow = text;
        buttonText.text = buttonName;
        if(showButton)
        {
            button.gameObject.SetActive(true);
        } else
        {
            button.gameObject.SetActive(false);
        }

        popupUi.SetActive(true);

        StartCoroutine(FillText());
    }

    private IEnumerator FillText()
    {
        StringBuilder show = new StringBuilder();
        for(int i=0; i<textToShow.Length; i++) {
            show.Append(textToShow[i]);
            textDisplay.text = show.ToString();
            yield return new WaitForSeconds(0.05f);
        }
    }
}
