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

    public void AnswerInit(int _answerID, string _answer)
    {
        answerID = _answerID;
        answer = _answer;
        answerText.text = answer;
        answerToggle.isOn = false;
        answerParent = transform.parent.parent.GetComponent<ContentNode>();
        Debug.Log("question parent " + transform.root.name);
        answerToggle.group = transform.parent.GetComponent<ToggleGroup>();
    }

    public void OnAnswerPicked()
    {
        if (answerID == answerParent._matchAnswerID)
        {

            if (answerToggle.isOn)
            {
                answerParent.thisQuestionScore = 5;
            }
            else
            {
                answerParent.thisQuestionScore = 0;
            }
            
        }

        else
        {
            answerParent.thisQuestionScore = 0;
        }
    }
}
