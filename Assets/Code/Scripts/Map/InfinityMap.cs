using System;
using DG.Tweening;
using UnityEngine;

public class InfinityMap : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxEffectMultiplier;
    [SerializeField] private bool infiniteHorizontal;
    [SerializeField] private bool infiniteVertical;
    [SerializeField] private Transform navMeshGO;
    private Transform _cameraTransform;
    private Vector3 _lastcameraposition;
    private float _textureUnitSizeX;
    private float _textureUnitSizeY;
    private Transform _transform;
    private Sequence _sequence;
    private bool _isTweenRunning = false;
    public static event Action OnMapReposition;


    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _lastcameraposition = _cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        _textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        _textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
        _transform = transform;
    }

    private void Update()
    {
        Vector3 deltaMovement = _cameraTransform.position - _lastcameraposition;
        _transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.x);
        _lastcameraposition = _cameraTransform.position;

        if (infiniteHorizontal)
        {
            if (Mathf.Abs(_cameraTransform.position.x - _transform.position.x) >= _textureUnitSizeX)
            {
                float offsetPositionX = (_cameraTransform.position.x - _transform.position.x)% _textureUnitSizeX;
               _transform.position = new Vector3(_cameraTransform.position.x + offsetPositionX, _transform.position.y);
                
                if (!_isTweenRunning)
                {
                    _isTweenRunning = true;
                    navMeshGO
                        .DOMove(new Vector3(_cameraTransform.position.x + offsetPositionX, _transform.position.y), 0.2f)
                        .SetEase(Ease.Linear)
                        .OnComplete(() => { _isTweenRunning = false; });
                }
            }
        }



        if (infiniteVertical)
        {
            if (Mathf.Abs(_cameraTransform.position.y - _transform.position.y) >= _textureUnitSizeY)
            {
                float offsetPositionY = (_cameraTransform.position.y - _transform.position.y) % _textureUnitSizeY;
                _transform.position = new Vector3(_transform.position.x, _cameraTransform.position.y + offsetPositionY);

                if (!_isTweenRunning)
                {
                    _isTweenRunning = true;
                    
                    navMeshGO.DOMove(new Vector3(_transform.position.x, _cameraTransform.position.y + offsetPositionY),3f)
                        .SetEase(Ease.Linear)
                        .OnComplete(() => { _isTweenRunning = false; });
                }



            }
        }
    }


    private void InvokeReposition()
    {
        OnMapReposition?.Invoke();
    }
}