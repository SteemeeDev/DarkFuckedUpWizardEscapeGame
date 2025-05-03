using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] float textSpeed = 20;
    [SerializeField] GameObject[] textBoxElements;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] AudioClip[] audios;

    AudioSource audioSource;
    RectTransform _rectTransform;

    public Coroutine dialogueWriter;
    Coroutine boxMover;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        audioSource = GetComponent<AudioSource>();  
    }

    bool userSkipped = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            userSkipped = true;
        }
    }

    public IEnumerator ReadDialogue(string dialogueObj, float lastingTime, float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (GameObject element in textBoxElements)
        {
            element.SetActive(true);
        }

        boxMover = StartCoroutine(MoveBox(3.0f, 0));

        dialogueText.enabled = true;
        dialogueText.text = "";

        userSkipped = false;

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

            dialogueText.text += dialogueObj[i];

            if (!userSkipped)
            {
                if (playAudio)
                {
                    audioSource.pitch = UnityEngine.Random.Range(0.75f, 1.15f);
                    audioSource.volume = 0;// Random.Range(0.5f, 1f);
                    audioSource.PlayOneShot(audios[0]);
                }
                yield return new WaitForSeconds(1.0f / textSpeed);
            }

        }


        userSkipped = false;
        float elapsed = 0;

        while (true)
        {
            elapsed += Time.deltaTime;

            // You can set lastingTime to -1 to have dialogue appear indefinetely
            if (lastingTime > 0 && elapsed > lastingTime) break;
            if (userSkipped) break;


            yield return null;
        }

        yield return boxMover = StartCoroutine(MoveBox(1.0f, -350.0f));

        foreach (GameObject element in textBoxElements)
        {
            element.SetActive(false);
        }
        dialogueText.enabled = false;
    }

    IEnumerator MoveBox(float moveTime, float targetY)
    {
        float startY = transform.position.y;
        float elapsed = 0;
        while (elapsed < moveTime)
        {
            float t = elapsed / moveTime;
            elapsed += Time.deltaTime;

            transform.position = new Vector3(transform.position.x, Mathf.Lerp(startY, targetY, 1 - Mathf.Pow(1 - t, 5)), 0);

            yield return null;
        }
    }
}
