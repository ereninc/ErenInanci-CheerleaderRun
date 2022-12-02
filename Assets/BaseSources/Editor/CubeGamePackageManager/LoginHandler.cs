using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;


public class Permission
{
    public bool HavePass { get; set; }
    public string AuthKey { get; set; }
}


public static class LoginHandler
{


    public static bool CheckPermission(string key, out string auth)
    {
        Permission perm = PackageDownloadManager.GetRequestHelper<Permission>($"{PackageDownloadManager.baseUrl}/api/CheckPermission/" + key);
        auth = perm.AuthKey;
        return perm.HavePass;
    }



}