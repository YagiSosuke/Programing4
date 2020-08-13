using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*両面同じ駒を扱うスクリプト*/

public class EqualPiece : MonoBehaviour
{
    public bool EqualF;    //両面が同じかどうか見る

    // Start is called before the first frame update
    void Start()
    {
        EqualF = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (EqualF)
        {
            GetComponent<Image>().color = new Color(0.2f,0.2f,0.2f);
        }
        else
        {
            GetComponent<Image>().color = new Color(1,1,1);
        }
    }

    //ボタンが押されたら
    public void ButtonPush()
    {
        EqualF = !EqualF;
    }
}
