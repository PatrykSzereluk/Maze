
using UnityEngine;

public class GameManager : MonoBehaviour {

    [Header("Objects")]
    public GameObject Hero;
    public GameObject Wall;
    public GameObject Floor;
    public GameObject Door;
    public GameObject Key;
    public GameObject cam;

    public Transform startPoint;

    [Header("Map Options")]
    public int mapSizeX = 36;
    public int mapSizeY = 36;
    public int minLineToKey;

    [Header("Parents")]
    public Transform walls;
    public Transform floors;

    [Header("Camera Options")]
    public float cameraSpeed;
    

    private enum Dirs
    {
        None,
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3
    }

    private  enum ObjectsToBuild
    {
        Wall=0,
        Floor,
        Key,
        Door
    }

    void Start()
    {
        CreateAWorld();
    }

    private void CreateAWorld()
    {

        int[,] levelToArray = new int[mapSizeX, mapSizeY];

        ResetTheArray(levelToArray);

        FindARoad(levelToArray);

       GameObject hero = InstantiateHero();

        InstantiateCamera(hero);

        GenerateLevel(levelToArray);
    }

    private void FindARoad(int[,] levelToArray)
    {

        int previousMove = (int)Dirs.None;

        int move = 0;

        // Current possition x in array
        int posX = 1;
        // Current possition y in array
        int posY = 1;

        int nextMove;

        while (move < minLineToKey)
        {
            if (move == 0)
            {
                levelToArray[1, 1] = (int)ObjectsToBuild.Floor;
                move++;
                continue;
            }

            nextMove = Random.Range((int)Dirs.Up, (int)Dirs.Right + 1);

            if(nextMove != previousMove)
            {
                Debug.Log(nextMove);
                switch(nextMove)
                {
                    case (int)Dirs.Up:
                        {
                            if(posY < mapSizeY -2 && levelToArray[posX,posY+1] == (int)ObjectsToBuild.Wall)
                            {
                                posY++;
                                if ((levelToArray[posX + 1, posY] == (int)ObjectsToBuild.Floor || posX + 1 >= mapSizeX - 2)
                                                                    && (levelToArray[posX - 1, posY] == (int)ObjectsToBuild.Floor || posX - 1 <= 1)
                                                                    && (levelToArray[posX, posY + 1] == (int)ObjectsToBuild.Floor || posY + 1 >= mapSizeY - 2)
                                                                    && (levelToArray[posX, posY - 1] == (int)ObjectsToBuild.Floor || posY - 1 <= 1))
                                {
                                    posY--;
                                }
                                {
                                    levelToArray[posX, posY] = (int)ObjectsToBuild.Floor; move++;
                                }
                            }
                            break;
                        }
                    case (int)Dirs.Down:
                        {
                            if (posY > 1 && levelToArray[posX, posY - 1] == (int)ObjectsToBuild.Wall)
                            {
                                posY--;
                                if ((levelToArray[posX + 1, posY] == (int)ObjectsToBuild.Floor || posX + 1 >= mapSizeX - 2)
                                                                    && (levelToArray[posX - 1, posY] == (int)ObjectsToBuild.Floor || posX - 1 <= 1)
                                                                    && (levelToArray[posX, posY + 1] == (int)ObjectsToBuild.Floor || posY + 1 >= mapSizeY - 2)
                                                                    && (levelToArray[posX, posY - 1] == (int)ObjectsToBuild.Floor || posY - 1 <= 1))
                                {
                                    posY++;
                                }
                                {
                                    levelToArray[posX, posY] = (int)ObjectsToBuild.Floor; move++;
                                }

                            }
                            break;
                        }
                    case (int)Dirs.Right:
                        {
                            if (posX < mapSizeX - 2 && levelToArray[posX + 1, posY] == (int)ObjectsToBuild.Wall)
                            {
                                posX++;
                                if ((levelToArray[posX + 1, posY] == (int)ObjectsToBuild.Floor || posX + 1 >= mapSizeX - 2)
                                                                    && (levelToArray[posX - 1, posY] == (int)ObjectsToBuild.Floor || posX - 1 <= 1)
                                                                    && (levelToArray[posX, posY + 1] == (int)ObjectsToBuild.Floor || posY + 1 >= mapSizeY - 2)
                                                                    && (levelToArray[posX, posY - 1] == (int)ObjectsToBuild.Floor || posY - 1 <= 1))
                                {
                                    posX--;
                                }
                                {
                                    levelToArray[posX, posY] = (int)ObjectsToBuild.Floor; move++;
                                }

                            }
                            break;
                        }
                    case (int)Dirs.Left:
                        {
                            if (posX > 1 && levelToArray[posX - 1, posY] == (int)ObjectsToBuild.Wall)
                            {
                                posX--;
                                if ((levelToArray[posX + 1, posY] == (int)ObjectsToBuild.Floor || posX + 1 >= mapSizeX - 2)
                                                                    && (levelToArray[posX - 1, posY] == (int)ObjectsToBuild.Floor || posX - 1 <= 1)
                                                                    && (levelToArray[posX, posY + 1] == (int)ObjectsToBuild.Floor || posY + 1 >= mapSizeY - 2)
                                                                    && (levelToArray[posX, posY - 1] == (int)ObjectsToBuild.Floor || posY - 1 <= 1))
                                {
                                    posX++;
                                }
                                {
                                    levelToArray[posX, posY] = (int)ObjectsToBuild.Floor;
                                    move++;
                                }

                            }
                            break;
                        }
                    default:
                        {

                            continue;
                        }
                }
               
            }

            previousMove = nextMove;

        }
        

    }

    /// <summary>
    /// Function generate level based on array send in arguments
    /// </summary>
    /// <param name="levelToArray"></param>
    private void GenerateLevel(int[,] levelToArray)
    {
        for (int i = 0; i < mapSizeX; i++)
        {
            for (int j = 0; j < mapSizeY; j++)
            {
                if (levelToArray[i, j] == (int) ObjectsToBuild.Wall)
                {
                    Instantiate(Wall, startPoint.position + new Vector3(i, j, 0), Quaternion.identity, walls);
                }
                else if (levelToArray[i, j] == (int)ObjectsToBuild.Floor)
                {
                    Instantiate(Floor, startPoint.position + new Vector3(i, j, 0), Quaternion.identity, floors);
                }
                else if (levelToArray[i, j] == (int)ObjectsToBuild.Key)
                {
                    Instantiate(Key, startPoint.position + new Vector3(i, j, 0), Quaternion.identity);
                }
                else if (levelToArray[i, j] == (int)ObjectsToBuild.Door)
                {
                    Instantiate(Door, startPoint.position + new Vector3(i, j, 0), Quaternion.identity);
                }
            }
        }
    }

    
    private void ResetTheArray(int[,] levelToArray)
    {
        for (int i = 0; i < mapSizeX; i++)
        {
            for (int j = 0; j < mapSizeY; j++)
            {
                levelToArray[i, j] = 0;
            }
        }
    }

    private void InstantiateCamera(GameObject hero)
    {
        GameObject g = Instantiate(cam, startPoint.position + new Vector3(1, 1, -10), Quaternion.identity);
        g.GetComponent<CameraBehaviour>().Target = hero.transform;
    }

    private GameObject InstantiateHero()
    {
        return Instantiate(Hero, startPoint.position + new Vector3(1, 1, 0), Quaternion.identity);
    }




































    //// case (int) Dirs.Down:
    ////                    {
    ////    if (posY > 1 && levelToArray[posX, posY - 1] == (int)ObjectsToBuild.Wall)
    ////    {
    ////        posY--;
    ////        levelToArray[posX, posY] = (int)ObjectsToBuild.Floor;

    ////    }
    ////    break;
    ////}
    ////                case (int) Dirs.Right:
    ////                    {
    ////    if (posX < mapSizeX - 2 && levelToArray[posX + 1, posY] == (int)ObjectsToBuild.Wall)
    ////    {
    ////        posX++;
    ////        levelToArray[posX, posY] = (int)ObjectsToBuild.Floor;

    ////    }
    ////    break;
    ////}
    ////                case (int) Dirs.Left:
    ////                    {
    ////    if (posX > 1 && levelToArray[posX - 1, posY] == (int)ObjectsToBuild.Wall)
    ////    {
    ////        posX--;
    ////        levelToArray[posX, posY] = (int)ObjectsToBuild.Floor;

    ////    }
    ////    break;
    ////}
    ////                default:
    ////                    {

    ////                        continue;
    ////                    }













    //#region


    //private void GenerateGame()
    //{
    //    int[,] tab = new int[x, y];

    //    GenerateWorld(tab);

    //    GenerateObject(2, tab, 25, 34, 2, 26);
    //    GenerateObject(3, tab, 10, 34, 10, 34);

    //    CreateWorld(tab);

    //    GameObject hero = InstantiateHero();

    //    InstantiateCamera(hero);

    //}



    //private void GenerateWorld(int[,] tab)
    //{
    //    for (int i = 0; i < x; i++)
    //    {
    //        for (int j = 0; j < y; j++)
    //        {
    //            tab[i, j] = 0;
    //        }
    //    }

    //    tab[1, 1] = 1;

    //    int xx = 1;
    //    int yy = 1;
    //    int move = 0;

    //    for (int i = 0; i < 29; i++)
    //    {
    //        SetXXYY(ref xx, ref yy, i);

    //        move = 0;

    //        while (move < 80)
    //        {
    //            int rand = Random.Range(0, 4);
    //            // 0-Up,1-Down,2-Left,3-Right

    //            move++;

    //            if (rand == 0)
    //            {
    //                if (yy + 1 > y - 2) { continue; }
    //                else if (tab[xx, yy + 1] == 2)
    //                {
    //                    break;
    //                }
    //                else
    //                {
    //                    yy++;
    //                    tab[xx, yy] = 1;
    //                }
    //            }
    //            else if (rand == 1)
    //            {
    //                if (yy - 1 < 1) { continue; }
    //                else if (tab[xx, yy - 1] == 2)
    //                {
    //                    break;
    //                }
    //                else
    //                {
    //                    yy--;
    //                    tab[xx, yy] = 1;
    //                }
    //            }
    //            else if (rand == 2)
    //            {
    //                if (xx - 1 < 1) { continue; }
    //                else if (tab[xx - 1, yy] == 2)
    //                {
    //                    break;
    //                }
    //                else
    //                {
    //                    xx--;
    //                    tab[xx, yy] = 1;
    //                }
    //            }
    //            else if (rand == 3)
    //            {
    //                if (xx + 1 > x - 2) { continue; }
    //                else if (tab[xx + 1, yy] == 2)
    //                {
    //                    break;
    //                }
    //                else
    //                {
    //                    xx++;
    //                    tab[xx, yy] = 1;
    //                }
    //            }
    //        }

    //    }
    //}

    //private static void SetXXYY(ref int xx, ref int yy, int i)
    //{
    //    if (i == 1) { xx = 5; yy = 10; }
    //    else if (i == 2) { xx = 5; yy = 16; }
    //    else if (i == 3) { xx = 5; yy = 22; }
    //    else if (i == 4) { xx = 5; yy = 30; }
    //    else if (i == 5) { xx = 12; yy = 10; }
    //    else if (i == 6) { xx = 12; yy = 16; }
    //    else if (i == 7) { xx = 12; yy = 22; }
    //    else if (i == 8) { xx = 12; yy = 30; }
    //    else if (i == 9) { xx = 18; yy = 10; }
    //    else if (i == 10) { xx = 18; yy = 16; }
    //    else if (i == 11) { xx = 18; yy = 22; }
    //    else if (i == 12) { xx = 18; yy = 20; }
    //    else if (i == 13) { xx = 24; yy = 10; }
    //    else if (i == 14) { xx = 24; yy = 16; }
    //    else if (i == 15) { xx = 24; yy = 22; }
    //    else if (i == 16) { xx = 24; yy = 30; }
    //    else if (i == 17) { xx = 30; yy = 10; }
    //    else if (i == 18) { xx = 30; yy = 16; }
    //    else if (i == 19) { xx = 30; yy = 22; }
    //    else if (i == 20) { xx = 30; yy = 30; }
    //    else if (i == 21) { xx = 20; yy = 10; }
    //    else if (i == 22) { xx = 20; yy = 16; }
    //    else if (i == 23) { xx = 20; yy = 22; }
    //    else if (i == 24) { xx = 20; yy = 30; }
    //    else if (i == 25) { xx = 5; yy = 5; }
    //    else if (i == 26) { xx = 10; yy = 27; }
    //    else if (i == 27) { xx = 11; yy = 17; }
    //    else if (i == 28) { xx = 5; yy = 5; }
    //}

    //private void CreateWorld(int[,] tab)
    //{
    //    //for (int i = 0; i < x; i++)
    //    //{
    //    //    for (int j = 0; j < y; j++)
    //    //    {
    //    //        if (tab[i, j] == 0)
    //    //        {
    //    //            Instantiate(Wall, startPoint.position + new Vector3(i, j, 0), Quaternion.identity, walls);
    //    //        }
    //    //        else if (tab[i, j] == 1)
    //    //        {
    //    //            Instantiate(Floor, startPoint.position + new Vector3(i, j, 0), Quaternion.identity, floors);
    //    //        }
    //    //        else if (tab[i, j] == 2)
    //    //        {
    //    //            Instantiate(Key, startPoint.position + new Vector3(i, j, 0), Quaternion.identity);
    //    //        }
    //    //        else if (tab[i, j] == 3)
    //    //        {
    //    //            Instantiate(Door, startPoint.position + new Vector3(i, j, 0), Quaternion.identity);
    //    //        }
    //    //    }
    //    //}
    //}

    //private void GenerateObject(int ob, int[,] tab, int minX, int maxX, int minY, int maxY)
    //{
    //    bool place = true;

    //    do
    //    {
    //        int randx = Random.Range(minX, maxX);
    //        int randy = Random.Range(minY, maxY);



    //        if (tab[randx, randy] == 1)
    //        {
    //            place = false;
    //            tab[randx, randy] = ob;
    //        }

    //        if (!place && ob == 2)
    //        {
    //            Instantiate(Floor, startPoint.position + new Vector3(randx, randy, 0), Quaternion.identity, floors);
    //        }

    //    } while (place);
    //}

    //#endregion






}
