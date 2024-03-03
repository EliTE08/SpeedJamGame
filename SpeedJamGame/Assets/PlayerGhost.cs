using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerGhost : Singleton<PlayerGhost>
{
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private PlayerController player;
    [SerializeField] private List<GameObject> checkPoints;
    private List<Vector2> _playerPosition = new List<Vector2>();
    private Transform _playerTransform;
    private Transform _nearestCheckPoint;
    private bool _isPlaying;
    private int _frame;
    private bool _shouldRecord = true;

    protected override void Awake()
    {
        base.Awake();
        _playerTransform = player.transform;
        player.onCheckPointHit += CheckpointHit;
    }

    public void PlayRecorded()
    {
        vcam.m_Follow = transform;
        _isPlaying = true;
        _shouldRecord = false;
        _frame = 0;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
            PlayRecorded();
        
        foreach (var checkPoint in checkPoints)
        {
            if ((_playerTransform.position - checkPoint.transform.position).sqrMagnitude <= 64f)
            {
                _nearestCheckPoint = checkPoint.transform;
                break;
            }
        }
        
        if (_nearestCheckPoint != null && _shouldRecord)
            Record();
        if(_nearestCheckPoint == null && !_isPlaying)
            _playerPosition.Clear();
        if (_isPlaying)
        {
            transform.position = _playerPosition[_frame];
            _frame++;
            if (_frame >= _playerPosition.Count)
            {
                _isPlaying = false;
                _shouldRecord = true;
                vcam.m_Follow = player.transform;
                player.Respawn(transform.position);
            }
        }
    }
    
    private void CheckpointHit(GameObject checkPoint)
    {
        checkPoints.Remove(checkPoint);
        //_nearestCheckPoint = null;
        _shouldRecord = false;
    }

    private void Record()
    {
        _playerPosition.Add(player.transform.position);
    }
}
