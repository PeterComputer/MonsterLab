using UnityEngine;

public class SpriteScroller : MonoBehaviour {
    private Renderer _renderer;
    [SerializeField] private float _x, _y;

    private Vector2 _offset;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        _offset += new Vector2(_x, _y) * Time.deltaTime;
        _renderer.material.mainTextureOffset = _offset;
    }
}