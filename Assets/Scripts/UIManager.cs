using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public ScrollRect sr;
    public float x;
    public float y;
    public GameObject item;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(printSizeDelta());
        print("start@1: "+sr.content.sizeDelta.y);
        Instantiate(item, Vector3.one, Quaternion.identity, sr.content.transform);
        Instantiate(item, Vector3.one, Quaternion.identity, sr.content.transform);
        Instantiate(item, Vector3.one, Quaternion.identity, sr.content.transform);
        Instantiate(item, Vector3.one, Quaternion.identity, sr.content.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(sr.gameObject.GetComponent<RectTransform>());
        print("start@2: "+sr.content.sizeDelta.y);
        LayoutRebuilder.ForceRebuildLayoutImmediate(sr.content.gameObject.GetComponent<RectTransform>());
        print("start@3: "+sr.content.sizeDelta.y);
    }

    public void saysomething()
    {
        Debug.Log("hello world");
    }
    
    IEnumerator printSizeDelta()
    {
        yield return new WaitForEndOfFrame();
        print("frame@1:"+sr.content.sizeDelta.y);
        yield return new WaitForEndOfFrame();
        print("frame@2:"+sr.content.sizeDelta.y);
        yield return new WaitForEndOfFrame();
        print("frame@3:"+sr.content.sizeDelta.y);
    }
}
