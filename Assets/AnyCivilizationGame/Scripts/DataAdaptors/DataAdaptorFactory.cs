using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DataAdaptorFactory
{
    public static DataAdaptor Get(DataAdaptorType type)
    {
        switch (type)
        {
            case DataAdaptorType.Json:
                return new JsonDataAdaptor();
            case DataAdaptorType.Http:
                throw new NotImplementedException("Http is not implemented");
            case DataAdaptorType.LoadBalanacer:
                throw new NotImplementedException("LoadBalanacer is not implemented");
            default:
                return new JsonDataAdaptor();
        }
    }
}
