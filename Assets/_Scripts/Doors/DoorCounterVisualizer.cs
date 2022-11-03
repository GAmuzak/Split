using System.Collections.Generic;
using UnityEngine;

public class DoorCounterVisualizer : MonoBehaviour
{
    [SerializeField] private float separation;
    [SerializeField] private float factor = 10f;

    private GameObject _counterTile;
    private Transform _counterTileTransform;
    private List<GameObject> _counterTiles;
    private int _index;

    private void Start()
    {
        _counterTile = transform.GetChild(1).GetChild(0).gameObject;
        _counterTileTransform = _counterTile.transform;
        _counterTiles = new List<GameObject>();
    }

    public void SetVisualizer(int index)
    {
        _counterTile.gameObject.SetActive(true);
        
        float scale = 100f / index;

        var localScale = _counterTileTransform.localScale;
        _counterTile.transform.localScale = new Vector3(localScale.x, scale - separation, localScale.z);
        
        for (int i = 1; i < index; i++)
        {
            var tileTemp = Instantiate(_counterTile,
                _counterTile.transform.position + _counterTile.transform.up * factor * (scale / 100) * i,
                _counterTile.transform.rotation, transform.GetChild(1));
            
            _counterTiles.Add(tileTemp);

            var tileTransform = tileTemp.transform.localScale;
            var transformLocalScale = new Vector3(tileTransform.x, scale - separation, tileTransform.z);
            tileTemp.transform.localScale = transformLocalScale;
        }

        _index = _counterTiles.Count - 1;
    }

    public void CountDown()
    {
        if (_counterTiles.Count == 0) return;

        var counterTileMesh = _counterTiles[_index].transform.GetChild(0);
        counterTileMesh.GetComponent<MeshRenderer>().material =
            transform.GetChild(0).GetComponent<MeshRenderer>().material;
        counterTileMesh.GetComponent<Animator>().Play("Count", -1, 0f);
        _index--;
    }

    public void ResetTiles()
    {
        foreach (var counterTile in _counterTiles)
            Destroy(counterTile);
        _counterTiles.Clear();
        _counterTile.gameObject.SetActive(false);
    }
}
