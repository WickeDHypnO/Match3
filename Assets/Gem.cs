using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GemType
{
    Red,
    Blue,
    Yellow,
    Green,
    Pink,
    Orange
}

public enum GemState
{
    Normal,
    Selected
}

public class Gem : MonoBehaviour
{
    [SerializeField]
    public List<Color> colors;
    public Vector2 position;
    public bool particlesAfterDestroy = true;
    GemType type;
    public GameObject selected;
    public ParticleSystem hint;
    public GemType Type
    {
        get { return type; }
        set
        {
            type = value;
            GetComponent<SpriteRenderer>().color = colors[(int)type];
        }
    }
    GemState state;
    public GemState State
    {
        get { return state; }
        set
        {
            state = value;
            if(state == GemState.Selected)
            {
                selected.SetActive(true);
            }
            else
            {
                selected.SetActive(false);
            }
        }
    }
    public GameObject destroyParticle;


    void OnMouseDown()
    {
        if(GameManager.interactable)
        SelectGem();
    }

    public void SelectGem()
    {
        FindObjectOfType<Board>().SelectGem(this);
    }

    public void Hint()
    {
        hint.Play();
    }

    void OnDestroy()
    {
        if(GameManager.gameStarted && particlesAfterDestroy)
        Instantiate(destroyParticle, transform.position, Quaternion.identity);
    }
}
