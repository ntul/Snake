using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public Data data;
    public GameObject trackPrefab;
    public GameObject track;
    public GameObject dudesPrefab;
    public GameObject obstaclesPrefab1;
    public GameObject obstaclesPrefab2;

    private float diamondsEaten = 0;
    private List<Coroutine> diamondCounters;

    private void Awake()
    {
        Screen.fullScreen = false;
        Application.targetFrameRate = 60;
        diamondCounters = new List<Coroutine>();
    }
    private void Start()
    {
        data.tracks.Enqueue(track);
        InitColors();
        SpawnDudes(0);
        CreateNextObstacles(0);
    }

    private void OnEnable()
    {
        data.OnGameOver += GameOver;
        data.OnRestart += Restart;
        data.OnGateCross += GateCross;
        data.OnLoaderTrigger += LoadNextSegment;
        data.OnFever += StartFever;
        data.OnDiamondEaten += DiamondEaten;
    }

    private void OnDisable()
    {
        data.OnGameOver -= GameOver;
        data.OnRestart -= Restart;
        data.OnGateCross -= GateCross;
        data.OnLoaderTrigger -= LoadNextSegment;
        data.OnFever -= StartFever;
        data.OnDiamondEaten -= DiamondEaten;
    }
    void GameOver()
    {
        Time.timeScale = 0;
        StartCoroutine(WaitForInput());
    }

    void LevelComplete()
    {
        StopAllCoroutines();
        Time.timeScale = 0;
        StartCoroutine(WaitForInput());
    }
    void GateCross()
    {
        if (data.GatesCrossed >= data.winGates)
        {
            LevelComplete();
            return;
        }
        StartCoroutine(DestroyLastTrack(1));
        StartCoroutine(DestroyLastObstacle(1));
        DespawnDudes();
    }

    void LoadNextSegment()
    {
        SetNextGateColor();
        CreateNextTrack();
        SpawnDudes(data.GatesCrossed + 1);
        CreateNextObstacles(data.GatesCrossed + 1);

    }

    void CreateNextTrack()
    {
        data.tracks.Enqueue(Instantiate(trackPrefab, Vector3.forward*(data.GatesCrossed+1) * data.trackLength, Quaternion.identity));
        data.snakeMat.SetColor("_Color2", data.snakeMat.GetColor("_Color"));
        data.snakeMat.SetColor("_Color", data.gateMat.color);
        data.snakeMat.SetFloat("_Pos", (data.GatesCrossed + 1) * data.trackLength); 
    }

    void CreateNextObstacles(int track)
    {
        data.obstacles.Enqueue(Instantiate(Random.value > 0.5f ? obstaclesPrefab1 : obstaclesPrefab2, Vector3.forward * (data.trackLength*(data.obstaclesOffset+track)), Quaternion.identity));
       

    }

    IEnumerator DestroyLastTrack(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Destroy(data.tracks.Dequeue());
    }

    IEnumerator DestroyLastObstacle(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Destroy(data.obstacles.Dequeue());
    }


    void SetNextGateColor()
    {
        //if (data.colors.Count == 0) data.colors = new List<Color>(data.baseColors);
        int i = Random.Range(0, data.Colors.Count);
        data.gateMat.color = data.Colors[i];
        data.Colors.RemoveAt(i);
    }


    void InitColors()
    {
        int i = Random.Range(0, data.Colors.Count);
        data.gateMat.color = data.Colors[i];
        data.snakeMat.color = data.Colors[i];
        data.Colors.RemoveAt(i);

    }
    IEnumerator WaitForInput()
    {
        while (!Input.anyKeyDown)
        {
            
            yield return null;
        }
        data.IsGameOver = false;
    }
    void Restart()
    {
        Time.timeScale = 1;
        data.InitData();
        SceneManager.LoadScene(0);
    }

    void DiamondEaten(int val)
    {
        if (diamondsEaten >= 3)
        {
            data.IsFever = true;
            foreach (Coroutine coroutine in diamondCounters)
            {
                StopCoroutine(coroutine);
            }
            diamondsEaten = 0;
            diamondCounters.Clear();
        }
        else
        {
            diamondCounters.Add(StartCoroutine(DiamondCounter()));
        }
        
    }

    IEnumerator DiamondCounter()
    {
        diamondsEaten++;
        yield return new WaitForSeconds(5f);
        diamondsEaten--;
    }

    void StartFever(bool isFever)
    {
        if(isFever) StartCoroutine(StartFeverCoroutine());
    }
    IEnumerator StartFeverCoroutine()
    {
        data.moveSpeed *= data.feverSpeedMultiplier;
        yield return new WaitForSeconds(5f);
        data.moveSpeed /= data.feverSpeedMultiplier;
        data.IsFever = false;      
    }

    void DespawnDudes()
    {
        for(int i=0; i<8; i++)
        {
            Destroy(data.dudes.Dequeue());
        }
    }
    void SpawnDudes(int track)
    {
        float minPos = data.trackLength * data.dudesMinPos + (data.trackLength* track);
        float maxPos = data.trackLength * data.dudesMaxPos + (data.trackLength* track);
        float step = (maxPos - minPos) / 4;

        data.dudeMat.color = data.gateMat.color;
        int x = Random.Range(0, data.Colors.Count);
        Color secondColor = data.Colors[x];
        data.Colors.RemoveAt(x);

        List<Vector2> positions = new List<Vector2>();
        Vector2 pos;
        Vector3 fullPos;
        GameObject dudes1;
        GameObject dudes2;
        for(int i=0; i<4; i++)
        {
            pos.x = -data.dudesXoffset;
            pos.y = minPos + step*i+Random.Range(-data.dudesYoffset, data.dudesYoffset + 1);
            positions.Add(pos);

            pos.x = data.dudesXoffset;
            pos.y = minPos + step * i + Random.Range(-data.dudesYoffset, data.dudesYoffset + 1);
            positions.Add(pos);
        }

        for(int i=0; i<8; i+=2)
        {
            fullPos.x = positions[i].x;
            fullPos.y = 0.3f;
            fullPos.z = positions[i].y;

            dudes1 = Instantiate(dudesPrefab, fullPos, Quaternion.identity*Quaternion.AngleAxis(Random.Range(0,180), Vector3.up));

            fullPos.x = positions[i+1].x;
            fullPos.y = 0.3f;
            fullPos.z = positions[i+1].y;

            dudes2 = Instantiate(dudesPrefab, fullPos, Quaternion.identity * Quaternion.AngleAxis(Random.Range(0, 180), Vector3.up));


            MaterialPropertyBlock materialBlock = new MaterialPropertyBlock();
            if (Random.Range(0, 2) == 0)
            {
                
                foreach(Renderer rend in dudes1.GetComponentsInChildren<Renderer>())
                {
                    rend.GetPropertyBlock(materialBlock);
                    materialBlock.SetColor("_Color", data.dudeMat.color);
                    rend.SetPropertyBlock(materialBlock);
                }

                foreach (Renderer rend in dudes2.GetComponentsInChildren<Renderer>())
                {
                    rend.GetPropertyBlock(materialBlock);
                    materialBlock.SetColor("_Color", secondColor);
                    rend.SetPropertyBlock(materialBlock);
                }
            }
            else
            {
                foreach (Renderer rend in dudes1.GetComponentsInChildren<Renderer>())
                {
                    rend.GetPropertyBlock(materialBlock);
                    materialBlock.SetColor("_Color", secondColor);
                    rend.SetPropertyBlock(materialBlock);
                }

                foreach (Renderer rend in dudes2.GetComponentsInChildren<Renderer>())
                {
                    rend.GetPropertyBlock(materialBlock);
                    materialBlock.SetColor("_Color", data.dudeMat.color);
                    rend.SetPropertyBlock(materialBlock);
                }
            }
            data.dudes.Enqueue(dudes1);
            data.dudes.Enqueue(dudes2);

            
        }
    }
}
