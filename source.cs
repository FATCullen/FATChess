using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Pieces : MonoBehaviour
{
    //Defining some variables
    public Sprite white_king, white_queen, white_rook, white_bishop, white_knight, white_pawn,
    black_king, black_queen, black_rook, black_bishop, black_knight, black_pawn, empty, marker, menu, game_over;
    public int[,] PieceArray;
    int collums = 8; 
    int rows = 8;
    public Vector3 mousepos1, mousepos2;
    public Vector3Int pos1round, pos2round;
    public bool secondclick = false;
    public bool firstspawn = true;
    public int move = 0;
    public bool whiteenpassant = false;
    public bool blackenpassant = false;
    public Vector3Int whiteenpassantcords = Vector3Int.zero;
    public Vector3Int blackenpassantcords = Vector3Int.zero;
    public bool whitecastleright = true;
    public bool whitecastleleft = true;
    public bool blackcastleright = true;
    public bool blackcastleleft = true;
    public bool usingai;
    public bool checkingforcheck = false;
    public int promotion = 0;
    public bool promotionrequired = false;
    public int currentmovevalue = -1000;
    public int bestmovevalue = -1000;
    int[,] isBoardSame;
    int[,] savedBoard;
    bool menunotcomplete = false;
    bool gameover = false;
    //Start is called before the first frame update
    void Start()
    {
        Menu();
    }
    //Print the pieces from the array to the board
    private void SpawnPieces(int x, int y, int value)
    {
        string objecttodestroy;
        if (firstspawn == false)
        {
            for (int x2 = 0; x2 < collums; x2++)
            {
                for (int y2 = 0; y2 < rows; y2++)
                {
                    int x3 = x2;
                    int y3 = y2;
                    objecttodestroy = "x: " + x3.ToString() + " y: " + y3.ToString();
                    GameObject destroyobject = GameObject.Find(objecttodestroy);
                    Destroy(destroyobject.gameObject);
                }
            }
        }
        //Creating object at correct position
        GameObject g = new GameObject("x: " + x + " y: " + y);
        g.transform.position = new Vector3(y - (4 - 0.5f), (-x + 7) - (4 - 0.5f), -1);
        var s = g.AddComponent<SpriteRenderer>();
        //Giving the correct sprite based on the value
        if (PieceArray[x, y] == 0)
        {
            s.sprite = empty;
        }
        else if (PieceArray[x, y] == 1)
        {
            s.sprite = white_king;
        }
        else if (PieceArray[x, y] == 2)
        {
            s.sprite = white_queen;
        }
        else if (PieceArray[x, y] == 3)
        {
            s.sprite = white_rook;
        }
        else if (PieceArray[x, y] == 4)
        {
            s.sprite = white_bishop;
        }
        else if (PieceArray[x, y] == 5)
        {
            s.sprite = white_knight;
        }
        else if (PieceArray[x, y] == 6)
        {
            s.sprite = white_pawn;
        }
        else if (PieceArray[x, y] == 7)
        {
            s.sprite = black_king;
        }
        else if (PieceArray[x, y] == 8)
        {
            s.sprite = black_queen;
        }
        else if (PieceArray[x, y] == 9)
        {
            s.sprite = black_rook;
        }
        else if (PieceArray[x, y] == 10)
        {
            s.sprite = black_bishop;
        }
        else if (PieceArray[x, y] == 11)
        {
            s.sprite = black_knight;
        }
        else if (PieceArray[x, y] == 12)
        {
            s.sprite = black_pawn;
        }
    }
    //Gets player input and modifies array
    private void Update()
    {
        if (menunotcomplete == true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) == true)
            {
                if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < 0 && Camera.main.ScreenToWorldPoint(Input.mousePosition).x > -2 && Camera.main.ScreenToWorldPoint(Input.mousePosition).y < 0 && Camera.main.ScreenToWorldPoint(Input.mousePosition).y > -2)
                {
                    usingai = true;
                    menunotcomplete = false;
                    string objecttodestroy;
                    objecttodestroy = "Menu";
                    GameObject destroyobject = GameObject.Find(objecttodestroy);
                    Destroy(destroyobject.gameObject);
                    Reset();
                }
                if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > 0 && Camera.main.ScreenToWorldPoint(Input.mousePosition).x < 2 && Camera.main.ScreenToWorldPoint(Input.mousePosition).y < 0 && Camera.main.ScreenToWorldPoint(Input.mousePosition).y > -2)
                {
                    usingai = false;
                    menunotcomplete = false;
                    string objecttodestroy;
                    objecttodestroy = "Menu";
                    GameObject destroyobject = GameObject.Find(objecttodestroy);
                    Destroy(destroyobject.gameObject);
                    Reset();
                }
            }
        }
        if (gameover == true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) == true)
            {
                if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < 0 && Camera.main.ScreenToWorldPoint(Input.mousePosition).x > -2 && Camera.main.ScreenToWorldPoint(Input.mousePosition).y < 0 && Camera.main.ScreenToWorldPoint(Input.mousePosition).y > -2)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > 0 && Camera.main.ScreenToWorldPoint(Input.mousePosition).x < 2 && Camera.main.ScreenToWorldPoint(Input.mousePosition).y < 0 && Camera.main.ScreenToWorldPoint(Input.mousePosition).y > -2)
                {
                    Application.Quit();
                }
            }
        }
        if (menunotcomplete == false && gameover == false)
        {
            if (move % 2 == 0)
            {
                blackenpassant = false;
                blackenpassantcords = Vector3Int.zero;
            }
            if (move % 2 == 1)
            {
                whiteenpassant = false;
                whiteenpassantcords = Vector3Int.zero;
            }
            if (move % 2 == 1 && usingai == true)
            {
                isBoardSame = savedBoard;
                savedBoard = PieceArray;
                int[] AIMoveCords = AIMove();
                pos1round.x = AIMoveCords[0];
                pos1round.y = AIMoveCords[1];
                pos2round.x = AIMoveCords[2];
                pos2round.y = AIMoveCords[3];
                IsMoveLegal(-1 * (pos1round.y - 3), pos1round.x + 4, -1 * (pos2round.y - 3), pos2round.x + 4);
                if (promotionrequired == true)
                {
                    PieceArray[-1 * (pos1round.y - 3), pos1round.x + 4] = promotion;
                    promotion = 0;
                    promotionrequired = false;
                }
                PieceArray[-1 * (pos2round.y - 3), pos2round.x + 4] = PieceArray[-1 * (pos1round.y - 3), pos1round.x + 4];
                PieceArray[-1 * (pos1round.y - 3), pos1round.x + 4] = 0;
                pos1round = new Vector3Int(0, 0, 0);
                pos2round = new Vector3Int(0, 0, 0);
                mousepos1 = new Vector3(0, 0, 0);
                mousepos2 = new Vector3(0, 0, 0);
                for (int x = 0; x < collums; x++)
                {
                    for (int y = 0; y < rows; y++)
                    {
                        SpawnPieces(x, y, PieceArray[x, y]);
                    }
                }
                secondclick = false;
                move++;
                bool check = false;
                for (int a = 0; a < 8; a++)
                {
                    for (int b = 0; b < 8; b++)
                    {
                        for (int c = 0; c < 8; c++)
                        {
                            for (int d = 0; d < 8; d++)
                            {
                                if (IsMoveLegal(a, b, c, d) == true)
                                {
                                    check = true;
                                }
                            }
                        }
                    }
                }
                if (check == false)
                {
                    gameover = true;
                    GameOver();
                }
            }
            if (Input.GetKeyDown(KeyCode.Mouse0) == true && pos1round == new Vector3Int(0, 0, 0) && (move % 2 == 0 || (move % 2 == 1 && usingai == false)))
            {
                mousepos1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos1round = new Vector3Int(Mathf.FloorToInt(mousepos1.x), Mathf.FloorToInt(mousepos1.y), 1);
                GameObject g = new GameObject("Marker");
                g.transform.position = new Vector3(pos1round.x + 0.5f, pos1round.y + 0.5f, 0);
                var s = g.AddComponent<SpriteRenderer>();
                s.sprite = marker;
            }
            if (Input.GetKeyDown(KeyCode.Mouse0) == true && pos1round != new Vector3Int(0, 0, 0) && secondclick == true && (move % 2 == 0 || (move % 2 == 1 && usingai == false)))
            {
                mousepos2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos2round = new Vector3Int(Mathf.FloorToInt(mousepos2.x), Mathf.FloorToInt(mousepos2.y), 1);
            }
            if (secondclick == true && pos1round == pos2round && (move % 2 == 0 || (move % 2 == 1 && usingai == false)))
            {
                pos1round = new Vector3Int(0, 0, 0);
                pos2round = new Vector3Int(0, 0, 0);
                mousepos1 = new Vector3(0, 0, 0);
                mousepos2 = new Vector3(0, 0, 0);
                secondclick = false;
                Destroy(GameObject.Find("Marker"));
            }
            if (pos1round != new Vector3Int(0, 0, 0) && pos2round != new Vector3Int(0, 0, 0) && pos1round != pos2round)
            {
                if (IsMoveLegal(-1 * (pos1round.y - 3), pos1round.x + 4, -1 * (pos2round.y - 3), pos2round.x + 4) == true)
                {
                    if (promotionrequired == true)
                    {
                        PieceArray[-1 * (pos1round.y - 3), pos1round.x + 4] = promotion;
                        promotion = 0;
                        promotionrequired = false;
                    }
                    PieceArray[-1 * (pos2round.y - 3), pos2round.x + 4] = PieceArray[-1 * (pos1round.y - 3), pos1round.x + 4];
                    PieceArray[-1 * (pos1round.y - 3), pos1round.x + 4] = 0;
                    pos1round = new Vector3Int(0, 0, 0);
                    pos2round = new Vector3Int(0, 0, 0);
                    mousepos1 = new Vector3(0, 0, 0);
                    mousepos2 = new Vector3(0, 0, 0);
                    for (int x = 0; x < collums; x++)
                    {
                        for (int y = 0; y < rows; y++)
                        {
                            SpawnPieces(x, y, PieceArray[x, y]);
                        }
                    }
                    secondclick = false;
                    move++;
                    Destroy(GameObject.Find("Marker"));
                    bool check = false;
                    for (int a = 0; a < 8; a++)
                    {
                        for (int b = 0; b < 8; b++)
                        {
                            for (int c = 0; c < 8; c++)
                            {
                                for (int d = 0; d < 8; d++)
                                {
                                    if (IsMoveLegal(a, b, c, d) == true)
                                    {
                                        check = true;
                                    }
                                }
                            }
                        }
                    }
                    if (check == false)
                    {
                        gameover = true;
                        GameOver();
                    }
                }
                else
                {
                    pos1round = new Vector3Int(0, 0, 0);
                    pos2round = new Vector3Int(0, 0, 0);
                    mousepos1 = new Vector3(0, 0, 0);
                    mousepos2 = new Vector3(0, 0, 0);
                    secondclick = false;
                    Destroy(GameObject.Find("Marker"));
                }
            }
            if (pos1round != new Vector3Int(0, 0, 0))
            {
                secondclick = true;
            }
        }
    }
    //Checking if move is legal
    public bool IsMoveLegal(int startx, int starty, int endx, int endy)
    {
        if (PieceArray[startx, starty] == 0)
        {
            return false;
        }
        //Checking input is valid
        if (startx >= 8 || startx < 0 || starty >= 8 || starty < 0 || endx >= 8 || endx < 0 || endy >= 8 || endy < 0)
        {
            return false;
        }
        if (checkingforcheck == false)
        {
            int storeendvalue = PieceArray[endx, endy];
            PieceArray[endx, endy] = PieceArray[startx, starty];
            PieceArray[startx, starty] = 0;
            move++;
            checkingforcheck = true;
            for (int a = 0; a < 8; a++)
            {
                for (int b = 0; b < 8; b++)
                {
                    for (int c = 0; c < 8; c++)
                    {
                        for (int d = 0; d < 8; d++)
                        {
                            if (IsMoveLegal(a, b, c, d) == true && ((move % 2 == 0 && PieceArray[c, d] == 7) || (move % 2 == 1 && PieceArray[c, d] == 1)))
                            {
                                checkingforcheck = false;
                                move--;
                                PieceArray[startx, starty] = PieceArray[endx, endy];
                                PieceArray[endx, endy] = storeendvalue;
                                promotion = 0;
                                promotionrequired = false;
                                return false;
                            }
                        }
                    }
                }
            }
            checkingforcheck = false;
            move--;
            PieceArray[startx, starty] = PieceArray[endx, endy];
            PieceArray[endx, endy] = storeendvalue;
            promotion = 0;
            promotionrequired = false;
        }
        //White pawn
        if (PieceArray[startx, starty] == 6 && move % 2 == 0)
        {
            //Moving 2 squares on first move
            if (startx == 6 && endx == 4 && starty == endy && PieceArray[endx + 1, endy] == 0 && PieceArray[endx, endy] == 0)
            {
                if (checkingforcheck == false)
                {
                    blackenpassant = true;
                    blackenpassantcords = new Vector3Int(endx, endy, 1);
                }
                return true;
            }
            //Moving 1 square
            else if (startx - 1 == endx && starty == endy && PieceArray[endx, endy] == 0)
            {
                if (endx == 0)
                {
                    promotion = getpromotioninput();
                    if (promotion == 0)
                    {
                        return false;
                    }
                    else
                    {
                        promotionrequired = true;
                    }
                }
                return true;
            }
            //Taking Diagonally
            else if (startx - 1 == endx && (starty + 1 == endy || starty - 1 == endy) && PieceArray[endx, endy] >= 7)
            {
                if (endx == 0)
                {
                    promotion = getpromotioninput();
                    if (promotion == 0)
                    {
                        if (checkingforcheck == true)
                        {
                            return true;
                        }
                        if (checkingforcheck == false)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        promotionrequired = true;
                    }
                }
                return true;
            }
            //En passant
            else if (startx - 1 == endx && (starty + 1 == endy || starty - 1 == endy) && PieceArray[endx, endy] == 0 && whiteenpassant == true && whiteenpassantcords == new Vector3Int(startx, endy, 1))
            {
                if (checkingforcheck == false)
                {
                    PieceArray[startx, endy] = 0;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        //Black pawn
        if (PieceArray[startx, starty] == 12 && move % 2 == 1)
        {
            //Moving 2 squares on first move
            if (startx == 1 && endx == 3 && starty == endy && PieceArray[endx - 1, endy] == 0 && PieceArray[endx, endy] == 0)
            {
                if (checkingforcheck == false)
                {
                    whiteenpassant = true;
                    whiteenpassantcords = new Vector3Int(endx, endy, 1);
                }
                return true;
            }
            //Moving 1 square
            else if (startx + 1 == endx && starty == endy && PieceArray[endx, endy] == 0)
            {
                if (endx == 7)
                {
                    if (usingai == true)
                    {
                        if (checkingforcheck == false)
                        {
                            promotion = 8;
                            promotionrequired = true;
                        }
                    }
                    if (usingai == false)
                    {
                        if (checkingforcheck == false)
                        {
                            promotion = getpromotioninput();
                            if (promotion == 0)
                            {
                                if (checkingforcheck == true)
                                {
                                    return true;
                                }
                                if (checkingforcheck == false)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                promotionrequired = true;
                            }
                        }
                    }
                }
                return true;
            }
            //Taking Diagonally
            else if (startx + 1 == endx && (starty - 1 == endy || starty + 1 == endy) && PieceArray[endx, endy] <= 6 && PieceArray[endx, endy] != 0)
            {
                if (endx == 7)
                {
                    if (usingai == true)
                    {
                        if (checkingforcheck == false)
                        {
                            promotion = 8;
                            promotionrequired = true;
                        }
                    }
                    if (usingai == false)
                    {
                        if (checkingforcheck == false)
                        {
                            promotion = getpromotioninput();
                            if (promotion == 0)
                            {
                                if (checkingforcheck == true)
                                {
                                    return true;
                                }
                                if (checkingforcheck == false)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                promotionrequired = true;
                            }
                        }
                    }
                }
                return true;
            }
            //En passant
            else if (startx + 1 == endx && (starty + 1 == endy || starty - 1 == endy) && PieceArray[endx, endy] == 0 && blackenpassant == true && blackenpassantcords == new Vector3Int(startx, endy, 1))
            {
                if (checkingforcheck == false)
                {
                    PieceArray[startx, endy] = 0;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        //White king
        if (PieceArray[startx, starty] == 1 && move % 2 == 0)
        {
            if ((((startx + 1 == endx || startx - 1 == endx) && (starty + 1 == endy || starty - 1 == endy)) || ((startx + 1 == endx || startx - 1 == endx) && starty == endy) || ((starty + 1 == endy || starty - 1 == endy) && startx == endx)) && (PieceArray[endx, endy] == 0 || PieceArray[endx, endy] >= 7))
            {
                if (checkingforcheck == false)
                {
                    whitecastleleft = false;
                    whitecastleright = false;
                }
                return true;
            }
            //Castling right
            else if (whitecastleright == true && PieceArray[7, 5] == 0 && PieceArray[7, 6] == 0 && PieceArray[7, 7] == 3 && endx == 7 && endy == 6)
            {
                move++;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (IsMoveLegal(i, j, 7, 4) == true)
                        {
                            move--;
                            return false;
                        }
                        else if (IsMoveLegal(i, j, 7, 5) == true)
                        {
                            move--;
                            return false;
                        }
                    }
                }
                move--;
                if (checkingforcheck == false)
                {
                    whitecastleright = false;
                    whitecastleleft = false;
                    PieceArray[7, 7] = 0;
                    PieceArray[7, 5] = 3;
                }
                return true;
            }
            //Castling left
            else if (whitecastleleft == true && PieceArray[7, 3] == 0 && PieceArray[7, 2] == 0 && PieceArray[7, 1] == 0 && PieceArray[7, 0] == 3 && endx == 7 && endy == 2)
            {
                move++;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (IsMoveLegal(i, j, 7, 4) == true)
                        {
                            move--;
                            return false;
                        }
                        else if (IsMoveLegal(i, j, 7, 3) == true)
                        {
                            move--;
                            return false;
                        }
                    }
                }
                move--;
                if (checkingforcheck == false)
                {
                    whitecastleright = false;
                    whitecastleleft = false;
                    PieceArray[7, 0] = 0;
                    PieceArray[7, 3] = 3;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        //Black King
        if (PieceArray[startx, starty] == 7 && move % 2 == 1)
        {
            if ((((startx + 1 == endx || startx - 1 == endx) && (starty + 1 == endy || starty - 1 == endy)) || ((startx + 1 == endx || startx - 1 == endx) && starty == endy) || ((starty + 1 == endy || starty - 1 == endy) && startx == endx)) && (PieceArray[endx, endy] == 0 || PieceArray[endx, endy] <= 6))
            {
                if (checkingforcheck == false)
                {
                    blackcastleleft = false;
                    blackcastleright = false;
                }
                return true;
            }
            //Castling right
            else if (blackcastleright == true && PieceArray[0, 5] == 0 && PieceArray[0, 6] == 0 && PieceArray[0, 7] == 9 && endx == 0 && endy == 6)
            {
                move++;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (IsMoveLegal(i, j, 0, 4) == true)
                        {
                            move--;
                            return false;
                        }
                        else if (IsMoveLegal(i, j, 0, 5) == true)
                        {
                            move--;
                            return false;
                        }
                    }
                }
                move--;
                if (checkingforcheck == false)
                {
                    blackcastleright = false;
                    blackcastleleft = false;
                    PieceArray[0, 7] = 0;
                    PieceArray[0, 5] = 9;
                }
                return true;
            }
            //Castling left
            else if (blackcastleleft == true && PieceArray[0, 3] == 0 && PieceArray[0, 2] == 0 && PieceArray[0, 1] == 0 && PieceArray[0, 0] == 9 && endx == 0 && endy == 2)
            {
                move++;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (IsMoveLegal(i, j, 0, 4) == true)
                        {
                            move--;
                            return false;
                        }
                        else if (IsMoveLegal(i, j, 0, 3) == true)
                        {
                            move--;
                            return false;
                        }
                    }
                }
                move--;
                if (checkingforcheck == false)
                {
                    blackcastleright = false;
                    blackcastleleft = false;
                    PieceArray[0, 0] = 0;
                    PieceArray[0, 3] = 9;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        //White knight
        if (PieceArray[startx, starty] == 5 && move % 2 == 0)
        {
            if ((((startx + 2 == endx || startx - 2 == endx) && (starty + 1 == endy || starty - 1 == endy)) || ((startx + 1 == endx || startx - 1 == endx) && (starty + 2 == endy || starty - 2 == endy))) && (PieceArray[endx, endy] == 0 || PieceArray[endx, endy] >= 7))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //Black knight
        if (PieceArray[startx, starty] == 11 && move % 2 == 1)
        {
            if ((((startx + 2 == endx || startx - 2 == endx) && (starty + 1 == endy || starty - 1 == endy)) || ((startx + 1 == endx || startx - 1 == endx) && (starty + 2 == endy || starty - 2 == endy))) && (PieceArray[endx, endy] == 0 || PieceArray[endx, endy] <= 6))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //White rook
        if (PieceArray[startx, starty] == 3 && move % 2 == 0)
        {
            if (startx == endx && starty != endy && (PieceArray[endx, endy] == 0 || PieceArray[endx, endy] >= 7))
            {
                if (starty < endy)
                {
                    for (int i = starty + 1; i < endy; i++)
                    {
                        if (PieceArray[startx, i] != 0)
                        {
                            return false;
                        }
                    }
                    if (startx == 7 && starty == 7)
                    {
                        if (checkingforcheck == false)
                        {
                            whitecastleright = false;
                        }
                    }
                    if (startx == 7 && starty == 0)
                    {
                        if (checkingforcheck == false)
                        {
                            whitecastleleft = false;
                        }
                    }
                    return true;
                }
                if (starty > endy)
                {
                    for (int i = starty - 1; i > endy; i--)
                    {
                        if (PieceArray[startx, i] != 0)
                        {
                            return false;
                        }
                    }
                    if (startx == 7 && starty == 7)
                    {
                        if (checkingforcheck == false)
                        {
                            whitecastleright = false;
                        }
                    }
                    if (startx == 7 && starty == 0)
                    {
                        if (checkingforcheck == false)
                        {
                            whitecastleleft = false;
                        }
                    }
                    return true;
                }
            }
            else if  (startx != endx && starty == endy && (PieceArray[endx, endy] == 0 || PieceArray[endx, endy] >= 7))
            {
                if (startx < endx)
                {
                    for (int i = startx + 1; i < endx; i++)
                    {
                        if (PieceArray[i, starty] != 0)
                        {
                            return false;
                        }
                    }
                    if (startx == 7 && starty == 7)
                    {
                        if (checkingforcheck == false)
                        {
                            whitecastleright = false;
                        }
                    }
                    if (startx == 7 && starty == 0)
                    {
                        if (checkingforcheck == false)
                        {
                            whitecastleleft = false;
                        }
                    }
                    return true;
                }
                if (startx > endx)
                {
                    for (int i = startx - 1; i > endx; i--)
                    {
                        if (PieceArray[i, starty] != 0)
                        {
                            return false;
                        }
                    }
                    if (startx == 7 && starty == 7)
                    {
                        if (checkingforcheck == false)
                        {
                            whitecastleright = false;
                        }
                    }
                    if (startx == 7 && starty == 0)
                    {
                        if (checkingforcheck == false)
                        {
                            whitecastleleft = false;
                        }
                    }
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        //Black rook
        if (PieceArray[startx, starty] == 9 && move % 2 == 1)
        {
            if (startx == endx && starty != endy && (PieceArray[endx, endy] == 0 || PieceArray[endx, endy] <= 6))
            {
                if (starty < endy)
                {
                    for (int i = starty + 1; i < endy; i++)
                    {
                        if (PieceArray[startx, i] != 0)
                        {
                            return false;
                        }
                    }
                    if (startx == 0 && starty == 7)
                    {
                        if (checkingforcheck == false)
                        {
                            blackcastleright = false;
                        }
                    }
                    if (startx == 0 && starty == 0)
                    {
                        if (checkingforcheck == false)
                        {
                            blackcastleleft = false;
                        }
                    }
                    return true;
                }
                if (starty > endy)
                {
                    for (int i = starty - 1; i > endy; i--)
                    {
                        if (PieceArray[startx, i] != 0)
                        {
                            return false;
                        }
                    }
                    if (startx == 0 && starty == 7)
                    {
                        if (checkingforcheck == false)
                        {
                            blackcastleright = false;
                        }
                    }
                    if (startx == 0 && starty == 0)
                    {
                        if (checkingforcheck == false)
                        {
                            blackcastleleft = false;
                        }
                    }
                    return true;
                }
            }
            else if (startx != endx && starty == endy && (PieceArray[endx, endy] == 0 || PieceArray[endx, endy] <= 6))
            {
                if (startx < endx)
                {
                    for (int i = startx + 1; i < endx; i++)
                    {
                        if (PieceArray[i, starty] != 0)
                        {
                            return false;
                        }
                    }
                    if (startx == 0 && starty == 7)
                    {
                        if (checkingforcheck == false)
                        {
                            blackcastleright = false;
                        }
                    }
                    if (startx == 0 && starty == 0)
                    {
                        if (checkingforcheck == false)
                        {
                            blackcastleleft = false;
                        }
                    }
                    return true;
                }
                if (startx > endx)
                {
                    for (int i = startx - 1; i > endx; i--)
                    {
                        if (PieceArray[i, starty] != 0)
                        {
                            return false;
                        }
                    }
                    if (startx == 0 && starty == 7)
                    {
                        if (checkingforcheck == false)
                        {
                            blackcastleright = false;
                        }
                    }
                    if (startx == 0 && starty == 0)
                    {
                        if (checkingforcheck == false)
                        {
                            blackcastleleft = false;
                        }
                    }
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        //White bishop
        if (PieceArray[startx, starty] == 4 && move % 2 == 0)
        {
            if (startx - endx == starty - endy && (PieceArray[endx, endy] == 0 || PieceArray[endx, endy] >=7))
            {
                if (startx < endx && starty < endy)
                {
                    for (int i = -1; i > startx - endx; i--)
                    {
                        if (PieceArray[startx - i, starty - i] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                if (startx > endx && starty > endy)
                {
                    for (int i = 1; i < startx - endx; i++)
                    {
                        if (PieceArray[startx - i, starty - i] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            if (startx - endx == -(starty - endy) && (PieceArray[endx, endy] == 0 || PieceArray[endx, endy] >= 7))
            {
                if (startx > endx && starty < endy)
                {
                    for (int i = -1; i > endx - startx; i--)
                    {
                        if (PieceArray[startx + i, starty - i] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                if (startx < endx && starty > endy)
                {
                    for (int i = 1; i < endx - startx; i++)
                    {
                        if (PieceArray[startx + i, starty - i] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
        }
        //Black bishop
        if (PieceArray[startx, starty] == 10 && move % 2 == 1)
        {
            if (startx - endx == starty - endy && (PieceArray[endx, endy] == 0 || PieceArray[endx, endy] <= 6))
            {
                if (startx < endx && starty < endy)
                {
                    for (int i = -1; i > startx - endx; i--)
                    {
                        if (PieceArray[startx - i, starty - i] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                if (startx > endx && starty > endy)
                {
                    for (int i = 1; i < startx - endx; i++)
                    {
                        if (PieceArray[startx - i, starty - i] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            if (startx - endx == -(starty - endy) && (PieceArray[endx, endy] == 0 || PieceArray[endx, endy] <= 6))
            {
                if (startx > endx && starty < endy)
                {
                    for (int i = -1; i > endx - startx; i--)
                    {
                        if (PieceArray[startx + i, starty - i] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                if (startx < endx && starty > endy)
                {
                    for (int i = 1; i < endx - startx; i++)
                    {
                        if (PieceArray[startx + i, starty - i] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
        }
        //White queen
        if (PieceArray[startx, starty] == 2 && move % 2 == 0)
        {
            if (startx == endx && starty != endy && (PieceArray[endx, endy] == 0 || PieceArray[endx, endy] >= 7))
            {
                if (starty < endy)
                {
                    for (int i = starty + 1; i < endy; i++)
                    {
                        if (PieceArray[startx, i] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                if (starty > endy)
                {
                    for (int i = starty - 1; i > endy; i--)
                    {
                        if (PieceArray[startx, i] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            if (startx != endx && starty == endy && (PieceArray[endx, endy] == 0 || PieceArray[endx, endy] >= 7))
            {
                if (startx < endx)
                {
                    for (int i = startx + 1; i < endx; i++)
                    {
                        if (PieceArray[i, starty] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                if (startx > endx)
                {
                    for (int i = startx - 1; i > endx; i--)
                    {
                        if (PieceArray[i, starty] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            if (startx - endx == starty - endy && (PieceArray[endx, endy] == 0 || PieceArray[endx, endy] >= 7))
            {
                if (startx < endx && starty < endy)
                {
                    for (int i = -1; i > startx - endx; i--)
                    {
                        if (PieceArray[startx - i, starty - i] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                if (startx > endx && starty > endy)
                {
                    for (int i = 1; i < startx - endx; i++)
                    {
                        if (PieceArray[startx - i, starty - i] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            if (startx - endx == -(starty - endy) && (PieceArray[endx, endy] == 0 || PieceArray[endx, endy] >= 7))
            {
                if (startx > endx && starty < endy)
                {
                    for (int i = -1; i > endx - startx; i--)
                    {
                        if (PieceArray[startx + i, starty - i] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                if (startx < endx && starty > endy)
                {
                    for (int i = 1; i < endx - startx; i++)
                    {
                        if (PieceArray[startx + i, starty - i] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
                return false;
        }
        //Black queen
        if (PieceArray[startx, starty] == 8 && move % 2 == 1)
        {
            if (startx == endx && starty != endy && (PieceArray[endx, endy] == 0 || PieceArray[endx, endy] <= 6))
            {
                if (starty < endy)
                {
                    for (int i = starty + 1; i < endy; i++)
                    {
                        if (PieceArray[startx, i] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                if (starty > endy)
                {
                    for (int i = starty - 1; i > endy; i--)
                    {
                        if (PieceArray[startx, i] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            if (startx != endx && starty == endy && (PieceArray[endx, endy] == 0 || PieceArray[endx, endy] <= 6))
            {
                if (startx < endx)
                {
                    for (int i = startx + 1; i < endx; i++)
                    {
                        if (PieceArray[i, starty] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                if (startx > endx)
                {
                    for (int i = startx - 1; i > endx; i--)
                    {
                        if (PieceArray[i, starty] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            if (startx - endx == starty - endy && (PieceArray[endx, endy] == 0 || PieceArray[endx, endy] <= 6))
            {
                if (startx < endx && starty < endy)
                {
                    for (int i = -1; i > startx - endx; i--)
                    {
                        if (PieceArray[startx - i, starty - i] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                if (startx > endx && starty > endy)
                {
                    for (int i = 1; i < startx - endx; i++)
                    {
                        if (PieceArray[startx - i, starty - i] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            if (startx - endx == -(starty - endy) && (PieceArray[endx, endy] == 0 || PieceArray[endx, endy] <= 6))
            {
                if (startx > endx && starty < endy)
                {
                    for (int i = -1; i > endx - startx; i--)
                    {
                        if (PieceArray[startx + i, starty - i] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                if (startx < endx && starty > endy)
                {
                    for (int i = 1; i < endx - startx; i++)
                    {
                        if (PieceArray[startx + i, starty - i] != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        return false;
    }
    //Promotions
    public int getpromotioninput()
    {
        bool done = false;
        while (!done)
        {
            if (Input.GetKey(KeyCode.Q) == true)
            {
                if (move % 2 == 0)
                {
                    return 2;
                }
                else if (move % 2 == 1)
                {
                    return 8;
                }
            }
            if (Input.GetKey(KeyCode.K) == true)
            {
                if (move % 2 == 0)
                {
                    return 5;
                }
                else if (move % 2 == 1)
                {
                    return 11;
                }
            }
            if (Input.GetKey(KeyCode.R) == true)
            {
                if (move % 2 == 0)
                {
                    return 3;
                }
                else if (move % 2 == 1)
                {
                    return 9;
                }
            }
            if (Input.GetKey(KeyCode.B) == true)
            {
                if (move % 2 == 0)
                {
                    return 4;
                }
                else if (move % 2 == 1)
                {
                    return 10;
                }
            }
            else
            {
                return 0;
            }
        }
        return 0;
    }
    public int[] AIMove()
    {
        int[,] PieceArrayCopy = PieceArray;
        int[] bestmove = new int[] {20, 20, 20, 20};
        int counter = 0;
        checkingforcheck = true;
        for (int a = UnityEngine.Random.Range(-4, 3); a < 4; a += 0)
        {
            for (int b = UnityEngine.Random.Range(-4, 3); b < 4; b += 0)
            {
                for (int c = UnityEngine.Random.Range(-4, 3); c < 4; c += 0)
                {
                    for (int d = UnityEngine.Random.Range(-4, 3); d < 4; d += 0)
                    {
                        counter++;
                        d++;
                        if (d > 3)
                        {
                            d = -4;
                            c++;
                            if (c > 3)
                            {
                                c = -4;
                                b++;
                                if (b > 3)
                                {
                                    b = -4;
                                    a++;
                                    if (a > 3)
                                    {
                                        a = -4;
                                    }
                                }
                            }
                        }
                        if (counter > 4096)
                        {
                            checkingforcheck = false;
                            PieceArray = PieceArrayCopy;
                            bestmovevalue = -1000;
                            currentmovevalue = -1000;
                            return bestmove;
                        }
                        if (IsMoveLegal(-(b - 3), a + 4, -(d - 3), c + 4) == true)
                        {
                            currentmovevalue = 0;
                            if (PieceArray[-(d - 3), c + 4] == 0)
                            {
                                currentmovevalue = currentmovevalue + 0;
                            }
                            if (PieceArray[-(d - 3), c + 4] == 1)
                            {
                                currentmovevalue = currentmovevalue + 100;
                            }
                            if (PieceArray[-(d - 3), c + 4] == 2)
                            {
                                currentmovevalue = currentmovevalue + 9;
                            }
                            if (PieceArray[-(d - 3), c + 4] == 3)
                            {
                                currentmovevalue = currentmovevalue + 5;
                            }
                            if (PieceArray[-(d - 3), c + 4] == 4)
                            {
                                currentmovevalue = currentmovevalue + 3;
                            }
                            if (PieceArray[-(d - 3), c + 4] == 5)
                            {
                                currentmovevalue = currentmovevalue + 3;
                            }
                            if (PieceArray[-(d - 3), c + 4] == 6)
                            {
                                currentmovevalue = currentmovevalue + 2;
                            }
                            if ((-(d - 3) == 3 || -(d - 3) == 4) && (c + 4 == 3 || c + 4 == 4))
                            {
                                currentmovevalue = currentmovevalue + 1;
                            }
                            if (move < 10 && PieceArray[-(b - 3), a + 4] == 7)
                            {
                                currentmovevalue = currentmovevalue - 2;
                            }
                            if (move < 10 && PieceArray[-(b - 3), a + 4] == 12 && -(b - 3) != 1)
                            {
                                currentmovevalue = currentmovevalue - 1;
                            }
                            if (PieceArray[-(b - 3), a + 4] == 7)
                            {
                                currentmovevalue = currentmovevalue - 1;
                            }
                            for (int a2 = -3; a2 < 4; a2++)
                            {
                                for (int b2 = -3; b2 < 4; b2++)
                                {
                                    if (IsMoveLegal(-(d - 3), c + 4, -(b2 - 3), a2 + 4) == true && PieceArray[a2, b2] == 1)
                                    {
                                        currentmovevalue = currentmovevalue + 2;
                                    }
                                }
                            }
                            move++;
                            int savepiecevalue = PieceArray[-(d - 3), c + 4];
                            PieceArray[-(d - 3), c + 4] = PieceArray[-(b - 3), a + 4];
                            PieceArray[-(b - 3), a + 4] = 0;
                            int checkingworstcase = 0;
                            int worstcasescenario = 0;
                            int counter2 = 0;
                            for (int e = -4; e < 4; e++)
                            {
                                for (int f = -4; f < 4; f++)
                                {
                                    for (int g = -4; g < 4; g++)
                                    {
                                        for (int h = -4; h < 4; h++)
                                        {
                                            if (IsMoveLegal(-(f - 3), e + 4, -(h - 3), g + 4) == true)
                                            {
                                                counter2++;
                                                if (PieceArray[-(h - 3), g + 4] == 0)
                                                {
                                                    checkingworstcase = 0;
                                                }
                                                if (PieceArray[-(h - 3), g + 4] == 7)
                                                {
                                                    checkingworstcase = 1000;
                                                }
                                                if (PieceArray[-(h - 3), g + 4] == 8)
                                                {
                                                    checkingworstcase = 9;
                                                }
                                                if (PieceArray[-(h - 3), g + 4] == 9)
                                                {
                                                    checkingworstcase = 5;
                                                }
                                                if (PieceArray[-(h - 3), g + 4] == 10)
                                                {
                                                    checkingworstcase = 3;
                                                }
                                                if (PieceArray[-(h - 3), g + 4] == 11)
                                                {
                                                    checkingworstcase = 3;
                                                }
                                                if (PieceArray[-(h - 3), g + 4] == 12)
                                                {
                                                    checkingworstcase = 1;
                                                }
                                                for (int c2 = -3; c2 < 4; c2++)
                                                {
                                                    for (int d2 = -3; d2 < 4; d2++)
                                                    {
                                                        if (IsMoveLegal(-(h - 3), g + 4, -(d2 - 3), c2 + 4) == true && PieceArray[c2, d2] == 1)
                                                        {
                                                            checkingworstcase = checkingworstcase + 1;
                                                        }
                                                    }
                                                }
                                                if (isBoardSame == PieceArray)
                                                {
                                                    checkingworstcase = checkingworstcase + 50;
                                                }
                                                if (checkingworstcase > worstcasescenario)
                                                {
                                                    worstcasescenario = checkingworstcase;
                                                }
                                                checkingworstcase = 0;
                                            }
                                        }
                                    }
                                }
                            }
                            if (counter2 == 0)
                            {
                                worstcasescenario = 100;
                            }
                            if (counter2 < 6)
                            {
                                worstcasescenario = worstcasescenario - Mathf.FloorToInt((6 - counter2) / 2);
                            }
                            counter2 = 0;
                            currentmovevalue = currentmovevalue - worstcasescenario;
                            PieceArray[-(b - 3), a + 4] = PieceArray[-(d - 3), c + 4];
                            PieceArray[-(d - 3), c + 4] = savepiecevalue;
                            move--;
                            if (currentmovevalue > bestmovevalue)
                            {
                                bestmovevalue = currentmovevalue;
                                bestmove = new int[] {a, b, c, d};
                            }
                            currentmovevalue = -1000;
                        }
                    }
                }
            }
        }
        checkingforcheck = false;
        PieceArray = PieceArrayCopy;
        bestmovevalue = -1000;
        currentmovevalue = -1000;
        return bestmove;
    }
    public void Reset()
    {
        firstspawn = true;
        //Defining size of PieceArray
        PieceArray = new int[collums, rows];
        //Loading array with starting chess positions
        PieceArray = new int[,]
        {
            {9, 11, 10, 8, 7, 10, 11, 9},
            {12, 12, 12, 12, 12, 12, 12, 12},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {6, 6, 6, 6, 6, 6, 6, 6},
            {3, 5, 4, 2, 1, 4, 5, 3}
        };
        for (int x = 0; x < collums; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                SpawnPieces(x, y, PieceArray[x, y]);
            }
        }
        firstspawn = false;
    }
    public void Menu()
    {
        menunotcomplete = true;
        GameObject g = new GameObject("Menu");
        g.transform.position = new Vector3(0, 0, -3);
        var s = g.AddComponent<SpriteRenderer>();
        s.sprite = menu;
    }
    public void GameOver()
    {
        GameObject g = new GameObject("Game_Over");
        g.transform.position = new Vector3(0, 0, -3);
        var s = g.AddComponent<SpriteRenderer>();
        s.sprite = game_over;
    }
}