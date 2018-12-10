
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
    public int maxMoves;
    public int minMovesToKey;

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
        // Tworzymy tablice
        int[,] levelToArray = new int[mapSizeX, mapSizeY];
        // Wypełniamy ją 0 
        ResetTheArray(levelToArray);
        // Tworzymy przestrzeń, po której można chodzić oraz wstawiamy klucz
        FindARoad(levelToArray);
        // Tworzymy bohatera
        GameObject hero = InstantiateHero();
        // Tworzymy kamerę
        InstantiateCamera(hero);
        // Tworzymy drzwi
        PlaceDoor(levelToArray);
        // Na podstawie tabeli generujemy mapę
        GenerateLevel(levelToArray);
        // sprawdzamy ile obiektów typu floor  jest na  mapie
        CheckFloors(levelToArray);

    }

    private void CheckFloors(int[,] levelToArray)
    {
        int ile = 0;
        for (int i = 0; i < mapSizeX; i++)
        {
            for (int j = 0; j < mapSizeY; j++)
            {
                if (levelToArray[i, j] == 1)
                {
                    ile++;
                }
            }
        }
        Debug.LogWarning(ile);
    }

    private void FindARoad(int[,] levelToArray)
    {
        int previousMove = (int)Dirs.None;

        int move = 0;

        int keyPositionX = 0;
        int keyPositionY = 0;

        // Current possition x in array
        int posX = 1;
        // Current possition y in array
        int posY = 1;

        int nextMove;

        while (move < maxMoves)
        {
            // Ustanowienie punktu startowego
            if (move == 0)
            {
                levelToArray[1, 1] = (int)ObjectsToBuild.Floor;
                move++;
                continue;
            }

            nextMove = Random.Range((int)Dirs.Up, (int)Dirs.Right + 1);

            // jesli nastepny ruch nie był poprzednim ruchem
            if(nextMove != previousMove)
            {
                switch(nextMove)
                {
                    case (int)Dirs.Up:
                        {   // Idziemy w danym kierunku jeśli nie wyszliśmy za mapę oraz stoi tam ściana
                            if(posY < mapSizeY -2 && levelToArray[posX,posY+1] == (int)ObjectsToBuild.Wall)
                            {
                                    posY++;
                                    levelToArray[posX, posY] = (int)ObjectsToBuild.Floor;
                                    move++;
                            }
                            else if(CheckWalls(levelToArray,posX,posY)) // Jesli się zakleszczymy
                            {
                                //Tymczasowa zmienna, ponieważ nie zawsze znajdziemy odpowiednie miejsce w danym kierunku
                                int tmpPosY = posY;
                                // Szukamy najbliszego punktu w danym kierunku, gdzie możemy postawić ściane
                                while(tmpPosY < mapSizeY - 2 && levelToArray[posX,tmpPosY] == (int)ObjectsToBuild.Floor)
                                {
                                    tmpPosY++;
                                }//jeśli to nie jest koniec mapy oraz nie jest postawiona podłoga na krawędzi
                                if (levelToArray[posX, tmpPosY] != (int)ObjectsToBuild.Floor && tmpPosY < mapSizeY - 2)
                                {
                                    posY = tmpPosY;
                                    levelToArray[posX, posY] = (int)ObjectsToBuild.Floor;
                                    move++;
                                }
                            }
                            break;
                        }
                    case (int)Dirs.Down:
                        {
                            if (posY > 1 && levelToArray[posX, posY - 1] == (int)ObjectsToBuild.Wall)
                            {
                                    posY--;
                                    levelToArray[posX, posY] = (int)ObjectsToBuild.Floor;
                                    move++;
                            }
                            else if (CheckWalls(levelToArray, posX, posY))
                            {
                                int tmpPosY = posY;
                                while (tmpPosY > 1 && levelToArray[posX, tmpPosY] == (int)ObjectsToBuild.Floor)
                                {
                                    tmpPosY--;
                                }
                                if (levelToArray[posX, tmpPosY] != (int)ObjectsToBuild.Floor && tmpPosY > 1)
                                {
                                    posY = tmpPosY;
                                    levelToArray[posX, posY] = (int)ObjectsToBuild.Floor;
                                    move++;
                                }
                            }
                            break;
                        }
                    case (int)Dirs.Right:
                        {
                            if (posX < mapSizeX - 2 && levelToArray[posX + 1, posY] == (int)ObjectsToBuild.Wall)
                            {
                                    posX++;
                                    levelToArray[posX, posY] = (int)ObjectsToBuild.Floor;
                                    move++;
                            }
                            else if (CheckWalls(levelToArray, posX, posY))
                            {
                                int tmpPosX = posX;
                                while (tmpPosX < mapSizeX - 2 && levelToArray[tmpPosX, posY] == (int)ObjectsToBuild.Floor)
                                {
                                    tmpPosX++;
                                }
                                if (levelToArray[tmpPosX, posY] != (int)ObjectsToBuild.Floor && posX < mapSizeX - 2)
                                {
                                    posX = tmpPosX;
                                    levelToArray[posX, posY] = (int)ObjectsToBuild.Floor;
                                    move++;
                                }

                            }
                            break;
                        }
                    case (int)Dirs.Left:
                        {
                            if (posX > 1 && levelToArray[posX - 1, posY] == (int)ObjectsToBuild.Wall)
                            {
                                    posX--;
                                    levelToArray[posX, posY] = (int)ObjectsToBuild.Floor;
                                    move++;

                            }
                            else if (CheckWalls(levelToArray, posX, posY))
                            {
                                int tmpPosX = posX;
                                while (tmpPosX > 1 && levelToArray[tmpPosX, posY] == (int)ObjectsToBuild.Floor)
                                {
                                    tmpPosX--;
                                }
                                if (levelToArray[tmpPosX, posY] != (int)ObjectsToBuild.Floor && posX > 1)
                                {
                                    posX = tmpPosX;
                                    levelToArray[posX, posY] = (int)ObjectsToBuild.Floor;
                                    move++;
                                }
                            }
                            break;
                        }
                }
               
            }

            if(move == minMovesToKey)
            {
                keyPositionX = posX;
                keyPositionY = posY; 
            }

            previousMove = nextMove;

        }

        Instantiate(Key, startPoint.position + new Vector3(keyPositionX, keyPositionY, 0), Quaternion.identity);
    }

    private bool CheckWalls(int[,] levelToArray, int posX, int posY) 
    {

        // Warunek jest zabezpieczniem przed zwróceniem wyjątku w kolejnym warunku
        if (posY >= mapSizeY || posX >= mapSizeX || posX <= 0 || posY <= 0)
            return false;

        // Sprawdz czy dookoła są podłogi lub ściany graniczne
        return (levelToArray[posX + 1, posY] == (int)ObjectsToBuild.Floor || posX + 1 > mapSizeX - 2)
                && (levelToArray[posX - 1, posY] == (int)ObjectsToBuild.Floor || posX - 1 < 2)
                && (levelToArray[posX, posY + 1] == (int)ObjectsToBuild.Floor || posY + 1 > mapSizeY - 2)
                && (levelToArray[posX, posY - 1] == (int)ObjectsToBuild.Floor || posY - 1 < 2);
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

    private void FixMap(int[,] levelToArray)
    {
        for (int i = 0; i < mapSizeX; i++)
        {
            for (int j = 0; j < mapSizeY; j++)
            {
                if (i == 0 || j == 0 || i == mapSizeX - 1 || j == mapSizeY - 1)
                    levelToArray[i, j] = 0;
            }
        }
    }

    private void PlaceDoor(int[,] levelToArray)
    {
        bool hasNotDoor = true;
        int randPosX = 0;
        int randPosY = 0;
        do
        {
            randPosX = Random.Range(10, mapSizeX - 1);
            randPosY = Random.Range(10, mapSizeX - 1);

            if (levelToArray[randPosX, randPosY] != (int)ObjectsToBuild.Wall
                && levelToArray[randPosX, randPosY] != (int)ObjectsToBuild.Key
                && levelToArray[randPosX, randPosY] == (int)ObjectsToBuild.Floor)
            {
                levelToArray[randPosX, randPosY] = (int)ObjectsToBuild.Door;
                hasNotDoor = false;
            }

        } while (hasNotDoor);
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
