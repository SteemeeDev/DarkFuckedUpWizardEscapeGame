using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] float textSpeed = 20;

    Image textBox;

    [SerializeField] DialogueObj[] allDialogue;
    [SerializeField] TMP_Text dialogueText;

    AudioSource audioSource;
    [SerializeField] AudioClip[] audios;

    private void Start()
    {
        textBox = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();  
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            StopAllCoroutines();
            StartCoroutine(ReadDialogue(0));
        }
    }

    public IEnumerator ReadDialogue(int index)
    {
        textBox.enabled = true;
        dialogueText.enabled = true;
        dialogueText.text = "";
        for (int i = 0; i < allDialogue[index].Dialogue.Length; i++)
        {
            bool playAudio = false;
            switch (char.ToLower(allDialogue[index].Dialogue[i]))
            {
                case 'a':
                    playAudio = true;
                    break;
                case 'e':
                    playAudio = true;
                    break;
                case 'i':
                    playAudio = true;
                    break;
                case 'o':
                    playAudio = true;
                    break;
                case 'u':
                    playAudio = true;
                    break;
            }

            if (playAudio)
            {
                audioSource.pitch = Random.Range(0.75f, 1.15f);
                audioSource.volume = Random.Range(0.5f, 1f);
                audioSource.PlayOneShot(audios[0]);
            }

            dialogueText.text += allDialogue[index].Dialogue[i];
            yield return new WaitForSeconds(1.0f/textSpeed);
        }

        yield return new WaitForSeconds(5);

        textBox.enabled = false;
        dialogueText.enabled = false;
    }
}
