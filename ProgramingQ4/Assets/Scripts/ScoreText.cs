using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*スコアを表示*/

public class ScoreText : MonoBehaviour
{
    int bsNum;  //黒の点数
    int wsNum;  //白の点数

    [SerializeField] Text BlackScore;    //黒の得点
    [SerializeField] Text WhiteScore;    //白の得点

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //点数をカウントする
    public void CountScore()
    {
        bsNum = 0;  //点数を0に設定
        wsNum = 0;  //点数を0に設定

        //マス目探索
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                //そのマスが黒ならば
                if(ReversScript.map[j, i] == 1 || ReversScript.map[j, i] == 3)
                {
                    bsNum++;
                }
                //そのマスが白ならば
                else if(ReversScript.map[j, i] == 2 || ReversScript.map[j, i] == 4)
                {
                    wsNum++;
                }
            }
        }

        BlackScore.text = bsNum.ToString();
        WhiteScore.text = wsNum.ToString();
    }
}
