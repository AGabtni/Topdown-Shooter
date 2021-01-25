using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[System.Serializable]
public class Dialogue
{
    public float fontSize;
    public string speakerName;

    [TextArea(3, 10)]
    public string[] sentences;



}
public class DialogueManager : GenericSingletonClass<DialogueManager>
{
    // Start is called before the first frame update
    public GameObject dialogueBox;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText;
    public Animator animator;
    private Queue<string> sentences;
    void Awake()
    {
        base.Awake();
        sentences = new Queue<string>();
        dialogueBox.SetActive(false);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueBox.SetActive(true);

        animator.SetBool("IsOpen", true);

        if (nameText != null) nameText.text = dialogue.speakerName;

        sentences.Clear();

        dialogText.fontSize = dialogue.fontSize;

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();

    }
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

    }

    IEnumerator TypeSentence(string sentence)
    {

        dialogText.text = "";
        foreach (char c in sentence)
        {

            dialogText.text += c;
            yield return null;

        }
    }
    public void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        dialogueBox.SetActive(false);

    }

    public bool IsDialogueOpen()
    {
        return animator.GetBool("IsOpen");
    }
}
