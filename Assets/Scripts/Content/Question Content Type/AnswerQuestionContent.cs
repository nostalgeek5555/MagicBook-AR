using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnswerQuestionContent : MonoBehaviour
{
    public int answerID;
    public string answer;
    public TextMeshProUGUI answerText;
    public Toggle answerToggle;
    public ContentNode answerParent;

    private void OnDisable()
    {
        answerToggle.onValueChanged.RemoveAllListeners();
    }

    public void AnswerInit(int _answerID, string _answer)
    {
        answerID = _answerID;
        answer = _answer;
        answerText.text = answer;
        answerToggle.isOn = false;
        answerParent = transform.parent.parent.GetComponent<ContentNode>();
        answerToggle.group = transform.parent.GetComponent<ToggleGroup>();

        answerToggle.onValueChanged.RemoveAllListeners();
        answerToggle.onValueChanged.AddListener(delegate {
            OnAnswerPicked(answerToggle);
        });
    }

    public void OnAnswerPicked(Toggle toggle)
    {
        int questionID = ContentPanelUI.Instance.subchapterContentNodes.IndexOf(answerParent);

        if (answerID == answerParent._matchAnswerID)
        {
            if (toggle.isOn)
            {
                //answerParent.thisQuestionScore = 5;
                //answerParent.questionAnswered = true;
                ContentPanelUI.Instance.subchapterContentNodes[questionID].thisQuestionScore = 5;
                ContentPanelUI.Instance.subchapterContentNodes[questionID].questionAnswered = true;
            }

            else
            {
                ContentPanelUI.Instance.subchapterContentNodes[questionID].thisQuestionScore = 0;
                ContentPanelUI.Instance.subchapterContentNodes[questionID].questionAnswered = false;
            }
            
        }

        else
        {
            ContentPanelUI.Instance.subchapterContentNodes[questionID].thisQuestionScore = 0;
            ContentPanelUI.Instance.subchapterContentNodes[questionID].questionAnswered = true;
        }
    }
}
