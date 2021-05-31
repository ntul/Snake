using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class Data : ScriptableObject
{
    [Header("Defaults")]
    [SerializeField]
    private float defaultMoveSpeed;
    [SerializeField]
    private float defaultTurnSpeed;


    [Header("Snake")]
    public float moveSpeed;
    public float turnSpeed;
    public float feverSpeedMultiplier;
    

    [Header("Track")]
    public float trackLength;
    private int gatesCrossed;
    public float obstaclesOffset;
    public int winGates;

    [Header("Dudes")]
    public float dudesMinPos;
    public float dudesMaxPos;
    public float dudesXoffset;
    public float dudesYoffset;

    
    [Header("Scores")]
    [SerializeField]
    private int dudeScore;
    private List<Color> colors;
    [SerializeField]
    private int diamondScore;



    [Header("Materials")]
    public Material gateMat;
    public Material snakeMat;
    public Material dudeMat;



    [Header("Bools")]
    [SerializeField]
    private bool isFever;
    [SerializeField]
    private bool isGameOver;


    public Queue<GameObject> tracks;
    public Queue<GameObject> dudes;
    public Queue<GameObject> obstacles;

    public bool IsFever
    {
        get { return isFever; }

        set
        {
            if (isFever == value) return;
            if (OnFever != null)
            {
                isFever = value;
                OnFever(value);
            }
        }
    }
  
    public List<Color> Colors
    {
        get 
        {
            if (colors.Count == 0) colors = new List<Color>(baseColors);
            return colors;
        }
    }

    public int DudeScore
    {
        get { return dudeScore; }

        set
        {
            if (dudeScore == value) return;
            if (OnDudeEaten != null)
            {
                dudeScore = value;
                OnDudeEaten(value);
            }
        }
    }

    public int DiamondScore
    {
        get { return diamondScore; }

        set
        {
            if (diamondScore == value) return;
            if (OnDiamondEaten != null)
            {
                diamondScore = value;
                OnDiamondEaten(value);

            }
        }
    }
    public int GatesCrossed
    {
        get { return gatesCrossed; }

        set
        {
            if (gatesCrossed == value) return;
            if (OnGateCross != null)
            {
                gatesCrossed = value;
                OnGateCross.Invoke();
            }
        }
    }
    
    public bool IsGameOver
    {
        get { return isGameOver; }

        set
        {
            //if (isGameOver == value) return;
            if (OnGameOver != null)
            {
                isGameOver = value;
                if (value) OnGameOver.Invoke();
                else OnRestart.Invoke();
            }
        }
    }

    public List<Color> baseColors;

    public delegate void Event();
    public delegate void EventInt(int val);
    public delegate void EventBool(bool val);
    public event Event OnGameOver;
    public event Event OnRestart;
    public event Event OnGateCross;
    public event EventInt OnDudeEaten;
    public event EventInt OnDiamondEaten;
    public event Event OnLoaderTrigger;
    public event EventBool OnFever;

    private void OnEnable()
    {
        InitData();
    }

    public void InitData()
    {
        isGameOver = false;
        tracks = new Queue<GameObject>();
        dudes = new Queue<GameObject>();
        obstacles = new Queue<GameObject>();
        gatesCrossed = 0;
        colors = new List<Color>(baseColors);
        dudeScore = 0;
        diamondScore = 0;
        snakeMat.SetFloat("_Pos", 0f);
        isFever = false;
        moveSpeed = defaultMoveSpeed;
        turnSpeed = defaultTurnSpeed;
    }

    public void LoaderTrigger()
    {
        OnLoaderTrigger.Invoke();
    }

}
