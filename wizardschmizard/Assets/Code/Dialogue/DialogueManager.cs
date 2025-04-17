using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] float textSpeed = 20;

    [SerializeField] GameObject[] textBoxElements;

    [SerializeField] TMP_Text dialogueText;

    AudioSource audioSource;
    [SerializeField] AudioClip[] audios;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();  
    }


    public IEnumerator ReadDialogue(string dialogueObj)
    {
        foreach (GameObject element in textBoxElements)
        {
            element.SetActive(true);
        }

        dialogueText.enabled = true;
        dialogueText.text = "";
        for (int i = 0; i < dialogueObj.Length; i++)
        {
            bool playAudio = false;
            switch (char.ToLower(dialogueObj[i]))
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
                case 'y':
                    playAudio = true;
                    break;
            }

            if (playAudio)
            {
                audioSource.pitch = Random.Range(0.75f, 1.15f);
                audioSource.volume = Random.Range(0.5f, 1f);
                audioSource.PlayOneShot(audios[0]);
            }

            dialogueText.text += dialogueObj[i];
            yield return new WaitForSeconds(1.0f/textSpeed);
        }

        yield return new WaitForSeconds(5);

        foreach (GameObject element in textBoxElements)
        {
            element.SetActive(true);
        }
        dialogueText.enabled = false;
    }
}
