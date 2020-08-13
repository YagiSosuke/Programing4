using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*誰のターンか表示するスクリプト*/

public class WhoTurnText : MonoBehaviour
{
    [SerializeField] Text WordText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ReversScript.WhoTurn == "Black")
        {
            WordText.text = "黒のターンです";
        }
        else 
        {
            WordText.text = "白のターンです";
        }
    }
}
