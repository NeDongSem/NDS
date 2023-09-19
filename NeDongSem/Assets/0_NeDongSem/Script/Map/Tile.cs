using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private int m_parentTileNumber = 0;
    private int m_tileNumber = 0;
    private int m_col = 18;//¸ÊÀÇ ¿­ÀÇ ¼ö

    private eTileType m_eTileType;
    public eTileType TileType
    {
        get { return m_eTileType; }
        set { m_eTileType = value; }
    }

    void Start()
    {
        SetTileName();
    }

    private void SetTileName()
    {
        string _parentTileNumber;
        string _tileNumber;

        if (transform.parent.name.Length == 9)
        {
            _parentTileNumber = transform.parent.name.Substring(transform.parent.name.Length - 1);
            int.TryParse(_parentTileNumber, out m_parentTileNumber);
        }
        else if(transform.parent.name.Length == 10)
        {
            _parentTileNumber = transform.parent.name.Substring(transform.parent.name.Length - 2);
            int.TryParse(_parentTileNumber, out m_parentTileNumber);
        }

        if (transform.name.Length == 15)
        {
            _tileNumber = transform.name.Substring(transform.name.Length - 1);
            int.TryParse(_tileNumber, out m_tileNumber);
        }
        else if (transform.name.Length == 16)
        {
            _tileNumber = transform.name.Substring(transform.name.Length - 2);
            int.TryParse(_tileNumber, out m_tileNumber);
        }

        int _finalTileNumber = 0;
        _finalTileNumber = (m_parentTileNumber - 1) * m_col + m_tileNumber;
        string _tileName = "Tile" + _finalTileNumber.ToString();
        transform.name = _tileName;
    }
}
