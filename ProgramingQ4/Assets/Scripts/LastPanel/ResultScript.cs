using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*対戦結果を表示*/

public class ResultScript : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Text result;
    [SerializeField] Text score;

    [SerializeField] ScoreText scoreScript;

    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //ゲーム終了
        if (scoreScript.turnNum == 0)
        {
            panel.SetActive(true);
            score.text = "黒:" + scoreScript.bsNum + "  vs  白:" + scoreScript.wsNum;
            if (scoreScript.bsNum > scoreScript.wsNum)
            {
                result.text = ("黒の勝利です");
            }
            else if (scoreScript.bsNum < scoreScript.wsNum)
            {
                result.text = ("白の勝利です");
            }
            else
            {
                result.text = ("引き分けです");
            }
        }
    }
}
