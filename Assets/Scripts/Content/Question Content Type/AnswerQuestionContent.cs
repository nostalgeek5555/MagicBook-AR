using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnswerQuestionContent : MonoBehaviour
{
    public int answerID;
    public string answer;
    public TextMeshProUGUI answerText;
    public Toggle answerToggle;

    public void answerInit(int _answerID, string _answer)
    {
        answerID = _answerID;
        answer = _answer;
        answerText.text = answer;
        answerToggle.group = transform.parent.GetComponent<ToggleGroup>();
    }
}
