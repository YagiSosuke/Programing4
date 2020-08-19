using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

/*終了が面でボタンを押した時の動作*/

public class LastButtonScript : MonoBehaviour
{
    [SerializeField] GameObject FirstPanel;
    [SerializeField] GameObject EndPanel;
    [SerializeField] ReversScript revers;
    [SerializeField] ScoreText scoreScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void OnceButton()
    {
        EndPanel.SetActive(false);
        scoreScript.turnNum = -1;


        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                //マスゲームオブジェクト取得
                switch (i)
                {
                    case 0:
                        revers.MapObject[j, i] = GameObject.Find("A").transform.GetChild(j).gameObject;
                        break;
                    case 1:
                        revers.MapObject[j, i] = GameObject.Find("B").transform.GetChild(j).gameObject;
                        break;
                    case 2:
                        revers.MapObject[j, i] = GameObject.Find("C").transform.GetChild(j).gameObject;
                        break;
                    case 3:
                        revers.MapObject[j, i] = GameObject.Find("D").transform.GetChild(j).gameObject;
                        break;
                    case 4:
                        revers.MapObject[j, i] = GameObject.Find("E").transform.GetChild(j).gameObject;
                        break;
                    case 5:
                        revers.MapObject[j, i] = GameObject.Find("F").transform.GetChild(j).gameObject;
                        break;
                    case 6:
                        revers.MapObject[j, i] = GameObject.Find("G").transform.GetChild(j).gameObject;
                        break;
                    case 7:
                        revers.MapObject[j, i] = GameObject.Find("H").transform.GetChild(j).gameObject;
                        break;
                }
                ReversScript.map[j, i] = 0;      //状態を0に       //スクリプト取得
                revers.MapObject[j, i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        //初期位置を配置
        //初期位置配置
        ReversScript.map[3, 3] = ReversScript.map[4, 4] = 2;
        ReversScript.map[3, 4] = ReversScript.map[4, 3] = 1;

        revers.MapObject[3, 3].transform.GetChild(0).gameObject.SetActive(true);
        revers.MapObject[3, 3].transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.white;
        revers.MapObject[4, 4].transform.GetChild(0).gameObject.SetActive(true);
        revers.MapObject[4, 4].transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.white;
        revers.MapObject[3, 4].transform.GetChild(0).gameObject.SetActive(true);
        revers.MapObject[3, 4].transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.black;
        revers.MapObject[4, 3].transform.GetChild(0).gameObject.SetActive(true);
        revers.MapObject[4, 3].transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.black;

        ReversScript.WhoTurn = "Black";      //最初は黒のターン
        ReversScript.MyColor = 1;            //最初の自分の色は黒
        ReversScript.MyColor2 = 3;            //最初の自分の色は黒
        ReversScript.MyColorC = Color.black;

        ReversScript.EnemyColor = 2;         //最初の敵の色は白
        ReversScript.EnemyColor2 = 4;         //最初の敵の色は白
        ReversScript.EnemyColorC = Color.white;

        revers.PassF = false;      //基本、パスはできない

        revers.MyEqualPieceF = false;
        revers.EnemyEqualPieceF = false;

        FirstPanel.SetActive(true);

    }


    public void EndButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
    }
}
