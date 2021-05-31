using System.Collections;
using UnityEngine;

public class Cone : MonoBehaviour
{

    public Data data;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude"))
        {
            MaterialPropertyBlock materialBlock = new MaterialPropertyBlock();
            other.gameObject.GetComponent<Renderer>().GetPropertyBlock(materialBlock);
            if (materialBlock.GetColor("_Color") == data.snakeMat.color || data.IsFever)
            {
                StartCoroutine(EatObject(other.gameObject));
                data.DudeScore++;
            }
 

        }
        else if (other.CompareTag("Diamond"))
        {
            StartCoroutine(EatObject(other.gameObject));
            if(!data.IsFever) data.DiamondScore += 10;
        }
        else  if(other.CompareTag("Obstacle"))
        {
            if(data.IsFever) StartCoroutine(EatObject(other.gameObject));
        }
    }

    IEnumerator EatObject(GameObject obj)
    {
        float time = Time.time;
        while (Time.time-time < 0.05f)
        {
            obj.transform.localScale *= 0.9f;
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, transform.position+Vector3.forward*0.5f, 2.5f * Time.deltaTime);
            yield return null;
        }
        obj.SetActive(false); 
    }

}
