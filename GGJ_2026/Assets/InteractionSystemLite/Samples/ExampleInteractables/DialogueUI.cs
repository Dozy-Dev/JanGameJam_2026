using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float fadeSpeed = 10f;

    private float targetAlpha = 0;

    private void Update()
    {
        canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
    }

    public void ShowDialogue(string text)
    {
        dialogueText.text = text;
        targetAlpha = 1f;
    }

    public void HideDialogue()
    {
        targetAlpha = 0f;
    }
    public void ShowDialogueForSeconds(string text, float seconds)
    {
        ShowDialogue(text);
        CancelInvoke();
        Invoke(nameof(HideDialogue), seconds);
    }

}
