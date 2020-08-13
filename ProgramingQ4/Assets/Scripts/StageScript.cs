using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*マス目のスクリプト*/

public class StageScript : MonoBehaviour
{
    public int map_x;   //xマス目はいくつか
    public int map_y;   //yマス目はいくつか

    public GameObject ParentObject;     //親となるオブジェクト
    [SerializeField] GameObject PieceObject;        //コマのオブジェクト
    public Image PieceImage;       //コマのイメージ
    [SerializeField] GameObject MarkObject;        //マークのオブジェクト

    bool EnterF;        //マウスがマスに乗っているか
    
    public bool CheckF = false;    //コマが置けるならのフラグ


    //マス色関係
    Color DefaultColor = new Color(44/255f, 166/255f, 36/255f);    //初期色
    Color EnterColor = new Color(148/255f, 245/255f, 151/255f);    //乗った時の色
    float Count = 0;        //色変化

    //両面同じ駒のフラグ
    EqualPiece equalpiece;

    void Start()
    {
        //このオブジェクト（コマ）を取得
        PieceObject = transform.GetChild(0).gameObject;
        PieceImage = PieceObject.GetComponent<Image>();
        PieceObject.SetActive(false);       //まっさらにする
        //初期位置を配置
        if((map_x == 3 && map_y == 3) || (map_x == 4 && map_y == 4))
        {
            PieceObject.SetActive(true);
            PieceImage.color = new Color(1, 1, 1);
        }
        else if ((map_x == 3 && map_y == 4) || (map_x == 4 && map_y == 3))
        {
            PieceObject.SetActive(true);
        }
        //初期位置配置
        ReversScript.map[3, 3] = ReversScript.map[4, 4] = 2;
        ReversScript.map[3, 4] = ReversScript.map[4, 3] = 1;
        //マークを取得しておく
        MarkObject = transform.GetChild(1).gameObject;
        MarkObject.SetActive(false);

        equalpiece = GameObject.Find("EqualButton").GetComponent<EqualPiece>();
    }
    void Update()
    {
        CheckMap();         //マスにコマが置けるかチェック

        //マスの上にいるとき
        if (EnterF && ReversScript.map[map_x, map_y] == 0)
        {
            if (Count < 1)
                Count += 0.2f;
        }
        //マスの上にいないとき
        else
        {
            if (Count > 0)
                Count -= 0.2f;
        }
        
        this.gameObject.GetComponent<Image>().color = Color.Lerp(DefaultColor, EnterColor, Count);
        




    }

    //マス目の上にマウスを置いた時
    public void StageEnter()
    {
        EnterF = true;
    }
    //マス目の上からマウスを離した時
    public void StageExit()
    {
        EnterF = false;
    }

    //マス目をクリックしたとき
    public void StageClick()
    {
        //コマが置けるマスでの動作
        if (CheckF)
        {
            PieceObject.SetActive(true);            //コマを配置する
            if (ReversScript.MyColor == 2 || ReversScript.MyColor == 4)           //自分が白陣だったら
                PieceObject.GetComponent<Image>().color = Color.white;

            //両面別の駒の場合
            if (!equalpiece.EqualF)
            {
                ReversScript.map[map_x, map_y] = ReversScript.MyColor;     //座標系を更新する
            }
            else
            {
                ReversScript.map[map_x, map_y] = ReversScript.MyColor2;     //座標系を更新する
            }
            ReversScript.TurnOver(map_x, map_y);    //コマをひっくり返す
            ReversScript.TurnEndF = true;           //ターンエンド
            Debug.Log("ターンエンド" + ReversScript.MyColor);

        }
    }
    


    //現地がコマを置けるか調査
    public void CheckMap()
    {
        //マスが空の時のみ見る
        if (ReversScript.map[map_x, map_y] == 0)
        {
            //上方向チェック
            for (int i = map_y - 2; i >= 0; i--)
            {
                //隣が白の時
                if (ReversScript.map[map_x, map_y - 1] == ReversScript.EnemyColor || ReversScript.map[map_x, map_y - 1] == ReversScript.EnemyColor2)
                {
                    //置いていない時はだめ
                    if (ReversScript.map[map_x, i] == 0)
                    {
                        break;
                    }
                    //黒の場合
                    else if (ReversScript.map[map_x, i] == ReversScript.MyColor || ReversScript.map[map_x, i] == ReversScript.MyColor2)
                    {
                        CheckF = true;      //ここにはコマが置けるのでtrue
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
            for (int i = map_y + 2; i < 8; i++)
            {
                //置かれる判定がされていない場合
                if (!CheckF)
                {
                    //隣が白の時
                    if (ReversScript.map[map_x, map_y + 1] == ReversScript.EnemyColor || ReversScript.map[map_x, map_y + 1] == ReversScript.EnemyColor2)
                    {
                        //置いていない時はだめ
                        if (ReversScript.map[map_x, i] == 0)
                        {
                            break;
                        }
                        //黒の場合
                        else if (ReversScript.map[map_x, i] == ReversScript.MyColor || ReversScript.map[map_x, i] == ReversScript.MyColor2)
                        {
                            CheckF = true;      //ここにはコマが置けるのでtrue
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
            for (int i = map_x - 2; i >= 0; i--)
            {
                //置かれる判定がされていない場合
                if (!CheckF)
                {
                    //隣が白の時
                    if (ReversScript.map[map_x - 1, map_y] == ReversScript.EnemyColor || ReversScript.map[map_x - 1, map_y] == ReversScript.EnemyColor2)
                    {
                        //置いていない時はだめ
                        if (ReversScript.map[i, map_y] == 0)
                        {
                            break;
                        }
                        //黒の場合
                        else if (ReversScript.map[i, map_y] == ReversScript.MyColor || ReversScript.map[i, map_y] == ReversScript.MyColor2)
                        {
                            CheckF = true;      //ここにはコマが置けるのでtrue
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
            for (int i = map_x + 2; i < 8; i++)
            {
                if (ReversScript.map[map_x, map_y] == 0)
                {
                    //置かれる判定がされていない場合
                    if (!CheckF)
                    {
                        //隣が白の時
                        if (ReversScript.map[map_x + 1, map_y] == ReversScript.EnemyColor || ReversScript.map[map_x + 1, map_y] == ReversScript.EnemyColor2)
                        {
                            //置いていない時はだめ
                            if (ReversScript.map[i, map_y] == 0)
                            {
                                break;
                            }
                            //黒の場合
                            else if (ReversScript.map[i, map_y] == ReversScript.MyColor || ReversScript.map[i, map_y] == ReversScript.MyColor2)
                            {
                                CheckF = true;      //ここにはコマが置けるのでtrue
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
                if (ReversScript.map[map_x, map_y] == 0 && map_x+i < 8 && map_y-i >= 0)
                {
                    //置かれる判定がされていない場合
                    if (!CheckF)
                    {
                        //隣が白の時
                        if (ReversScript.map[map_x + 1, map_y - 1] == ReversScript.EnemyColor || ReversScript.map[map_x + 1, map_y - 1] == ReversScript.EnemyColor2)
                        {
                            //置いていない時はだめ
                            if (ReversScript.map[map_x + i, map_y - i] == 0)
                            {
                                break;
                            }
                            //黒の場合
                            else if (ReversScript.map[map_x + i, map_y - i] == ReversScript.MyColor || ReversScript.map[map_x + i, map_y - i] == ReversScript.MyColor2)
                            {
                                CheckF = true;      //ここにはコマが置けるのでtrue
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
                if (ReversScript.map[map_x, map_y] == 0 && map_x + i < 8 && map_y + i < 8)
                {
                    //置かれる判定がされていない場合
                    if (!CheckF)
                    {
                        //隣が白の時
                        if (ReversScript.map[map_x + 1, map_y +1] == ReversScript.EnemyColor || ReversScript.map[map_x + 1, map_y + 1] == ReversScript.EnemyColor2)
                        {
                            //置いていない時はだめ
                            if (ReversScript.map[map_x + i, map_y + i] == 0)
                            {
                                break;
                            }
                            //黒の場合
                            else if (ReversScript.map[map_x + i, map_y + i] == ReversScript.MyColor || ReversScript.map[map_x + i, map_y + i] == ReversScript.MyColor2)
                            {
                                CheckF = true;      //ここにはコマが置けるのでtrue
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
                if (ReversScript.map[map_x, map_y] == 0 && map_x - i >= 0 && map_y + i < 8)
                {
                    //置かれる判定がされていない場合
                    if (!CheckF)
                    {
                        //隣が白の時
                        if (ReversScript.map[map_x - 1, map_y + 1] == ReversScript.EnemyColor || ReversScript.map[map_x - 1, map_y + 1] == ReversScript.EnemyColor2)
                        {
                            //置いていない時はだめ
                            if (ReversScript.map[map_x - i, map_y + i] == 0)
                            {
                                break;
                            }
                            //黒の場合
                            else if (ReversScript.map[map_x - i, map_y + i] == ReversScript.MyColor || ReversScript.map[map_x - i, map_y + i] == ReversScript.MyColor2)
                            {
                                CheckF = true;      //ここにはコマが置けるのでtrue
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
                if (ReversScript.map[map_x, map_y] == 0 && map_x - i >= 0 && map_y - i >= 0)
                {
                    //置かれる判定がされていない場合
                    if (!CheckF)
                    {
                        //隣が白の時
                        if (ReversScript.map[map_x - 1, map_y - 1] == ReversScript.EnemyColor || ReversScript.map[map_x - 1, map_y - 1] == ReversScript.EnemyColor2)
                        {
                            //置いていない時はだめ
                            if (ReversScript.map[map_x - i, map_y - i] == 0)
                            {
                                break;
                            }
                            //黒の場合
                            else if (ReversScript.map[map_x - i, map_y - i] == ReversScript.MyColor || ReversScript.map[map_x - i, map_y - i] == ReversScript.MyColor2)
                            {
                                CheckF = true;      //ここにはコマが置けるのでtrue
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

        //赤色マーカーを置く
        if (CheckF && ReversScript.map[map_x,map_y] == 0)
        {
            MarkObject.SetActive(true);
        }
        else
        {
            MarkObject.SetActive(false);
        }
    }
    
}
