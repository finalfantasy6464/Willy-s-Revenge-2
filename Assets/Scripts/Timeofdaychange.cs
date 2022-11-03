using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeofdaychange : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public bool lerp1 = true;
    public bool lerp2;
    public bool lerp3;
    public bool lerp4;

    public float t = 0;

    void Update()
    {
        t += Time.deltaTime / 10;

        if (lerp1)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, new Color(246, 170, 0, 255), t);
            if(t >= 1.2f)
            {
                lerp2 = true;
                lerp1 = false;
                t = 0;
            }
        }

        if(lerp2)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, new Color(0, 64, 104, 255), t);
            if(t >= 1.2f)
            {
                lerp3 = true;
                lerp2 = false;
                t = 0;
            }
        }
        if (lerp3)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, new Color(249, 255, 143, 255), t);
            if (t >= 1.2f)
            {
                lerp4 = true;
                lerp3 = false;
                t = 0;
            }
        }
        if (lerp4)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, new Color(255, 255, 255, 255), t);
            if (t >= 1.2f)
            {
                lerp1 = true;
                lerp4 = false;
                t = 0;
            }
        }
    }
}
