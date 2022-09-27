using Assets.HttpClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HttpClient : MonoBehaviour
{
    #region Singleton
    private static HttpClient _instance;
    public static HttpClient Instance
    {
        get
        {
            // if instance is null create a new httpClient instance for request handling
            //  after creating must initialize with configs.
            if (_instance == null)
            {
                _instance = new GameObject("HttpClient").AddComponent<HttpClient>();
                _instance.Initialize();
                DontDestroyOnLoad(_instance);
            }

            return _instance;
        }
    }

    #endregion

    private HttpClientOptions _options;


    /// <summary>
    /// Configure with environment file.
    /// Config file name must be "Production" or "Development" 
    /// The file should be in Resources/Environments
    /// </summary>
    public void Initialize()
    {
        var path = "Environments/" + (EnvironmentConfigiration.Instance.IsProduction ? "Production" : "Development");
        _options = Resources.Load<HttpClientOptions>(path);
        if (_options == null)
        {
            LogError("HttpClientOptions is not found in Resources/Environments/");
            Destroy(gameObject);
            return;
        }
        InitWarning();
    }

    private void InitWarning()
    {
        var mode = EnvironmentConfigiration.Instance.IsProduction ? $"<color=red>Production</color>" : $"<color=green>Development</color>";
        LogWarning($"Project production mod is {mode}.");
    }
    #region Http Methods
    public void Post<T>(IPostRequest webRequestArgs, Action<T> onSuccess = null, Action<UnityWebRequest> error = null) where T : class
    {
        var url = _options.api + webRequestArgs.Url;
        StartCoroutine(PostNumerator<T>(url, webRequestArgs, onSuccess, error));
    }
    public void Post(IPostRequest webRequestArgs, Action<string> onSuccess = null, Action<UnityWebRequest> error = null)
    {
        Post<string>(webRequestArgs, onSuccess, error);
    }
    public void Get<T>(IGetRequest getArgs, Action<T> onSuccess = null, Action<UnityWebRequest> error = null) where T : class
    {
        var url = _options.api + getArgs.Url;
        StartCoroutine(GetNumerator<T>(url, onSuccess, error));
    }
    public void Get(IGetRequest getArgs, Action<object> onSuccess = null, Action<UnityWebRequest> error = null)
    {
        Get<string>(getArgs, onSuccess, error);
    }
    #endregion

    #region Numerators
    private IEnumerator PostNumerator<T>(string url, IPostRequest body, Action<T> onSuccess, Action<UnityWebRequest> error)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, body.ToForm()))
        {
            yield return www.SendWebRequest();

            if (HandleError(www)) // Error
            {
                if (error != null)
                    error(www);
            }
            else // success
            {
                string responseData = www.downloadHandler.text;
                Log("Post " + url + " response: " + responseData);

                if (typeof(T) == typeof(string))
                {
                    if (onSuccess != null)
                        onSuccess((T)(object)responseData);
                }
                else
                {
                    var response = JsonUtility.FromJson<T>(responseData);

                    // Call Success action
                    if (onSuccess != null)
                        onSuccess(response);

                }
            }
            www.Dispose();
        }

    }

    private IEnumerator GetNumerator<T>(string url, Action<T> onSuccess, Action<UnityWebRequest> error = null)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (HandleError(www)) // Error
        {
            if (error != null)
                error(www);
        }
        else
        {

            string responseData = www.downloadHandler.text;
            Log("Get " + url + " response: " + responseData);


            if (typeof(T) == typeof(string))
            {
                if (onSuccess != null)
                    onSuccess((T)(object)responseData);
            }
            else
            {
                var response = JsonUtility.FromJson<T>(responseData);

                // Call Success action
                if (onSuccess != null)
                    onSuccess(response);
            }

        }
        www.Dispose();

    }


    #endregion

    #region Error Handling
    private bool HandleError(UnityWebRequest req)
    {
        if (req.result != UnityWebRequest.Result.Success || req.result == UnityWebRequest.Result.ProtocolError)
        {
            LogError($"Http Request failed. url: {req.url} error: {req.error} {req.downloadHandler.text}");

            return true;
        }

        return false;
    }

    #endregion

    #region Loging
    private void Log(string msg)
    {
        if (!_options.Log) return;
        Debug.Log(msg);
    }
    private void LogError(string msg)
    {
        if (!_options.Log) return;
        Debug.LogError(msg);
    }

    private void LogWarning(string msg)
    {
        if (!_options.Log) return;
        Debug.LogWarning(msg);
    }
    #endregion

}
