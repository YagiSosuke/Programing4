using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*オセロのシステムのスクリプト*/

public class ReversScript : MonoBehaviour
{
    //オセロのマス目
    //0...空白
    //1...黒
    //2...白
    //3...両面黒
    //4...両面白
    public static int[,] map = new int[8,8];
    public static bool TurnEndF = false;        //ターンの終了を判定するフラグ

    public GameObject[,] MapObject = new GameObject[8,8];      //マップのゲームオブジェクト
    public static StageScript[,] MapScript = new StageScript[8,8];      //マップのスクリプト

    public static string WhoTurn;     //誰のターンか示す変数

    public static int MyColor;      //自分の色
    public static int MyColor2;      //自分の色(両面同じ)
    public static Color MyColorC;   //自分の色(Color型)
    public static int EnemyColor;   //相手の色
    public static int EnemyColor2;   //相手の色(両面同じ)
    public static Color EnemyColorC;//相手の色(Color型)
    
    [SerializeField] ScoreText ScoreScript;      //スコアを格納してあるスクリプト

    public bool PassF;     //パスできるかどうかのフラグ

    [SerializeField] EqualPiece equalpiece;         //両面同じ駒を格納してあるスクリプト

    public bool MyEqualPieceF;      //自分が両面同じピースを使ったか？
    public bool EnemyEqualPieceF;   //相手が両面同じピースを使ったか？

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                //マスゲームオブジェクト取得
                switch (i)
                {
                    case 0:
                        MapObject[j,i] = GameObject.Find("A").transform.GetChild(j).gameObject;
                        break;
                    case 1:
                        MapObject[j, i] = GameObject.Find("B").transform.GetChild(j).gameObject;
                        break;
                    case 2:
                        MapObject[j, i] = GameObject.Find("C").transform.GetChild(j).gameObject;
                        break;
                    case 3:
                        MapObject[j, i] = GameObject.Find("D").transform.GetChild(j).gameObject;
                        break;
                    case 4:
                        MapObject[j, i] = GameObject.Find("E").transform.GetChild(j).gameObject;
                        break;
                    case 5:
                        MapObject[j, i] = GameObject.Find("F").transform.GetChild(j).gameObject;
                        break;
                    case 6:
                        MapObject[j, i] = GameObject.Find("G").transform.GetChild(j).gameObject;
                        break;
                    case 7:
                        MapObject[j, i] = GameObject.Find("H").transform.GetChild(j).gameObject;
                        break;
                }
                map[j, i] = 0;      //状態を0に
                MapScript[j, i] = MapObject[j, i].GetComponent<StageScript>();        //スクリプト取得
            }
        }

        WhoTurn = "Black";      //最初は黒のターン
        MyColor = 1;            //最初の自分の色は黒
        MyColor2 = 3;            //最初の自分の色は黒
        MyColorC = Color.black;

        EnemyColor = 2;         //最初の敵の色は白
        EnemyColor2 = 4;         //最初の敵の色は白
        EnemyColorC = Color.white;

        PassF = false;      //基本、パスはできない

        MyEqualPieceF = false;
        EnemyEqualPieceF = false;


    Debug.Log("表示します\n"+
            ReversScript.map[0, 0].ToString() + ReversScript.map[1, 0].ToString() + ReversScript.map[2, 0].ToString() + ReversScript.map[3, 0].ToString() + ReversScript.map[4, 0].ToString() + ReversScript.map[5, 0].ToString() + ReversScript.map[6, 0].ToString() + ReversScript.map[7, 0].ToString() + "\n" +
            ReversScript.map[0, 1] + ReversScript.map[1, 1] + ReversScript.map[2, 1] + ReversScript.map[3, 1] + ReversScript.map[4, 1] + ReversScript.map[5, 1] + ReversScript.map[6, 1] + ReversScript.map[7, 1] + "\n" +
            ReversScript.map[0, 2] + ReversScript.map[1, 2] + ReversScript.map[2, 2] + ReversScript.map[3, 2] + ReversScript.map[4, 2] + ReversScript.map[5, 2] + ReversScript.map[6, 2] + ReversScript.map[7, 2] + "\n" +
            ReversScript.map[0, 3] + ReversScript.map[1, 3] + ReversScript.map[2, 3] + ReversScript.map[3, 3] + ReversScript.map[4, 3] + ReversScript.map[5, 3] + ReversScript.map[6, 3] + ReversScript.map[7, 3] + "\n" +
            ReversScript.map[0, 4] + ReversScript.map[1, 4] + ReversScript.map[2, 4] + ReversScript.map[3, 4] + ReversScript.map[4, 4] + ReversScript.map[5, 4] + ReversScript.map[6, 4] + ReversScript.map[7, 4] + "\n" +
            ReversScript.map[0, 5] + ReversScript.map[1, 5] + ReversScript.map[2, 5] + ReversScript.map[3, 5] + ReversScript.map[4, 5] + ReversScript.map[5, 5] + ReversScript.map[6, 5] + ReversScript.map[7, 5] + "\n" +
            ReversScript.map[0, 6] + ReversScript.map[1, 6] + ReversScript.map[2, 6] + ReversScript.map[3, 6] + ReversScript.map[4, 6] + ReversScript.map[5, 6] + ReversScript.map[6, 6] + ReversScript.map[7, 6] + "\n" +
            ReversScript.map[0, 7] + ReversScript.map[1, 7] + ReversScript.map[2, 7] + ReversScript.map[3, 7] + ReversScript.map[4, 7] + ReversScript.map[5, 7] + ReversScript.map[6, 7] + ReversScript.map[7, 7]);
    }

    // Update is called once per frame
    void Update()
    {
        PassFlag();         //パスできるか検索する

        //ターン終了時
        if (TurnEndF)
        {
            //マス目が置けるかどうかの情報をクリアにする
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    MapScript[i, j].CheckF = false;
                }
            }
            //攻めの交代をする
            if (WhoTurn == "Black")
            {
                MyColor = 2;
                MyColor2 = 4;
                MyColorC = Color.white;
                EnemyColor = 1;
                EnemyColor2 = 3;
                EnemyColorC = Color.black;
                WhoTurn = "White";
            }
            else if (WhoTurn == "White")
            {
                MyColor = 1;
                MyColor2 = 3;
                MyColorC = Color.black;
                EnemyColor = 2;
                EnemyColor2 = 4;
                EnemyColorC = Color.white;
                WhoTurn = "Black";
            }
            //マスを更新、ひっくり返す
            UpdatePiece();

            //スコアの更新をする
            ScoreScript.CountScore();

            PassF = false;      //パスできない状況に戻す
            equalpiece.EqualF = false;      //両面同じでない状況に戻す

            TurnEndF = false;

        }
    }


    //マスをひっくり返す
    public static void TurnOver(int x, int y)
    {
        //上方向チェック
        bool CheckF = false;        //マス目が置けるかをチェック
        for (int i = y - 2; i >= 0; i--)
        {
            //隣が敵の時
            if (ReversScript.map[x, y - 1] == ReversScript.EnemyColor || ReversScript.map[x, y - 1] == ReversScript.EnemyColor2)
            {
                //置いていない時はだめ
                if (ReversScript.map[x, i] == 0)
                {
                    break;
                }
                //自の場合
                else if (ReversScript.map[x, i] == ReversScript.MyColor || ReversScript.map[x, i] == ReversScript.MyColor2)
                {
                    CheckF = true;      //ここにはコマが置けるのでtrue
                    break;
                }
                //敵の場合は進展なし
            }
            else {
                break;
            }
        }
        //ひっくり返す
        if (CheckF)
        {
            //隣がひっくり返せるならひっくり返す
            if (map[x, y - 1] == EnemyColor)
            {
                map[x, y - 1] = MyColor;
                MapScript[x, y - 1].PieceImage.color = MyColorC;
            }
            //自分の色が現れるまでひっくり返す
            for (int i = y - 2; i >= 0; i--)
            {
                //敵色だった場合
                if (map[x, i] == EnemyColor)
                {
                    map[x, i] = MyColor;
                    MapScript[x, i].PieceImage.color = MyColorC;
                }
                //ひっくり返せない敵色だった場合
                else if (map[x, i] == EnemyColor2)
                {
                }
                //自分の色だった場合
                else
                {
                    break;
                }
            }
        }
        
        //下方向チェック
        CheckF = false;        //マス目が置けるかをチェック
        for (int i = y + 2; i < 8; i++)
        {
            //隣が敵の時
            if (ReversScript.map[x, y + 1] == ReversScript.EnemyColor || ReversScript.map[x, y + 1] == ReversScript.EnemyColor2)
            {
                //置いていない時はだめ
                if (ReversScript.map[x, i] == 0)
                {
                    break;
                }
                //自の場合
                else if (ReversScript.map[x, i] == ReversScript.MyColor || ReversScript.map[x, i] == ReversScript.MyColor2)
                {
                    CheckF = true;      //ここにはコマが置けるのでtrue
                    break;
                }
                //敵の場合は進展なし
            }
            else {
                break;
            }
        }
        //ひっくり返す
        if (CheckF)
        {
            //隣をひっくり返す
            if (map[x, y + 1] == EnemyColor)
            {
                map[x, y + 1] = MyColor;
                MapScript[x, y + 1].PieceImage.color = MyColorC;
            }
            //自分の色が現れるまでひっくり返す
            for (int i = y + 2; i < 8; i++)
            {
                //敵色だった場合
                if (map[x, i] == EnemyColor)
                {
                    map[x, i] = MyColor;
                    MapScript[x, i].PieceImage.color = MyColorC;
                }
                else if (map[x, i] == EnemyColor2)
                {

                }
                //自分の色だった場合
                else
                {
                    break;
                }
            }
        }

        //右方向チェック
        CheckF = false;        //マス目が置けるかをチェック
        for (int i = x + 2; i < 8; i++)
        {
            //隣が敵の時
            if (ReversScript.map[x + 1, y] == ReversScript.EnemyColor || ReversScript.map[x + 1, y] == ReversScript.EnemyColor2)
            {
                //置いていない時はだめ
                if (ReversScript.map[i, y] == 0)
                {
                    break;
                }
                //自の場合
                else if (ReversScript.map[i, y] == ReversScript.MyColor || ReversScript.map[i, y] == ReversScript.MyColor2)
                {
                    CheckF = true;      //ここにはコマが置けるのでtrue
                    break;
                }
                //敵の場合は進展なし
            }
            else {
                break;
            }
        }
        //ひっくり返す
        if (CheckF)
        {
            //隣をひっくり返す
            if (map[x + 1, y] == EnemyColor)
            {
                map[x + 1, y] = MyColor;
                MapScript[x + 1, y].PieceImage.color = MyColorC;
            }
            //自分の色が現れるまでひっくり返す
            for (int i = x + 2; i < 8; i++)
            {
                //敵色だった場合
                if (map[i, y] == EnemyColor)
                {
                    map[i, y] = MyColor;
                    MapScript[i, y].PieceImage.color = MyColorC;
                }
                if (map[i, y] == EnemyColor2)
                {
                }
                //自分の色だった場合
                else
                {
                    break;
                }
            }
        }

        //左方向チェック
        CheckF = false;        //マス目が置けるかをチェック
        for (int i = x - 2; i >= 0; i--)
        {
            //隣が敵の時
            if (ReversScript.map[x - 1, y] == ReversScript.EnemyColor || ReversScript.map[x - 1, y] == ReversScript.EnemyColor2)
            {
                //置いていない時はだめ
                if (ReversScript.map[i, y] == 0)
                {
                    break;
                }
                //自の場合
                else if (ReversScript.map[i, y] == ReversScript.MyColor || ReversScript.map[i, y] == ReversScript.MyColor2)
                {
                    CheckF = true;      //ここにはコマが置けるのでtrue
                    break;
                }
                //敵の場合は進展なし
            }
            else {
                break;
            }
        }
        //ひっくり返す
        if (CheckF)
        {
            //隣をひっくり返す
            if (map[x - 1, y] == EnemyColor)
            {
                map[x - 1, y] = MyColor;
                MapScript[x - 1, y].PieceImage.color = MyColorC;
            }
            //自分の色が現れるまでひっくり返す
            for (int i = x - 2; i >= 0; i--)
            {
                //敵色だった場合
                if (map[i, y] == EnemyColor)
                {
                    map[i, y] = MyColor;
                    MapScript[i, y].PieceImage.color = MyColorC;
                }
                else if (map[i, y] == EnemyColor2)
                {

                }
                //自分の色だった場合
                else
                {
                    break;
                }
            }
        }
        
        //ナナメも追加する
        //右上チェック
        CheckF = false;        //マス目が置けるかをチェック
        for (int i = 2; i < 8; i++)
        {
            //端まで検索したら
            if (x + i >= 8 || y - i <= 0) break;
            //隣が敵の時
            if (ReversScript.map[x + 1, y - 1] == ReversScript.EnemyColor || ReversScript.map[x + 1, y - 1] == ReversScript.EnemyColor2)
            {
                //置いていない時はだめ
                if (ReversScript.map[x + i, y - i] == 0)
                {
                    break;
                }
                //自の場合
                else if (ReversScript.map[x + i, y - i] == ReversScript.MyColor || ReversScript.map[x + i, y - i] == ReversScript.MyColor2)
                {
                    CheckF = true;      //ここにはコマが置けるのでtrue
                    break;
                }
                //敵の場合は進展なし
            }
            else {
                break;
            }
        }
        //ひっくり返す
        if (CheckF)
        {
            //隣をひっくり返す
            if (map[x + 1, y - 1] == EnemyColor)
            {
                map[x + 1, y - 1] = MyColor;
                MapScript[x + 1, y - 1].PieceImage.color = MyColorC;
            }
            //自分の色が現れるまでひっくり返す
            for (int i =  2; i < 8; i++)
            {
                //敵色だった場合
                if (map[x + i, y - i] == EnemyColor)
                {
                    map[x + i, y - i] = MyColor;
                    MapScript[x + i, y - i].PieceImage.color = MyColorC;
                }
                else if (map[x + i, y - i] == EnemyColor2)
                {

                }
                //自分の色だった場合
                else
                {
                    break;
                }
            }
        }
        
        //右下チェック
        CheckF = false;        //マス目が置けるかをチェック
        for (int i = 2; i < 8; i++)
        {
            //端まで検索したら
            if (x + i >= 8 || y + i >= 8) break;
            //隣が敵の時
            if (ReversScript.map[x + 1, y + 1] == ReversScript.EnemyColor || ReversScript.map[x + 1, y + 1] == ReversScript.EnemyColor2)
            {
                //置いていない時はだめ
                if (ReversScript.map[x + i, y + i] == 0)
                {
                    break;
                }
                //自の場合
                else if (ReversScript.map[x + i, y + i] == ReversScript.MyColor || ReversScript.map[x + i, y + i] == ReversScript.MyColor2)
                {
                    CheckF = true;      //ここにはコマが置けるのでtrue
                    break;
                }
                //敵の場合は進展なし
            }
            else {
                break;
            }
        }
        //ひっくり返す
        if (CheckF)
        {
            //隣をひっくり返す
            if (map[x + 1, y + 1] == EnemyColor)
            {
                map[x + 1, y + 1] = MyColor;
                MapScript[x + 1, y + 1].PieceImage.color = MyColorC;
            }
            //自分の色が現れるまでひっくり返す
            for (int i = 2; i < 8; i++)
            {
                //敵色だった場合
                if (map[x + i, y + i] == EnemyColor)
                {
                    map[x + i, y + i] = MyColor;
                    MapScript[x + i, y + i].PieceImage.color = MyColorC;
                }
                else if (map[x + i, y + i] == EnemyColor2)
                {

                }
                //自分の色だった場合
                else
                {
                    break;
                }
            }
        }

        //左下チェック
        CheckF = false;        //マス目が置けるかをチェック
        for (int i = 2; i < 8; i++)
        {
            //端まで検索したら
            if (x - i < 0 || y + i >= 8) break;
            //隣が敵の時
            if (ReversScript.map[x - 1, y + 1] == ReversScript.EnemyColor || ReversScript.map[x - 1, y + 1] == ReversScript.EnemyColor2)
            {
                //置いていない時はだめ
                if (ReversScript.map[x - i, y + i] == 0)
                {
                    break;
                }
                //自の場合
                else if (ReversScript.map[x - i,y + i] == ReversScript.MyColor || ReversScript.map[x - i, y + i] == ReversScript.MyColor2)
                {
                    Debug.Log("斜めに自分がある("+(x- i) +","+(y+ i) +")"); 
                    CheckF = true;      //ここにはコマが置けるのでtrue
                    break;
                }
                //敵の場合は進展なし
            }
            else {
                break;
            }
        }
        //ひっくり返す
        if (CheckF)
        {
            //隣をひっくり返す
            if (map[x - 1, y + 1] == EnemyColor)
            {
                map[x - 1, y + 1] = MyColor;
                MapScript[x - 1, y + 1].PieceImage.color = MyColorC;
            }
            //自分の色が現れるまでひっくり返す
            for (int i = 2; i < 8; i++)
            {
                //敵色だった場合
                if (map[x - i, y + i] == EnemyColor)
                {
                    map[x - i, y + i] = MyColor;
                    MapScript[x - i, y + i].PieceImage.color = MyColorC;
                }
                else if (map[x - i, y + i] == EnemyColor2)
                {

                }
                //自分の色だった場合
                else
                {
                    break;
                }
            }
        }

        //左上チェック
        CheckF = false;        //マス目が置けるかをチェック
        for (int i = 2; i < 8; i++)
        {
            //端まで検索したら
            if ( x - i <= 0 || y - i <= 0) break;
            //隣が敵の時
            if (ReversScript.map[x - 1, y - 1] == ReversScript.EnemyColor || ReversScript.map[x - 1, y - 1] == ReversScript.EnemyColor2)
            {
                //置いていない時はだめ
                if (ReversScript.map[x - i, y - i] == 0)
                {
                    break;
                }
                //自の場合
                else if (ReversScript.map[x - i, y - i] == ReversScript.MyColor || ReversScript.map[x - i, y - i] == ReversScript.MyColor2)
                {
                    CheckF = true;      //ここにはコマが置けるのでtrue
                    break;
                }
                //敵の場合は進展なし
            }
            else {
                break;
            }
        }
        //ひっくり返す
        if (CheckF)
        {
            //隣をひっくり返す
            if (map[x - 1, y - 1] == EnemyColor)
            {
                map[x - 1, y - 1] = MyColor;
                MapScript[x - 1, y - 1].PieceImage.color = MyColorC;
            }
            //自分の色が現れるまでひっくり返す
            for (int i = 2; i < 8; i++)
            {
                //敵色だった場合
                if (map[x - i, y - i] == EnemyColor)
                {
                    map[x - i, y - i] = MyColor;
                    MapScript[x - i, y - i].PieceImage.color = MyColorC;
                }
                else if (map[x - i, y - i] == EnemyColor2)
                {

                }
                //自分の色だった場合
                else
                {
                    break;
                }
            }

        }


        /*
        Debug.Log(ReversScript.map[0, 0] + ReversScript.map[1, 0] + ReversScript.map[2, 0] + ReversScript.map[3, 0] + ReversScript.map[4, 0] + ReversScript.map[5, 0] + ReversScript.map[6, 0] + ReversScript.map[7, 0] + "\n" +
            ReversScript.map[0, 1] + ReversScript.map[1, 1] + ReversScript.map[2, 1] + ReversScript.map[3, 1] + ReversScript.map[4, 1] + ReversScript.map[5, 1] + ReversScript.map[6, 1] + ReversScript.map[7, 1] + "\n" +
            ReversScript.map[0, 2] + ReversScript.map[1, 2] + ReversScript.map[2, 2] + ReversScript.map[3, 2] + ReversScript.map[4, 2] + ReversScript.map[5, 2] + ReversScript.map[6, 2] + ReversScript.map[7, 2] + "\n" +
            ReversScript.map[0, 3] + ReversScript.map[1, 3] + ReversScript.map[2, 3] + ReversScript.map[3, 3] + ReversScript.map[4, 3] + ReversScript.map[5, 3] + ReversScript.map[6, 3] + ReversScript.map[7, 3] + "\n" +
            ReversScript.map[0, 4] + ReversScript.map[1, 4] + ReversScript.map[2, 4] + ReversScript.map[3, 4] + ReversScript.map[4, 4] + ReversScript.map[5, 4] + ReversScript.map[6, 4] + ReversScript.map[7, 4] + "\n" +
            ReversScript.map[0, 5] + ReversScript.map[1, 5] + ReversScript.map[2, 5] + ReversScript.map[3, 5] + ReversScript.map[4, 5] + ReversScript.map[5, 5] + ReversScript.map[6, 5] + ReversScript.map[7, 5] + "\n" +
            ReversScript.map[0, 6] + ReversScript.map[1, 6] + ReversScript.map[2, 6] + ReversScript.map[3, 6] + ReversScript.map[4, 6] + ReversScript.map[5, 6] + ReversScript.map[6, 6] + ReversScript.map[7, 6] + "\n" +
            ReversScript.map[0, 7] + ReversScript.map[1, 7] + ReversScript.map[2, 7] + ReversScript.map[3, 7] + ReversScript.map[4, 7] + ReversScript.map[5, 7] + ReversScript.map[6, 7] + ReversScript.map[7, 7]);
        */
    }

    //パスになってしまうかどうか検索
    void PassFlag()
    {
        PassF = true;
        for(int i = 0; i < 8; i++)
        {
            if (PassF) {
                for (int j = 0; j < 8; j++)
                {
                    if (MapScript[j, i].CheckF && map[j, i] == 0)
                    {
                        PassF = false;
                        break;
                    }
                }
            }
        }
    }


    //マス目のアップデート
    //ひっくり返した後に行う
    void UpdatePiece()
    {
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                switch(map[i,j])
                {
                    //マス目を黒にする
                    case 1:
                        MapScript[i, j].PieceImage.color = Color.black;
                        break;
                    //マス目を白にする
                    case 2:
                        MapScript[i, j].PieceImage.color = Color.white;
                        break;
                }
            }
        }
    }
}
