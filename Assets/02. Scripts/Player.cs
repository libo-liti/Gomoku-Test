using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<Vector2> _visited = new List<Vector2>();
    private int[] _board = new int[16*16];
    private GameObject _currentStone;

    public Transform Board;
    public GameObject whiteStone;
    public GameObject blackStone;

    private Camera _camera;
    private SpriteRenderer _sprite;
    private Vector2 _pos;

    private int n = 0;
    private bool isPlay;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isPlay)    // 마우스 눌렸을때 투명한 바둑 생성
        {
            var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            if ((mousePos.x < 0 || 15 < mousePos.x) || (mousePos.y < 0 || 15 < mousePos.y))
                return;
            
            _pos = GetStonePosition();
            if (IsVisited(_pos))
                return;
            Preview();
        }
        else if (Input.GetMouseButton(0) && isPlay)    // 마우스 드래그 중에 돌 움직임
        {
            _pos = GetStonePosition();
            if (IsVisited(_pos))
                return;
            
            _currentStone.transform.position = _pos;
        }
        else if (Input.GetMouseButtonUp(0) && isPlay)  // 마우스 땠을때 바둑판 위에 돌 위치 시킴
        {
            SetStone();
        }
    }

    /// <summary>
    /// 마우스 위치(스크린) -> 2차원 좌표 -> 바둑판
    /// </summary>
    /// <returns></returns>
    private Vector2 GetStonePosition()
    {
        // 마우스 위치 -> 2차원 좌표
        Vector2 pos = _camera.ScreenToWorldPoint(Input.mousePosition);
        var newPos = pos;
        
        // 바둑판 모서리로 이동
        newPos.x = Mathf.Round(pos.x);
        newPos.y = Mathf.Round(pos.y);

        // 바둑판 범위체크
        newPos.x = Mathf.Clamp(newPos.x, 0, 15);
        newPos.y = Mathf.Clamp(newPos.y, 0, 15);
        
        return newPos;
    }

    // 투명도 변화
    private Color ChangeAlpha(SpriteRenderer sprite, float a)
    {
        var color = sprite.color;
        color.a = a;
        sprite.color = color;
        return color;
    }
    
    // 바둑돌이 놓여 있는지 체크
    private bool IsVisited(Vector2 pos)
    {
        var index = (int)(pos.y * 16 + pos.x);
        bool result = _board[index] == 1 ? true : false;
        return result;
    }

    // 바둑돌 놓기
    private void SetStone()
    {
        // 바둑돌이 놓인 위치를 저장
        _pos = _currentStone.transform.position;
        _board[(int)(_pos.y * 16 + _pos.x)] = 1;
        _visited.Add(_pos);
        
        // 바둑돌을 board 자식으로 두기
        _currentStone.transform.SetParent(Board);
        
        _sprite.color = ChangeAlpha(_sprite, 1f);
        _currentStone = null;
        isPlay = false;
    }

    // 바둑돌 놓을 위치 미리보기
    private void Preview()
    {
        if (n++ % 2 == 0)
            _currentStone = Instantiate(blackStone, _pos, Quaternion.identity);
        else
            _currentStone = Instantiate(whiteStone, _pos, Quaternion.identity);

        isPlay = true;
        _sprite = _currentStone.GetComponent<SpriteRenderer>();
        _sprite.color = ChangeAlpha(_sprite, 0.6f);
    }
}
