using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    #region Variables

    [SerializeField]
    [Header("Initialization")]
    private bool _spinOnStart = true;

    private bool _isSpinning = false;
    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        Initialize();
    }

    private void LateUpdate()
    {
        if (_isSpinning)
        {
            Rotate();
        }
    }

    private void Rotate()
    {
        var currentThrobberRotation = transform.rotation * Quaternion.Euler(0, 0, 1);
        transform.rotation = currentThrobberRotation;
    }

    #endregion
    #region Setup Methods

    private void Initialize()
    {
        _isSpinning = _spinOnStart;
    }

    #endregion

    /// <summary>
    /// Starting Spining
    /// </summary>
    public void StartSpin()
    {
        _isSpinning =true;
    }
    /// <summary>
    /// Stoping Spinng
    /// </summary>
    public void StopSpin()
    {
        _isSpinning = false;
    }
}
