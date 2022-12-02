using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Reflection;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public static class PackageDownloadManager
{
    public const string baseUrl = "https://pmapi.cubegame.studio";
    public static bool Login(string password)
    {
        if (LoginHandler.CheckPermission(password, out var auth))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    
    public static void DownloadFile(string loginKey, string packageId, string folderName, string packageName, string mimeType, Action onComplete)
    {
        if (LoginHandler.CheckPermission(loginKey, out var auth))
        {
            string url = $"{baseUrl}/api/ReturnFile/{auth}/{packageId}/{mimeType}";
            DownloadAsyncSetup(url,
                Application.dataPath + $"/Downloaded Packages/{folderName}", (s, a) => onComplete.Invoke());
        }
        else
        {
            Debug.Log("WRONG PASSWORD");
        }
        
       
    }

    public static List<Package> GetPackagesJson(string key)
    {
        string url = $"{baseUrl}/api/GetPackagesJson/" + key;

        List<Package> packages = GetRequestHelper<List<Package>>(url);
        return packages;
    }
    
    private static void DownloadAsyncSetup(string url, string path, Action<object, AsyncCompletedEventArgs> onComplete)
    {
        using (WebClient wc = new WebClient())
        {
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(onComplete);
            wc.DownloadFileAsync(new Uri(url), path);
        }
    }
    public static T GetRequestHelper<T>(string url)
    {
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse) request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        T obj = JsonConvert.DeserializeObject<T>(json);
        return obj;
    }

  
}