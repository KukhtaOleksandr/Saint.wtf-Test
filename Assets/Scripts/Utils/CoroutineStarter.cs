using System.Collections;
using UnityEngine;

public class CoroutineStarter : MonoBehaviour
{
    public void StartCoroutineByMethod(IEnumerator method)
    {
        StartCoroutine(method);
    }
}
