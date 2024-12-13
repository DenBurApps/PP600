using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SpawnArea : MonoBehaviour
{
    [SerializeField] private RectTransform _minX;
    [SerializeField] private RectTransform _maxX;
    [SerializeField] private float _yPosition;

    public Vector2 GetRandomXPositionToSpawn()
    {
        float randomX = Random.Range(_minX.position.x, _maxX.position.x);
        
        return new Vector2(randomX, _yPosition);
    }
}
