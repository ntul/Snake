using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{

    public Data data;
    public GameObject gameOverScreen;
    public TextMeshProUGUI dudeScore;
    public TextMeshProUGUI diamondScore;
    public GameObject dudeScaler;
    public GameObject diamondScaler;
    public GameObject feverUi;
    public GameObject levelComplete;

    private void OnEnable()
    {
        data.OnGameOver += GameOver;
        data.OnRestart += Restart;
        data.OnDudeEaten += SetDudeScore;
        data.OnDiamondEaten += SetDiamondScore;
        data.OnFever += StartFever;
        data.OnGateCross += GateCross;
    }

    private void OnDisable()
    {
        data.OnGameOver -= GameOver;
        data.OnRestart -= Restart;
        data.OnDudeEaten -= SetDudeScore;
        data.OnDiamondEaten -= SetDiamondScore;
        data.OnFever -= StartFever;
        data.OnGateCross -= GateCross;
    }

    void GameOver()
    {
        gameOverScreen.SetActive(true);
    }

    void Restart()
    {
        gameOverScreen.SetActive(false);
    }

    void GateCross()
    {
        if (data.GatesCrossed >= data.winGates) LevelComplete();
    }

    void LevelComplete()
    {
        levelComplete.SetActive(true);
        feverUi.SetActive(false);
    }
    void SetDudeScore(int score)
    {
        //dudeScore.text = score.ToString();
        StartCoroutine(AnimateDudeScore(int.Parse(dudeScore.text), score));
    }

    void SetDiamondScore(int score)
    {
        StartCoroutine(AnimateDiamondScore(int.Parse(diamondScore.text), score)); 
    }

    void StartFever(bool isFever)
    {
        if (isFever)
        {
            feverUi.SetActive(true);
            StartCoroutine(AnimateFever());
        }
        else feverUi.SetActive(false);
            
    }

    IEnumerator AnimateFever()
    {
        float time = Time.time;
        while (Time.time - time < 5f)
        {
            while (feverUi.transform.localScale.x < 1.1f)
            {
                feverUi.transform.localScale += Vector3.one * 0.01f;
                yield return null;
            }
            while (feverUi.transform.localScale.x > 1f)
            {
                feverUi.transform.localScale -= Vector3.one * 0.01f;
                yield return null;
            }
        }
        
    }
    IEnumerator AnimateDiamondScore(int textScore ,int score)
    {
        while (textScore < score)
        {
            textScore++;
            diamondScore.text = textScore.ToString();
            //diamondScore.fontSize += 2;
            diamondScaler.transform.localScale *= 1.05f;
            yield return new WaitForSecondsRealtime(0.025f);
            diamondScaler.transform.localScale /= 1.05f;
            //diamondScore.fontSize -= 2;
        }
        
    }

    IEnumerator AnimateDudeScore(int textScore, int score)
    {

        textScore++;
        dudeScore.text = textScore.ToString();
        //dudeScore.fontSize += 2;
        dudeScaler.transform.localScale *= 1.05f;
        yield return new WaitForSecondsRealtime(0.025f);
        dudeScaler.transform.localScale /= 1.05f;
        //dudeScore.fontSize -= 2;


    }
}
