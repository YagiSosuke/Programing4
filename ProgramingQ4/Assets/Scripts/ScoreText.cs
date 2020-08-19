using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*スコアを表示*/

public class ScoreText : MonoBehaviour
{
    public int turnNum;    //残りターン数を示す
    public int bsNum;  //黒の点数
    public int wsNum;  //白の点数

    [SerializeField] Text Turn;    //残り手数
    [SerializeField] Text BlackScore;    //黒の得点
    [SerializeField] Text WhiteScore;    //白の得点

    // Start is called before the first frame update
    void Start()
    {
        turnNum = -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //点数をカウントする
    public void CountScore()
    {
        turnNum = 0;
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
                else if(ReversScript.map[j, i] == 0)
                {
                    turnNum++;
                }
            }
        }

        Turn.text = turnNum.ToString();
        BlackScore.text = bsNum.ToString();
        WhiteScore.text = wsNum.ToString();
    }
}
