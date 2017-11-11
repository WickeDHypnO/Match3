using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static bool gameStarted = false;
    public float delayBeforeGameStarted;
    public static bool interactable;

    // Use this for initialization
    void Start () {
        StartCoroutine(GameStartDelay(delayBeforeGameStarted));
	}

    IEnumerator GameStartDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameStarted = true;
    }
}
