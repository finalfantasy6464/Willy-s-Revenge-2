using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GatePromptButton : MonoBehaviour
{
    public float pressedTime;
    public float releasedTime;
    public float counter;
    public TextMeshProUGUI keyLabel;
    public Image key;
    public Sprite[] gamepadSprites;
    public Sprite[] keySprites;
    public bool isPressed;

    IEnumerator promptAnimation;
    
    void OnEnable()
    {
        promptAnimation = PromptAnimationRoutine();
        StartCoroutine(promptAnimation);
    }

    void OnDisable()
    {
        if(promptAnimation != null)
        {
            StopCoroutine(promptAnimation);
            promptAnimation = null;
        }
    }

    IEnumerator PromptAnimationRoutine()
    {
        while(true)
        {
            counter += Time.deltaTime;
            if(isPressed)
            {
                if(counter >= pressedTime)
                {
                    isPressed = false;
                    counter = 0f;
                    key.sprite = keySprites[0];
                }
            }
            else
            {
                if(counter >= releasedTime)
                {
                    isPressed = true;
                    counter = 0f;
                    key.sprite = keySprites[1];
                }
            }
            yield return null;
        }
    }
}
