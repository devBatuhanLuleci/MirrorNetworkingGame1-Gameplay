using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu()]
public class EnvironmentConfigiration : ScriptableObject
{

    private static EnvironmentConfigiration _instance;
    public static EnvironmentConfigiration Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<EnvironmentConfigiration>("EnvironmentConfigiration");
            }
            return _instance;
        }
    }

    public bool Log = true;

    public bool IsProduction = false;



}

