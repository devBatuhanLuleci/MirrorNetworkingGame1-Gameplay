using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataAdaptorFactory
{
    public DataAdaptor Get(DataAdaptorType type)
    {
        switch (type)
        {
            case DataAdaptorType.Json:
                return new JsonDataAdaptor();
                break;
            case DataAdaptorType.Http:
                throw new NotImplementedException("Http is not implemented");
                break;
            case DataAdaptorType.LoadBalanacer:
                throw new NotImplementedException("LoadBalanacer is not implemented");
                break;
            default:
                return new JsonDataAdaptor();
                break;
        }
    }
}
