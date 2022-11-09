using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool create=false;
    public static int EnemyCount=1;
    public int coins=0;
    public static bool stopPlatform=false;//variable that stores state of platform not the elevator
    public static bool playerAlive = true;
    GameObject Player;
    GameObject Animal;
    public GameObject[] Platform;
    public bool win = false;
    public static int level = 0;
    public static int devLevel = 0;
    public HUDController hud;
    public GameOverUI GameOver;
    public static GameManager instance;
    public float[] score = new float[2] { 0, 0 };
    public int gameState=1;
    Vector3[] OGWalls;
    public string[] wallHistory;
    bool started;
    public static int Inhibitors;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this)
        {
            instance.gameState = gameState;
            if (gameState == 1)//this apparently avoids erros form previous instantiations
            {
                instance.hud = FindObjectOfType<HUDController>();
                instance.GameOver = FindObjectOfType<GameOverUI>();
                instance.Platform = new GameObject[2] { GameObject.Find("MainPlatform"), GameObject.Find("MainPlatform (1)") };// the first is the current the other is the next
            }
            
            instance.Player = GameObject.Find("Player");
            instance.Animal = GameObject.Find("Animal");
            Destroy(gameObject);
        }
        //avoid destruction when changing scenes
        DontDestroyOnLoad(gameObject);
        if (instance.gameState == 1)//THESE ARE ALL THE VARIABLES THAT NEED TO RESET ONCE THE PLAYER RESPAWNS OR GOES HOME AND PLAYS AGAIN
        {
            instance.OGWalls = new Vector3[2] {GameObject.Find("Wall Level 00 (R)").transform.position,GameObject.Find("Wall Level 00 (L)").transform.position };//this function is to automate the position of the future walls based on the position of the defaulf walls which I can modify to my will
            instance.win = false;
            highScoreSet = false;
            instance.create=true;
            Player = GameObject.Find("Player");// we are missing a search for the game object every tme the player looses
            Animal = GameObject.Find("Animal");
            Platform = new GameObject[2] { GameObject.Find("MainPlatform"), GameObject.Find("MainPlatform (1)") };// the first is the current the other is the next
            instance.wallHistory = new string[6] { "000", "000", "000", "000", "000", "000" };
            hud = FindObjectOfType<HUDController>();
            GameOver = FindObjectOfType<GameOverUI>();
            playerAlive = true;
            instance.started = false;
            level = 0;
            instance.score = new float[2] { 0, 0 };//score[0] must be reset since the 101 is part of the funcitont hat gives the socre, score[1] must also reset becaus the previous level is 0
            instance.elapsed = 0;
            Inhibitors = 0;
            
        }
    }
    private void Start()
    {
        LoadGame();
    }
    void FixedUpdate()
    {
        if (gameState == 1)//checking the game state avoids any unwanted acitons in the home meny
        {
            if(level==0 && !started)
            {
                EnemySpawn();
                printingCash();//we are creating the coins that will appear in the first level
                messyRoom();
                changeHud();
                started = true;
            }
            NextLevel();
            instance.changeScore();
            SetHighScore();
        }

    }
    public static Vector3 PolToCart(float angle, float vector = 1, float y=0)//This is just a math funciton that may be used in other scripts to avoid doing the calculations
    {
        float radians = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(radians) * vector, y, Mathf.Sin(radians) * vector);
    }
    void EnemySpawn(float y= 3)
    {
        if (instance.create)
        {
            float e2Chance = (level == 4 ? 100.0f : (level > 4 ? 50.0f:0));
            float e3Chance = (level == 9 ? 100.0f : ((level > 9&&level<=12) ? 50.0f : (level>12?25.0f:0)));
            float e4Chance = (level == 15 ? 100.0f : ((level > 15 && level <= 18) ? 50.0f : (level > 18 ? 12.5f : 0)));
            EnemyCount = Random.Range(3,7);//between 3 and 6 enemies is the fun number
            int slotsLeft = EnemyCount;
            int Enemy4 = 1 *Chances(e4Chance);
            int e3Min = (level == 9 || level >= 12) ? 1 : ((level - 9) + 1);
            int e3Max = level < 12 ? ((level - 9) + 2) : slotsLeft;
            int Enemy3 = (Random.Range(e3Min, (e3Max>4?5:e3Max)))*Chances(e3Chance);//we first give the enemy3 the chance to spawn since it needs 2 or more to work well
                                                                                          //the slotsleft should have a -1 nevertheless the range is exlclusive on the max
            slotsLeft -= Enemy3;//then we update the slots left
            int e2Min = (level == 4 || level >= 8) ? 1 : ((level-4) + 1);
            int e2Max = level < 8 ? ((level-4) + 2) : slotsLeft;
            int Enemy2 = (slotsLeft >0 ? Random.Range(e2Min, (e2Max > 3 ? 4:(e2Max+1))) : 0) * Chances(e2Chance);//the enemy 2 uses the slots left as long as they are more than 0
                                                                                                                  //if there are more than 3 slots left it uses 4 as its max because the random function is exclusive on the max
            slotsLeft -= Enemy2;//update slots left
            int e1Min = (level == 0 || level >= 4) ? 1 : (level+2);
            int e1Max = level < 4 ? (level + 2) : slotsLeft;
            int Enemy1 = slotsLeft>0?Random.Range(e1Min, (e1Max+1)):0;//if there is one slot missing to fill the enemy1 will fill it with a random quantity between 1 and the actual numbers of slots left

            //ENEMY SPAWN STARTS, using the previously worked numbers the enemies start to spawn
            for (int i = 0; i < Enemy1; i++)
                Instantiate(Resources.Load<GameObject>("Enemy"), Platform[0].GetComponent<PlatformController>().ZoneInstantiation(30*level+3), Quaternion.identity);//new Vector3(Random.Range(8, -8), (30 * level + 3 ), Random.Range(8, -8))
            for (int h = 0; h < Enemy2; h++)
                Instantiate(Resources.Load<GameObject>("Enemy2"), Platform[0].GetComponent<PlatformController>().ZoneInstantiation((30 * level + 3), 1, 2), Quaternion.identity);
            for (int j = 0; j < Enemy3; j++)
            {
                Instantiate(Resources.Load<GameObject>("Enemy3"), Platform[0].GetComponent<PlatformController>().ZoneInstantiation((30 * level + 3),1,2), Quaternion.identity);
                Inhibitors++;//as enemy 3 has an inhibtion function we add the number so that the function can work
            }
            for (int h = 0; h < Enemy4; h++)
                Instantiate(Resources.Load<GameObject>("Enemy4"), Platform[0].GetComponent<PlatformController>().ZoneInstantiation((30 * level + 3), 1, 2), Quaternion.identity);
            EnemyCount = Enemy3+Enemy2+Enemy1+Enemy4;//in case there was any 0s in the previous random numbers for enemies 1,2 & 3 we update here the enemy count to the real enemy count
        }
    }
    void NextLevel()
    {
        
        if (EnemyCount == 0 && !win && Animal.GetComponent<AnimalController>().safe)
        {
            
            level++;
            stopPlatform = true;//this Stop Platoform is not for the elevator platform but rather to stop the actual platform from schirking as long as the player is changin level
            Platform[0] = Platform[1];
            Platform[1]=Instantiate(Resources.Load<GameObject>("MainPlatform"), new Vector3(0,30*level-1+30, 0), Quaternion.identity);//creates the next platform and saves it as the next platform
            Instantiate(Resources.Load<GameObject>("Elevator"), new Vector3(Player.transform.position.x, (30*level-30.5f), Player.transform.position.z), Quaternion.identity);//elevator
            Quaternion rotation = Quaternion.Euler(0, 180, 0);
            Instantiate(Resources.Load<GameObject>("LeftWall"), new Vector3(instance.OGWalls[1].x, (30*level+30)+instance.OGWalls[1].y,instance.OGWalls[1].z), rotation);//left wall
            
            rotation = Quaternion.Euler(0, -90, 0);
            Instantiate(Resources.Load<GameObject>("RightWall"), new Vector3(instance.OGWalls[0].x, (30 * level+30) + instance.OGWalls[0].y, instance.OGWalls[0].z), rotation);//right wall
            instance.score[1] += instance.score[0];
            instance.score[1] = instance.score[1] < 0 ? 0 : instance.score[1];//since the score[0] has been added to score[1], if the player took too long score[0] could make score[1] negaitve
                                                                              // therefore we will check for the negatives and make them 0 in case they appear
            elapsed = 0;// the X for the score funtion resests to keep the funtion wrking properly
            EnemySpawn(33);//we spawn the enmies of the next level
            printingCash(); // we are creating the coins that will be in the next level
            messyRoom();
            win = true;//this prompts the elevator to move, I think
            //instance.changeHud();
            
        }

    }
    public void changeHud()//just a function to access the hud from the game manager
    {
        hud.ResetHUD();
    }
    public void CoinGained(int quant)
    {
        coins += quant;
        SaveGame();
        print("COINS:" + GameManager.instance.coins);
        hud.NotifyNewCryptos(quant);
        hud.ResetHUD();
    }
    float elapsed=0;
    public void changeScore()
    {
        if (!win && playerAlive)//as long as the player hasn't won and he is still alive the score will remain changing
        {
            instance.score[0] = 103- Mathf.Pow(1.12f, Time.deltaTime+instance.elapsed+10);//parting from 101, here we'll reduce the points for each level
            float printedScore = instance.score[1] + instance.score[0];//the printed score will be the previous level score plus this new one, which could be negative if the player takes too long
            printedScore = printedScore >= 0 ? printedScore : 0;//we check if the printed, not the level score, is negative, in that case, it will just stay at 0
            instance.elapsed += Time.deltaTime;//this is the X for the funtion that reduces from the 101
            hud.ResetScore(Mathf.Round(printedScore)); //and we pass the quantity to the script that operates the HUD
        }
    }
    public int highScore = 0;
    public int highestLevel = 0;
    public static bool highScoreSet = false;
    public void SetHighScore()//this funciton here is responsible for bringing up the GameOverUI
    {
        if (!playerAlive && !highScoreSet)//if player dies there will be a high score evaluation, but just once
        {
            float HS = instance.score[1] + instance.score[0];//this is a variable used to avoid typing over and over instace.score
            if(Mathf.RoundToInt(HS) > highScore)//checks if new score is greater than previous high score
                instance.highScore = Mathf.RoundToInt(HS);//if so changes the high score
            if (level > highestLevel)//cheks if the level reached is the new level high
                highestLevel = level;
            highScoreSet = true;//this completes the highscore evaluation so the script will know that it won't need to run it again
            SaveGame();
            GameOver.CheckGameOver(highScore, highestLevel);//this passes the data to the scipt that handles the game over UI
        }
            
    }
    public void SaveGame()
    {
        SaveSystem.saveGameManager(this);
    }
    public void LoadGame()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        highestLevel = data.highestLevel;
        highScore = data.highScore;
        coins = data.coins;
        //print(coins);
    }
    public int Chances(float percent)
    {
        int result = 1;
        if (percent == 0)
        {
            return 0;
        }
        else if (50 % percent != 0)
            print("50 must be divisible by percent"); // the function only works for multiples of 50% (25%, 12.5%, etc...)
        else if (percent!=100)
        {
            for (int x = 0; x < (50 / percent); x++)//the loop makes sure to repeat the "coin toss" the indicated number of times
                                                    //50% would be 1 time; 25% would be 2 times; 12.5% would be 4 times; etc...
            {
                result *= Random.Range(0, 2);//this range either gives 1 or 0
            }
        }
        return result;
    }
    public void printingCash()
    {
        int coinQuant = Random.Range(0, 3);//we are generating a random number between 0 and 3 which is the number of coins that will appear
        //print(coinQuant);
        for(int x=0; x < coinQuant; x++)
        {
            Instantiate(Resources.Load<GameObject>("Coin"), Platform[0].GetComponent<PlatformController>().ZoneInstantiation((30 * level + 3), 4, 2), Quaternion.identity); //we are creating each coin
        }
    }
    public void messyRoom()
    {
        int boxQuant = Random.Range(1, 2);//we reuse the variable to create the box Groups
        for (int x = 0; x < boxQuant; x++)
        {
            Instantiate(Resources.Load<GameObject>("Boxes/BoxGroup" + Random.Range(1, 3)), Platform[0].GetComponent<PlatformController>().ZoneInstantiation((30 * level + 1.5f), 4, 2), Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0)); //we are creating each group of boxes
        }
        boxQuant = Random.Range(2,4);//we are generating a random number between 0 and 3 which is the number of coins that will appear
        for (int x = 0; x < boxQuant; x++)
        {
            int typeNum = Random.Range(1, 5);
            Instantiate(Resources.Load<GameObject>("Boxes/Box"+typeNum), Platform[0].GetComponent<PlatformController>().ZoneInstantiation((30 * level + 1.5f),(typeNum>2?4:3), (typeNum > 1 ? 2 : 1)), Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0)); //we are creating each box
        }
    }
    public void ImplodeDeathStar()//Developer should use this as he may see fit, although user should be warned this is extremely dangerous for his progress
    {
        print("BEFORE- HS: " + highScore + "; HL: " + highestLevel+"; C: "+coins);
        highestLevel = 0;//we reset each variable that will be stored
        highScore = 0;
        coins = 0;
        SaveGame();//then we save the reseted values
        LoadGame();//and we immediatly load those values into the game
        print("AFTER- HS: " + highScore + "; HL: " + highestLevel + "; C: " + coins);//now the values should all be 0
    }
}
