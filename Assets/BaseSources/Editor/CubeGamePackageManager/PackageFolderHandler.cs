using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public static class PackageFolderHandler
{
    public const string downloadedFolder = "Downloaded Packages";

    public static void InstallPackage(Package package, bool interactive)
    {
        AssetDatabase.Refresh();
        var path = GetAssetPath(package.FolderName);
        AssetDatabase.importPackageCompleted += OnPackageImported;
        AssetDatabase.ImportPackage(path, interactive);
        //      AssetDatabase.DeleteAsset(path);
        //     AssetDatabase.Refresh();
    }

    private static void OnPackageImported(string name)
    {
        Debug.Log("Name =" + name);
        AssetDatabase.importPackageCompleted -= OnPackageImported;
    }

    private static string GetAssetPath(string packageFolderName)
    {
        return $"Assets/{downloadedFolder}/{packageFolderName}";
    }

    public static void AssetFolderExist()
    {
        if (!AssetDatabase.IsValidFolder($"Assets/{downloadedFolder}"))
        {
            AssetDatabase.CreateFolder("Assets", downloadedFolder);
        }


        AssetDatabase.Refresh();
    }


    public static bool AssetDownloadCheck(Package package)
    {
        var guid = AssetDatabase.AssetPathToGUID(GetAssetPath(package.FolderName));

        return File.Exists(GetAssetPath(package.FolderName));
        return !string.IsNullOrEmpty(guid);
    }

    public static void ExtractPackage(Package selectedPck)
    {
    }

    public static void RemovePackage(Package selectedPck)
    {
        if (!AssetDatabase.IsValidFolder(GetAssetPath(selectedPck.FolderName)))
        {
            AssetDatabase.DeleteAsset(GetAssetPath(selectedPck.FolderName));
        }

        AssetDatabase.Refresh();
    }
}