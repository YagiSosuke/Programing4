using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*最初に表示されるパネルのスクリプト*/

public class FirstPanelScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //人と戦うボタン
    public void VSHumanButton()
    {
        AISYSTEM.VSAI_F = false;    //AIとは戦わない
        gameObject.SetActive(false);
    }

    //AIと戦うボタン
    public void VSAIButton()
    {
        AISYSTEM.VSAI_F = true;    //AIと戦う
        gameObject.SetActive(false);
    }
}
