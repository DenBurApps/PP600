using UnityEngine;

public class CellSpriteProvider : MonoBehaviour
{
    [SerializeField] private Sprite _first;
    [SerializeField] private Sprite _second;
    [SerializeField] private Sprite _third;
    [SerializeField] private Sprite _fourth;
    [SerializeField] private Sprite _fifth;
    [SerializeField] private Sprite _sixth;
    [SerializeField] private Sprite _seventh;
    [SerializeField] private Sprite _eights;
    [SerializeField] private Sprite _nineth;

    [SerializeField] private Sprite _emptySprite;

    public Sprite GetExactSprite(CellTypes type)
    {
        switch (type)
        {
            case(CellTypes.First):
                return _first;
            case(CellTypes.Second):
                return _second;
            case(CellTypes.Third):
                return _third;
            case (CellTypes.Fourth):
                return _fourth;
            case (CellTypes.Fifth):
                return _fifth;
            case (CellTypes.Sixth):
                return _sixth;
            case (CellTypes.Seventh):
                return _seventh;
            case (CellTypes.Eights):
                return _eights;
            case (CellTypes.Nineth):
                return _nineth;
        }

        return _emptySprite;
    }
}
