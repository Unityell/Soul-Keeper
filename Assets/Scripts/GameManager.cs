using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate             void InputMessage(string Message);
    public static               event InputMessage Signal;

    public                      int FPS;
    public                      int Difficulty;
    public static               int GameScore;

    public                      float GameSpeed;
    public                      float LevelLenghtInSeconds;
    private                     float ShadowTimer;
    [SerializeField] private    int ScorePerSecond;

    private                     bool ChangeLevelMoment;
    private                     bool Check;
    [SerializeField]            bool DebugKillPrefs;

    [SerializeField]            private RectTransform Point;
    [SerializeField]            private HUD HUD;
    [SerializeField]            private VFXManager VFXManager;
    [SerializeField]            private AudioManager AudioManager;
    [SerializeField]            private InputManager InputManager;
    [SerializeField]            private Generation LandGenerator;
    [SerializeField]            private WheatherGen WeatherManager;
    [SerializeField]            private Vector3 PlayerStartPos;
    [SerializeField]            private Player Player;

    void Start()
    {
        HUD.Initialization();
        InputManager.Initialization();
        RefreshScoreScore();
        ShadowTimer = LevelLenghtInSeconds;
        Application.targetFrameRate = FPS;
    }
    public void EmitSignal(string NewMessage)
    {
        Signal?.Invoke(NewMessage);
    }

    public void IntoSaveHouse()
    {
        AudioListener.pause = true;
        LandGenerator.DestroyAllSpawnedObject();
        EmitSignal("RefreshScore");
        EmitSignal("Save");
        Time.timeScale = 0;
    }
    void RefreshScoreScore()
    {
        GameManager.GameScore += ScorePerSecond;
        EmitSignal("RefreshScore");
        Invoke(nameof(RefreshScoreScore), 1f);
    }
    
    public void Continue()
    {
        AudioListener.pause = AudioManager.CheckSoundState;
        Player.transform.position = PlayerStartPos;
        SpawnSaveHouse(true);
        LevelLenghtInSeconds += 20f;
        ShadowTimer = LevelLenghtInSeconds;
        GameSpeed += 1f;
        Difficulty += 1;
        Point.anchoredPosition = new Vector3(0, Point.anchoredPosition.y, 0);
        ChangeLevelMoment = false;
        EmitSignal("HidePannel");
        LandGenerator.StartCoroutine(LandGenerator.StartGame());
        Time.timeScale = 1;
    }

    void SpawnSaveHouse(bool Switch)
    {
        LandGenerator.MoveHouse(Switch);
    }

    void Update()
    {
        if(ShadowTimer > 0)
        {
            ShadowTimer -= Time.deltaTime;
            float MoveStep = 100 - ShadowTimer/(LevelLenghtInSeconds/100);
            Point.anchoredPosition = new Vector3(MoveStep, Point.anchoredPosition.y, 0);
        }
        else
        {
            if(!ChangeLevelMoment)
            {
                SpawnSaveHouse(false);
                ChangeLevelMoment = true;
            }
        }
    }
}
