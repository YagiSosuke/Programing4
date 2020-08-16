using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*AIのスクリプト*/
//パスも実装

public class AISYSTEM : MonoBehaviour
{
    public static bool VSAI_F = false;       //AIと戦うかどうかのスクリプト

    int[,] mapAI = new int[8,8];          //AIが手を読む用のマップ
    int[,] mapAITemp = new int[8, 8];          //AIが手を読む用のマップ

    bool[,] CheckF = new bool[8,8];            //ひっくり返せるかどうかのフラグ

    int[,] Scoremap = { { 30, -12, 0, -1, -1, 0, -12, 30 },
                        { -12, -15, -3, -3, -3, -3, -15, -12 },
                        { 0, -3, 0, -1, -1, 0, -3, 0 },
                        { -1, -3, -1, -1, -1, -1, -3, -1 },
                        { -1, -3, -1, -1, -1, -1, -3, -1 },
                        { 0, -3, 0, -1, -1, 0, -3, 0 },
                        { -12, -15, -3, -3, -3, -3, -15, -12 },
                        { 30, -12, 0, -1, -1, 0, -12, 30 } };          //点数付けされたマップ

    int HighScore_x;
    int HighScore_y;
    int HighScore_Num;

    int myScore;            //自分の総合点
    int enemyScore;         //相手の総合点

    public int Shindo;      //先を読む深度
    public bool XYChangeF = false;    //強い座標を更新するフラグ

    [SerializeField] ReversScript reversscript;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 8; i++){
            for(int j = 0; j < 8; j++)
            {
                CheckF[i, j] = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //AIがマップを読む
    void MapReading(int num)
    {
        //最初、お互いの点数は0
        myScore = 0;
        enemyScore = 0;

        //現在のマップ情報をコピー
        for(int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                mapAI[i, j] = mapAITemp[i, j] = ReversScript.map[i, j];
            }
        }


        //点数をつけて一番いいところを探索
        HighScore_Num = -999;

        //num回深くまで探索する
        //ここから、まちがっているかも
        //探索できないほどマスが少ない時の処理を加えていない
        SerchInner(num, myScore, mapAI);



        //とりあえずこれで良しとする。
        //実際にひっくり返す

        //置く
        reversscript.MapObject[HighScore_x,HighScore_y].GetComponent<StageScript>().PieceObject.SetActive(true);            //コマを配置する
        ReversScript.map[HighScore_x, HighScore_y] = 2;     //両面別の駒を配置

        //ひっくり返す
        ReversScript.TurnOver(HighScore_x, HighScore_y);
    }


    //駒が置けるかチェック
     public void CheckMap(int[,] AImap, int x, int y, int my, int my2, int ene, int ene2)
    {
        //マスが空の時のみ見る
        if (AImap[x, y] == 0)
        {
            //上方向チェック
            for (int i = y - 2; i >= 0; i--)
            {
                //隣が白の時
                if (AImap[x, y - 1] == ene || AImap[x, y - 1] == ene2)
                {
                    //置いていない時はだめ
                    if (AImap[x, i] == 0)
                    {
                        break;
                    }
                    //黒の場合
                    else if (AImap[x, i] == my || AImap[x, i] == my2)
                    {
                        CheckF[x,y] = true;      //ここにはコマが置けるのでtrue
                        break;
                    }
                    //白の場合は進展なし
                    else
                    {
                    }
                }
                else
                {
                    break;
                }
            }
            //下方向
            for (int i = y + 2; i < 8; i++)
            {
                //置かれる判定がされていない場合
                if (!CheckF[x,y])
                {
                    //隣が白の時
                    if (AImap[x, y + 1] == ene || AImap[x, y + 1] == ene2)
                    {
                        //置いていない時はだめ
                        if (AImap[x, i] == 0)
                        {
                            break;
                        }
                        //黒の場合
                        else if (AImap[x, i] == my || AImap[x, i] == my2)
                        {
                            CheckF[x,y] = true;      //ここにはコマが置けるのでtrue
                            break;
                        }
                        //白の場合は進展なし
                    }
                    else {
                        break;
                    }
                }
            }
            //左方向
            for (int i = x - 2; i >= 0; i--)
            {
                //置かれる判定がされていない場合
                if (!CheckF[x,y])
                {
                    //隣が白の時
                    if (AImap[x - 1, y] == ene || AImap[x - 1, y] == ene2)
                    {
                        //置いていない時はだめ
                        if (AImap[i, y] == 0)
                        {
                            break;
                        }
                        //黒の場合
                        else if (AImap[i, y] == my || AImap[i, y] == my2)
                        {
                            CheckF[x,y] = true;      //ここにはコマが置けるのでtrue
                            break;
                        }
                        //白の場合は進展なし
                    }
                    else {
                        break;
                    }
                }
            }
            //右方向
            for (int i = x + 2; i < 8; i++)
            {
                if (AImap[x, y] == 0)
                {
                    //置かれる判定がされていない場合
                    if (!CheckF[x,y])
                    {
                        //隣が白の時
                        if (AImap[x + 1, y] == ene || AImap[x + 1, y] == ene2)
                        {
                            //置いていない時はだめ
                            if (AImap[i, y] == 0)
                            {
                                break;
                            }
                            //黒の場合
                            else if (AImap[i, y] == my || AImap[i, y] == my2)
                            {
                                CheckF[x,y] = true;      //ここにはコマが置けるのでtrue
                                break;
                            }
                            //白の場合は進展なし
                        }
                        else {
                            break;
                        }
                    }
                }
            }
            //ナナメも追加する
            //右上
            for (int i = 2; i < 8; i++)
            {
                if (AImap[x, y] == 0 && x+i < 8 && y-i >= 0)
                {
                    //置かれる判定がされていない場合
                    if (!CheckF[x,y])
                    {
                        //隣が白の時
                        if (AImap[x + 1, y - 1] == ene || AImap[x + 1, y - 1] == ene2)
                        {
                            //置いていない時はだめ
                            if (AImap[x + i, y - i] == 0)
                            {
                                break;
                            }
                            //黒の場合
                            else if (AImap[x + i, y - i] == my || AImap[x + i, y - i] == my2)
                            {
                                CheckF[x,y] = true;      //ここにはコマが置けるのでtrue
                                break;
                            }
                            //白の場合は進展なし
                        }
                        else {
                            break;
                        }
                    }
                }
            }
            //右下
            for (int i = 2; i < 8; i++)
            {
                if (AImap[x, y] == 0 && x + i < 8 && y + i < 8)
                {
                    //置かれる判定がされていない場合
                    if (!CheckF[x,y])
                    {
                        //隣が白の時
                        if (AImap[x + 1, y +1] == ene || AImap[x + 1, y + 1] == ene2)
                        {
                            //置いていない時はだめ
                            if (AImap[x + i, y + i] == 0)
                            {
                                break;
                            }
                            //黒の場合
                            else if (AImap[x + i, y + i] == my || AImap[x + i, y + i] == my2)
                            {
                                CheckF[x,y] = true;      //ここにはコマが置けるのでtrue
                                break;
                            }
                            //白の場合は進展なし
                        }
                        else {
                            break;
                        }
                    }
                }
            }
            //左下
            for (int i = 2; i < 8; i++)
            {
                if (AImap[x, y] == 0 && x - i >= 0 && y + i < 8)
                {
                    //置かれる判定がされていない場合
                    if (!CheckF[x,y])
                    {
                        //隣が白の時
                        if (AImap[x - 1, y + 1] == ene || AImap[x - 1, y + 1] == ene2)
                        {
                            //置いていない時はだめ
                            if (AImap[x - i, y + i] == 0)
                            {
                                break;
                            }
                            //黒の場合
                            else if (AImap[x - i, y + i] == my || AImap[x - i, y + i] == my2)
                            {
                                CheckF[x,y] = true;      //ここにはコマが置けるのでtrue
                                break;
                            }
                            //白の場合は進展なし
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            //左上
            for (int i = 2; i < 8; i++)
            {
                if (AImap[x, y] == 0 && x - i >= 0 && y - i >= 0)
                {
                    //置かれる判定がされていない場合
                    if (!CheckF[x,y])
                    {
                        //隣が白の時
                        if (AImap[x - 1, y - 1] == ene || AImap[x - 1, y - 1] == ene2)
                        {
                            //置いていない時はだめ
                            if (AImap[x - i, y - i] == 0)
                            {
                                break;
                            }
                            //黒の場合
                            else if (AImap[x - i, y - i] == my || AImap[x - i, y - i] == my2)
                            {
                                CheckF[x,y] = true;      //ここにはコマが置けるのでtrue
                                break;
                            }
                            //白の場合は進展なし
                        }
                        else {
                            break;
                        }
                    }
                }
            }
        }
    }

    //ひっくり返す
    //マップの指定もする
    public void TurnOver(int[,] AImap, int x, int y, int my, int my2, int ene, int ene2)
    {
        //上方向チェック
        bool CheckF = false;        //マス目が置けるかをチェック
        for (int i = y - 2; i >= 0; i--)
        {
            //隣が敵の時
            if (AImap[x, y - 1] == ene || AImap[x, y - 1] == ene2)
            {
                //置いていない時はだめ
                if (AImap[x, i] == 0)
                {
                    break;
                }
                //自の場合
                else if (AImap[x, i] == my || AImap[x, i] == my2)
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
            if (AImap[x, y - 1] == ene)
            {
                AImap[x, y - 1] = ene;
            }
            //自分の色が現れるまでひっくり返す
            for (int i = y - 2; i >= 0; i--)
            {
                //敵色だった場合
                if (AImap[x, i] == ene)
                {
                    AImap[x, i] = my;
                }
                //ひっくり返せない敵色だった場合
                else if (AImap[x, i] == ene2)
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
        for (int i = y + 2; i< 8; i++)
        {
            //隣が敵の時
            if (AImap[x, y + 1] == ene || AImap[x, y + 1] == ene2)
            {
                //置いていない時はだめ
                if (AImap[x, i] == 0)
                {
                    break;
                }
                //自の場合
                else if (AImap[x, i] == my || AImap[x, i] == my2)
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
            if (AImap[x, y + 1] == ene)
            {
                AImap[x, y + 1] = my;
            }
            //自分の色が現れるまでひっくり返す
            for (int i = y + 2; i< 8; i++)
            {
                //敵色だった場合
                if (AImap[x, i] == ene)
                {
                    AImap[x, i] = my;
                }
                else if (AImap[x, i] == ene2)
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
        for (int i = x + 2; i< 8; i++)
        {
            //隣が敵の時
            if (AImap[x + 1, y] == ene || AImap[x + 1, y] == ene2)
            {
                //置いていない時はだめ
                if (AImap[i, y] == 0)
                {
                    break;
                }
                //自の場合
                else if (AImap[i, y] == my || AImap[i, y] == my2)
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
            if (AImap[x + 1, y] == ene)
            {
                AImap[x + 1, y] = my;
            }
            //自分の色が現れるまでひっくり返す
            for (int i = x + 2; i< 8; i++)
            {
                //敵色だった場合
                if (AImap[i, y] == ene)
                {
                    AImap[i, y] = my;
                }
                if (AImap[i, y] == ene2)
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
            if (AImap[x - 1, y] == ene || AImap[x - 1, y] == ene2)
            {
                //置いていない時はだめ
                if (AImap[i, y] == 0)
                {
                    break;
                }
                //自の場合
                else if (AImap[i, y] == my || AImap[i, y] == my2)
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
            if (AImap[x - 1, y] == ene)
            {
                AImap[x - 1, y] = my;
            }
            //自分の色が現れるまでひっくり返す
            for (int i = x - 2; i >= 0; i--)
            {
                //敵色だった場合
                if (AImap[i, y] == ene)
                {
                    AImap[i, y] = my;
                }
                else if (AImap[i, y] == ene2)
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
        for (int i = 2; i< 8; i++)
        {
            //端まで検索したら
            if (x + i >= 8 || y - i <= 0) break;
            //隣が敵の時
            if (AImap[x + 1, y - 1] == ene || AImap[x + 1, y - 1] == ene2)
            {
                //置いていない時はだめ
                if (AImap[x + i, y - i] == 0)
                {
                    break;
                }
                //自の場合
                else if (AImap[x + i, y - i] == my || AImap[x + i, y - i] == my2)
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
            if (AImap[x + 1, y - 1] == ene)
            {
                AImap[x + 1, y - 1] = my;
            }
            //自分の色が現れるまでひっくり返す
            for (int i = 2; i< 8; i++)
            {
                //敵色だった場合
                if (AImap[x + i, y - i] == ene)
                {
                    AImap[x + i, y - i] = my;
                }
                else if (AImap[x + i, y - i] == ene2)
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
        for (int i = 2; i< 8; i++)
        {
            //端まで検索したら
            if (x + i >= 8 || y + i >= 8) break;
            //隣が敵の時
            if (AImap[x + 1, y + 1] == ene || AImap[x + 1, y + 1] == ene2)
            {
                //置いていない時はだめ
                if (AImap[x + i, y + i] == 0)
                {
                    break;
                }
                //自の場合
                else if (AImap[x + i, y + i] == my || AImap[x + i, y + i] == my2)
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
            if (AImap[x + 1, y + 1] == ene)
            {
                AImap[x + 1, y + 1] = my;
            }
            //自分の色が現れるまでひっくり返す
            for (int i = 2; i< 8; i++)
            {
                //敵色だった場合
                if (AImap[x + i, y + i] == ene)
                {
                    AImap[x + i, y + i] = my;
                }
                else if (AImap[x + i, y + i] == ene2)
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
        for (int i = 2; i< 8; i++)
        {
            //端まで検索したら
            if (x - i< 0 || y + i >= 8) break;
            //隣が敵の時
            if (AImap[x - 1, y + 1] == ene || AImap[x - 1, y + 1] == ene2)
            {
                //置いていない時はだめ
                if (AImap[x - i, y + i] == 0)
                {
                    break;
                }
                //自の場合
                else if (AImap[x - i, y + i] == my || AImap[x - i, y + i] == my2)
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
            if (AImap[x - 1, y + 1] == ene)
            {
                AImap[x - 1, y + 1] = my;
            }
            //自分の色が現れるまでひっくり返す
            for (int i = 2; i< 8; i++)
            {
                //敵色だった場合
                if (AImap[x - i, y + i] == ene)
                {
                    AImap[x - i, y + i] = my;
                }
                else if (AImap[x - i, y + i] == ene2)
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
        for (int i = 2; i< 8; i++)
        {
            //端まで検索したら
            if ( x - i <= 0 || y - i <= 0) break;
            //隣が敵の時
            if (AImap[x - 1, y - 1] == ene || AImap[x - 1, y - 1] == ene2)
            {
                //置いていない時はだめ
                if (AImap[x - i, y - i] == 0)
                {
                    break;
                }
                //自の場合
                else if (AImap[x - i, y - i] == my || AImap[x - i, y - i] == my2)
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
            if (AImap[x - 1, y - 1] == ene)
            {
                AImap[x - 1, y - 1] = my;
            }
            //自分の色が現れるまでひっくり返す
            for (int i = 2; i< 8; i++)
            {
                //敵色だった場合
                if (AImap[x - i, y - i] == ene)
                {
                    AImap[x - i, y - i] = my;
                }
                else if (AImap[x - i, y - i] == ene2)
                {

                }
                //自分の色だった場合
                else
                {
                    break;
                }
            }

        }
    }

    //再帰するAIの先読み
    //繰り返し数を引数にする
    void SerchInner(int Num, int score, int[,] map)
    {
        //2こマップを用意する必要あり？
        int[,] mapAITemp2 = new int[8, 8];
        int[,] mapAITemp3 = new int[8, 8];

        //ひっくり返せる場所を検索
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                CheckMap(map, i, j, 2, 4, 1, 3);
            }
        }
        
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                //始めに置き換えをする
                for (int p = 0; p < 8; p++)
                {
                    for(int q = 0; q < 8; q++)
                    {
                        mapAITemp2[p, q] = map[p, q];
                    }
                }


                //置ける場所なら
                if (CheckF[i, j])
                {
                    //ひっくり返す
                    TurnOver(mapAITemp2, i, j, 2, 4, 1, 3);
                    score += Scoremap[i,j];

                    //相手の手も読む
                    //ひっくり返せる場所を検索
                    for (int p = 0; p < 8; p++)
                    {
                        for (int q = 0; q < 8; q++)
                        {
                            CheckMap(mapAITemp2, p, q, 1, 3, 2, 4);
                        }
                    }
                    
                    for (int p = 0; p < 8; p++)
                    {
                        for (int q = 0; q < 8; q++)
                        {
                            //置ける場所なら
                            if (CheckF[p, q])
                            {
                                //マップの置き換えを行う
                                for (int r = 0; r < 8; r++)
                                {
                                    for (int s = 0; s < 8; s++)
                                    {
                                        mapAITemp3[r, s] = mapAITemp2[r, s];
                                    }
                                }

                                //ひっくり返す
                                TurnOver(mapAITemp3, p, q, 1, 3, 2, 4);
                                score -= Scoremap[p, q];

                                if (Num <= 0)
                                {
                                    SerchInner(Num-1, score, mapAITemp3);
                                }
                                else
                                {
                                    if(HighScore_Num < score)
                                    {
                                        HighScore_Num = score;
                                        XYChangeF = true;                                                                                  
                                    }
                                }
                                //最上層、最強座標を変える必要があるとき
                                if(Num == Shindo && XYChangeF)
                                {
                                    HighScore_x = p;
                                    HighScore_y = q;
                                    XYChangeF = false;
                                }
                            }
                        }
                    }


                }
            }
        }
        //置ける場所がなければ探索を終える?
        //if (HighScore_Num == -999)






        



        //差分を計算

    }
}

