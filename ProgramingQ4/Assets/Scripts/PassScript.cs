using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*パスするスクリプト*/

public class PassScript : MonoBehaviour
{
    [SerializeField] GameObject PassObject;  //パスするゲームオブジェクト
    Button PassButton;      //パスするボタン

    [SerializeField] ReversScript reversscript;        //総合的なのを含むゲームオブジェクト

    // Start is called before the first frame update
    void Start()
    {
        PassButton = PassObject.GetComponent<Button>();
        PassObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (reversscript.PassF)
        {
            PassObject.SetActive(true);
        }
        else
        {
            PassObject.SetActive(false);
        }
    }

    //ボタンを押したら(パスする)
    public void ButtonPush()
    {
        ReversScript.TurnEndF = true;
    }
}
