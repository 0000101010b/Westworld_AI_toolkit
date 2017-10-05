using UnityEngine;
using System.Collections;

public class scaleSpeech : MonoBehaviour { 

    public int len;

    public float w;
    public float h;
    public TextMesh text;
    // Use this for initialization
    void Start()
    {
        FixSpeechBubble();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            FixSpeechBubble();
    }

    public void FixSpeechBubble()
    {
        string str = text.text;

        Vector3 s = transform.localScale;

        if (str.Length < len)
        {
            s.x = w * str.Length;
            s.y = h * 5;

        }
        else
        {
            s.x = w * len;
            s.y = h * ((str.Length % len) + 5);

            char[] c = new char[500];

            int index = 0;
            for (int i = 0; i < str.Length; i++)
            {
                c[i + index] = str[i];

                if (i < str.Length - 1)
                {
                    if (i % len == 0 && i != 0 && str[i + 1] != '\n')
                    {
                        index += 1;
                        c[i + index] = '\n';
                    }
                }
            }

            string t = new string(c);
            t = t.Substring(0, str.Length + index);
            text.text = t;
        }
        transform.localScale = s;
    }
}
