using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*両面同じ駒を扱うスクリプト*/

public class EqualPiece : MonoBehaviour
{
    public bool EqualF;    //両面が同じかどうか見る
    [SerializeField] ReversScript revercescript;

    Button EqualButton;

    // Start is called before the first frame update
    void Start()
    {
        EqualF = false;
        EqualButton = GetComponent<Button>();;
    }

    // Update is called once per frame
    void Update()
    {
        if (ReversScript.WhoTurn == "Black")
        {
            if (!revercescript.MyEqualPieceF)
            {
                EqualButton.interactable = true;
                if (EqualF)
                {
                    GetComponent<Image>().color = new Color(0.2f, 1f, 1f);
                }
                else
                {
                    GetComponent<Image>().color = new Color(1, 1, 1);
                }
            }
            else
            {
                EqualButton.interactable = false;
            }
        }
        else if(ReversScript.WhoTurn == "White")
        {
            if (!revercescript.EnemyEqualPieceF)
            {
                EqualButton.interactable = true;
                if (EqualF)
                {
                    GetComponent<Image>().color = new Color(0.2f, 1f, 1f);
                }
                else
                {
                    GetComponent<Image>().color = new Color(1, 1, 1);
                }
            }
            else
            {
                EqualButton.interactable = false;
            }
        }
    }

    //ボタンが押されたら
    public void ButtonPush()
    {
        EqualF = !EqualF;
        if(ReversScript.WhoTurn == "Black")
        {
        }
        else if(ReversScript.WhoTurn == "White")
        {
        }
    }
}
