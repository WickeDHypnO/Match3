//Szybki i brzydki kod do gry match3, zrobiony w 6h jako trening przed gamejamem

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject gemPrefab;
    Dictionary<Vector2, GemType> gemBoard = new Dictionary<Vector2, GemType>();
    public Dictionary<Vector2, Gem> gems = new Dictionary<Vector2, Gem>();
    public int boardSize;
    public float boardOffset;
    public float heightOffset;
    public List<KeyValuePair<Vector2, Vector2>> gemsPair = new List<KeyValuePair<Vector2, Vector2>>();

    public Gem selectedGem;
    public Gem switchedGem;

    public void Start()
    {
        RandomBoard();
        ClearThreesAtStart();
        if (!IsMovePossible())
        {
            Reshuffle();
        }
        GameManager.interactable = true;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            if (IsMovePossible())
            {
                HighlightPair();
            }
        }
    }

    public void RandomBoard()
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                gemBoard.Add(new Vector2(i, j), (GemType)Random.Range(0, System.Enum.GetNames(typeof(GemType)).Length));
            }
        }
        SetUpNoMatches();
        InstantiateGems();
    }

    private void InstantiateGems()
    {
        foreach (Vector2 pos in gemBoard.Keys)
        {
            GameObject gem = Instantiate(gemPrefab, new Vector3(pos.x + boardOffset, pos.y + boardOffset, -1), Quaternion.identity);
            gem.GetComponent<Gem>().position = pos;
            gems.Add(pos, gem.GetComponent<Gem>());
            gem.GetComponent<Gem>().Type = gemBoard[pos];
            gem.transform.parent = transform;
        }
    }

    public void MoveGem(Vector2 lastPosition, Vector2 nextPosition)
    {
        iTween.MoveTo(gems[lastPosition].gameObject, new Vector3(nextPosition.x + boardOffset, nextPosition.y + boardOffset, -1), 0.5f);
        gems[lastPosition].position = nextPosition;
        if (!gems.ContainsKey(nextPosition))
        {
            gems.Add(nextPosition, gems[lastPosition]);
            gems.Remove(lastPosition);
        }
        else
        {
            Gem temp = gems[nextPosition];
            gems[nextPosition] = gems[lastPosition];
            gems[lastPosition] = temp;
            iTween.MoveTo(gems[lastPosition].gameObject, new Vector3(lastPosition.x + boardOffset, lastPosition.y + boardOffset, -1), 0.5f);
            gems[lastPosition].position = lastPosition;
        }
    }

    private void HighlightPair()
    {
        int highlight = Random.Range(0, gemsPair.Count);
        gems[gemsPair[highlight].Key].Hint();
        gems[gemsPair[highlight].Value].Hint();
    }

    private void SetUpNoMatches()
    {
        Dictionary<Vector2, GemType> gemBoardTemp = new Dictionary<Vector2, GemType>(gemBoard);
        foreach (Vector2 pos in gemBoardTemp.Keys)
        {
            if (gemBoard.ContainsKey(new Vector2(pos.x + 1, pos.y)) && gemBoard[pos] == gemBoard[new Vector2(pos.x + 1, pos.y)])
            {
                if (gemBoard.ContainsKey(new Vector2(pos.x + 2, pos.y)) && gemBoard[pos] == gemBoard[new Vector2(pos.x + 2, pos.y)])
                {
                    gemBoard[pos] -= 1;
                    if (gemBoard[pos] < 0)
                    {
                        gemBoard[pos] = (GemType)System.Enum.GetNames(typeof(GemType)).Length - 1;
                    }
                }
            }
            gemBoardTemp = new Dictionary<Vector2, GemType>(gemBoard);
            if (gemBoard.ContainsKey(new Vector2(pos.x - 1, pos.y)) && gemBoard[pos] == gemBoard[new Vector2(pos.x - 1, pos.y)])
            {
                if (gemBoard.ContainsKey(new Vector2(pos.x - 2, pos.y)) && gemBoard[pos] == gemBoard[new Vector2(pos.x - 2, pos.y)])
                {
                    gemBoard[pos] -= 1;
                    if (gemBoard[pos] < 0)
                    {
                        gemBoard[pos] = (GemType)System.Enum.GetNames(typeof(GemType)).Length - 1;
                    }
                }
            }
            gemBoardTemp = new Dictionary<Vector2, GemType>(gemBoard);
            if (gemBoard.ContainsKey(new Vector2(pos.x, pos.y + 1)) && gemBoard[pos] == gemBoard[new Vector2(pos.x, pos.y + 1)])
            {
                if (gemBoard.ContainsKey(new Vector2(pos.x, pos.y + 2)) && gemBoard[pos] == gemBoard[new Vector2(pos.x, pos.y + 2)])
                {
                    gemBoard[pos] -= 1;
                    if (gemBoard[pos] < 0)
                    {
                        gemBoard[pos] = (GemType)System.Enum.GetNames(typeof(GemType)).Length - 1;
                    }
                }
            }
            gemBoardTemp = new Dictionary<Vector2, GemType>(gemBoard);
            if (gemBoard.ContainsKey(new Vector2(pos.x, pos.y - 1)) && gemBoard[pos] == gemBoard[new Vector2(pos.x, pos.y - 1)])
            {
                if (gemBoard.ContainsKey(new Vector2(pos.x, pos.y - 2)) && gemBoard[pos] == gemBoard[new Vector2(pos.x, pos.y - 2)])
                {
                    gemBoard[pos] -= 1;
                    if (gemBoard[pos] < 0)
                    {
                        gemBoard[pos] = (GemType)System.Enum.GetNames(typeof(GemType)).Length - 1;
                    }
                }
            }
            gemBoardTemp = new Dictionary<Vector2, GemType>(gemBoard);
        }
    }

    private bool IsMovePossible()
    {
        gemsPair.Clear();
        bool result = false;
        foreach (Vector2 pos in gemBoard.Keys)
        {
            Dictionary<Vector2, GemType> board = new Dictionary<Vector2, GemType>(gemBoard);
            //test going right
            if (gemBoard.ContainsKey(new Vector2(pos.x + 1, pos.y)))
            {
                board[pos] = gemBoard[new Vector2(pos.x + 1, pos.y)];
                board[new Vector2(pos.x + 1, pos.y)] = gemBoard[pos];
                if (Is3MatchForBoard(board))
                {
                    Debug.Log("right");
                    gemsPair.Add(new KeyValuePair<Vector2, Vector2>(pos, new Vector2(pos.x + 1, pos.y)));
                    result = true;
                }
            }
            board = new Dictionary<Vector2, GemType>(gemBoard);
            //test going left
            if (gemBoard.ContainsKey(new Vector2(pos.x - 1, pos.y)))
            {
                board[pos] = gemBoard[new Vector2(pos.x - 1, pos.y)];
                board[new Vector2(pos.x - 1, pos.y)] = gemBoard[pos];
                if (Is3MatchForBoard(board))
                {
                    Debug.Log("left");
                    gemsPair.Add(new KeyValuePair<Vector2, Vector2>(pos, new Vector2(pos.x - 1, pos.y)));
                    result = true;
                }
            }
            board = new Dictionary<Vector2, GemType>(gemBoard);
            //test going up
            if (gemBoard.ContainsKey(new Vector2(pos.x, pos.y + 1)))
            {
                board[pos] = gemBoard[new Vector2(pos.x, pos.y + 1)];
                board[new Vector2(pos.x, pos.y + 1)] = gemBoard[pos];
                if (Is3MatchForBoard(board))
                {
                    Debug.Log("up");
                    gemsPair.Add(new KeyValuePair<Vector2, Vector2>(pos, new Vector2(pos.x, pos.y + 1)));
                    result = true;
                }
            }
            board = new Dictionary<Vector2, GemType>(gemBoard);
            //test going down
            if (gemBoard.ContainsKey(new Vector2(pos.x, pos.y - 1)))
            {
                board[pos] = gemBoard[new Vector2(pos.x, pos.y - 1)];
                board[new Vector2(pos.x, pos.y - 1)] = gemBoard[pos];
                if (Is3MatchForBoard(board))
                {
                    Debug.Log("down");
                    gemsPair.Add(new KeyValuePair<Vector2, Vector2>(pos, new Vector2(pos.x, pos.y - 1)));
                    result = true;
                }
            }
        }
        return result;
    }

    private bool Is3MatchForBoard(Dictionary<Vector2, GemType> board)
    {
        foreach (Vector2 pos in board.Keys)
        {
            if (board.ContainsKey(new Vector2(pos.x + 1, pos.y)) && board[pos] == board[new Vector2(pos.x + 1, pos.y)])
            {
                if (board.ContainsKey(new Vector2(pos.x + 2, pos.y)) && board[pos] == board[new Vector2(pos.x + 2, pos.y)])
                {
                    return true;
                }
            }
            if (board.ContainsKey(new Vector2(pos.x - 1, pos.y)) && board[pos] == board[new Vector2(pos.x - 1, pos.y)])
            {
                if (board.ContainsKey(new Vector2(pos.x - 2, pos.y)) && board[pos] == board[new Vector2(pos.x - 2, pos.y)])
                {
                    return true;
                }
            }
            if (board.ContainsKey(new Vector2(pos.x, pos.y + 1)) && board[pos] == board[new Vector2(pos.x, pos.y + 1)])
            {
                if (board.ContainsKey(new Vector2(pos.x, pos.y + 2)) && board[pos] == board[new Vector2(pos.x, pos.y + 2)])
                {
                    return true;
                }
            }
            if (board.ContainsKey(new Vector2(pos.x, pos.y - 1)) && board[pos] == board[new Vector2(pos.x, pos.y - 1)])
            {
                if (board.ContainsKey(new Vector2(pos.x, pos.y - 2)) && board[pos] == board[new Vector2(pos.x, pos.y - 2)])
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void DropDown()
    {
        for (int i = boardSize - 1; i >= 0; i--)
        {
            for (int j = boardSize - 1; j >= 0; j--)
            {
                if (gemBoard.ContainsKey(new Vector2(i, j)) && j > 0)
                {
                    if (!gemBoard.ContainsKey(new Vector2(i, j - 1)))
                    {
                        int iter = 0;
                        while (gemBoard.ContainsKey(new Vector2(i, j + iter)))
                        {
                            gemBoard[new Vector2(i, j + iter - 1)] = gemBoard[new Vector2(i, j + iter)];
                            gemBoard.Remove(new Vector2(i, j + iter));
                            MoveGem(new Vector2(i, j + iter), new Vector2(i, j + iter - 1));
                            iter++;
                        }
                    }
                }
            }
        }
    }

    private void FillHoles()
    {
        for (int i = boardSize - 1; i >= 0; i--)
        {
            for (int j = boardSize - 1; j >= 0; j--)
            {
                if (!gemBoard.ContainsKey(new Vector2(i, j)))
                {
                    gemBoard.Add(new Vector2(i, j), (GemType)Random.Range(0, System.Enum.GetNames(typeof(GemType)).Length));
                    GameObject gem = Instantiate(gemPrefab, new Vector3(i + boardOffset, j + boardOffset + heightOffset, -1), Quaternion.identity);
                    gem.GetComponent<Gem>().position = new Vector2(i, j);
                    gems.Add(new Vector2(i, j), gem.GetComponent<Gem>());
                    gem.GetComponent<Gem>().Type = gemBoard[new Vector2(i, j)];
                    gem.transform.parent = transform;
                    if (GameManager.gameStarted)
                    {
                        iTween.MoveTo(gem.gameObject, new Vector3(i + boardOffset, j + boardOffset, -1), 0.5f);
                    }
                    else
                    {
                        gem.transform.position = new Vector3(i + boardOffset, j + boardOffset, -1);
                    }
                }
            }
        }
    }

    public void SelectGem(Gem gem)
    {
        if (selectedGem)
        {
            switchedGem = gem;
            Dictionary<Vector2, GemType> board = new Dictionary<Vector2, GemType>(gemBoard);
            board[selectedGem.position] = gemBoard[switchedGem.position];
            board[switchedGem.position] = gemBoard[selectedGem.position];

            if (IsGemNextToSelected() && Is3MatchForBoard(board))
            {
                Debug.Log("good");
                gemBoard = board;
                SwitchGems();
                ClearAfterMove();
            }
            else
            {
                Debug.Log("bad");
                selectedGem.State = GemState.Normal;
                switchedGem.State = GemState.Normal;
                selectedGem = null;
                switchedGem = null;
            }
        }
        else
        {
            selectedGem = gem;
            gem.State = GemState.Selected;
        }
    }

    private bool IsGemNextToSelected()
    {
        if (Vector2.Distance(selectedGem.position, switchedGem.position) > 1.1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void SwitchGems()
    {
        Vector2 temp = selectedGem.position;
        MoveGem(selectedGem.position, switchedGem.position);
        MoveGem(switchedGem.position, temp);
        selectedGem.State = GemState.Normal;
        switchedGem.State = GemState.Normal;
        selectedGem = null;
        switchedGem = null;
    }

    private void ClearThrees(Dictionary<Vector2, GemType> board)
    {
        List<Vector2> gemPositionsToRemove = new List<Vector2>();
        foreach (Vector2 pos in board.Keys)
        {
            if (board.ContainsKey(new Vector2(pos.x + 1, pos.y)) && board[pos] == board[new Vector2(pos.x + 1, pos.y)])
            {
                if (board.ContainsKey(new Vector2(pos.x + 2, pos.y)) && board[pos] == board[new Vector2(pos.x + 2, pos.y)])
                {
                    gemPositionsToRemove.Add(pos);
                    gemPositionsToRemove.Add(new Vector2(pos.x + 1, pos.y));
                    gemPositionsToRemove.Add(new Vector2(pos.x + 2, pos.y));
                }
            }
            if (board.ContainsKey(new Vector2(pos.x - 1, pos.y)) && board[pos] == board[new Vector2(pos.x - 1, pos.y)])
            {
                if (board.ContainsKey(new Vector2(pos.x - 2, pos.y)) && board[pos] == board[new Vector2(pos.x - 2, pos.y)])
                {
                    gemPositionsToRemove.Add(pos);
                    gemPositionsToRemove.Add(new Vector2(pos.x - 1, pos.y));
                    gemPositionsToRemove.Add(new Vector2(pos.x - 2, pos.y));
                }
            }
            if (board.ContainsKey(new Vector2(pos.x, pos.y + 1)) && board[pos] == board[new Vector2(pos.x, pos.y + 1)])
            {
                if (board.ContainsKey(new Vector2(pos.x, pos.y + 2)) && board[pos] == board[new Vector2(pos.x, pos.y + 2)])
                {
                    gemPositionsToRemove.Add(pos);
                    gemPositionsToRemove.Add(new Vector2(pos.x, pos.y + 1));
                    gemPositionsToRemove.Add(new Vector2(pos.x, pos.y + 2));
                }
            }
            if (board.ContainsKey(new Vector2(pos.x, pos.y - 1)) && board[pos] == board[new Vector2(pos.x, pos.y - 1)])
            {
                if (board.ContainsKey(new Vector2(pos.x, pos.y - 2)) && board[pos] == board[new Vector2(pos.x, pos.y - 2)])
                {
                    gemPositionsToRemove.Add(pos);
                    gemPositionsToRemove.Add(new Vector2(pos.x, pos.y - 1));
                    gemPositionsToRemove.Add(new Vector2(pos.x, pos.y - 2));
                }
            }
        }
        foreach (Vector2 pos in gemPositionsToRemove)
        {
            gemBoard.Remove(pos);
            if (gems.ContainsKey(pos))
                Destroy(gems[pos].gameObject);
            gems.Remove(pos);
        }
    }

    IEnumerator ShowChanges()
    {
        GameManager.interactable = false;
        while (Is3MatchForBoard(gemBoard))
        {
            yield return new WaitForSeconds(0.5f);
            ClearThrees(gemBoard);
            yield return new WaitForSeconds(0.25f);
            DropDown();
            FillHoles();
            yield return new WaitForSeconds(0.25f);
        }
        if(!IsMovePossible())
        {
            StartCoroutine(Reshuffle());
        }
        GameManager.interactable = true;
    }

    IEnumerator Reshuffle()
    {
        List<GemType> gemTypes = new List<GemType>();
        foreach (Vector2 pos in gemBoard.Keys)
        {
            gemTypes.Add(gems[pos].Type);
        }
        foreach (Vector2 pos in gems.Keys)
        {
            int index = Random.Range(0, gemTypes.Count);
            gemBoard[pos] = gemTypes[index];
            gemTypes.RemoveAt(index);
        }
        foreach(Vector2 pos in gems.Keys)
        {
            iTween.MoveTo(gems[pos].gameObject, new Vector3(boardSize / 2 + boardOffset, boardSize / 2 + boardOffset, -1), 0.5f);
        }
        yield return new WaitForSeconds(0.5f);
        foreach (Vector2 pos in gemBoard.Keys)
        {
            gems[pos].particlesAfterDestroy = false;
            Destroy(gems[pos].gameObject);
            GameObject gem = Instantiate(gemPrefab, new Vector3(boardSize / 2 + boardOffset, boardSize / 2 + boardOffset, -1), Quaternion.identity);
            gem.GetComponent<Gem>().position = pos;
            gems[pos] = gem.GetComponent<Gem>();
            gem.GetComponent<Gem>().Type = gemBoard[pos];
            gem.transform.parent = transform;
            iTween.MoveTo(gems[pos].gameObject, new Vector3(pos.x + boardOffset, pos.y + boardOffset, -1), 0.5f);
        }
        StartCoroutine(ShowChanges());
    }

    void ClearAfterMove()
    {
        StartCoroutine(ShowChanges());
    }

    private void ClearThreesAtStart()
    {
        while (Is3MatchForBoard(gemBoard))
        {
            ClearThrees(gemBoard);
            DropDown();
            FillHoles();
        }
    }
}
