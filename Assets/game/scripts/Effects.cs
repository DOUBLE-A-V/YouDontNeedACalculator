using UnityEngine;
using TMPro;
using DG.Tweening;

public class Effects : MonoBehaviour
{
    public TMP_Text textPrefab;

    public TMP_Text SpawnText(string text, Color color, Vector3 pos, int size = 4)
    {
        TMP_Text obj = Instantiate(textPrefab);

        obj.text = text;
        obj.fontSize = size;
        obj.transform.position = pos;
        obj.transform.DOMove(new Vector3(Random.Range(0f, 1f), Random.Range(-1f, 1f), 0) + pos, 0.5f).SetEase(Ease.OutExpo);
        obj.color = color;
        obj.DOFade(0, 2f).SetEase(Ease.InExpo);
        Destroy(obj.gameObject, 2f);
        return obj;
    }

    public void SpawnEffect(string effectName, Vector3 pos)
    {
        GameObject prefab = M.prefabs.effects.Find(x => x.name == effectName);
        if (prefab)
        {
            prefab = Instantiate(prefab);
            prefab.transform.position = pos;
        } else
        {
            Debug.Log("effect not found: " + effectName);
        }
    }
}
