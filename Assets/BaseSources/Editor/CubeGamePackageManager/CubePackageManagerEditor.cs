#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class CubePackageManagerEditor : EditorWindow
{
    static CubePackageManagerEditor Window;
    ToolbarSearchField searchField;
    VisualElement packageInfo;
    ScrollView scrlViewPackageNames;
    VisualElement buttonsParent;
    VisualElement loginWindow;
    VisualElement packagesElement;
    VisualElement info;
    VisualElement downloading;
    Button btnDownload;
    Button btnRemove;
    Button btnDocumentation;
    Button btnLogin;
    Label packageName;
    Label versionInfo;
    Label packageDef;
    TextField passwordField;
    private Package selectedPck;
    private List<Package> packages;

    [MenuItem("Cube Games/Package Manager")]
    static void ShowWindow()
    {
        Window = GetWindow<CubePackageManagerEditor>();
        Window.minSize = new Vector2(700, 500);
        Window.maxSize = new Vector2(1000, 600);
        Window.Show();
    }

    private void OnEnable()
    {
        var cubePackageManagerUXML = Resources.Load<VisualTreeAsset>("CubePackageManager");
        cubePackageManagerUXML.CloneTree(rootVisualElement);
        Initialize(rootVisualElement);
    }

    private void Initialize(VisualElement root)
    {
        SetUIElements(root);
        LoadLoginButtonEvent();
        LoadDownloadButtonEvent();
        LoadInstallButtonEvent();
        LoadSearchEvent();
        CheckSavedPassword();
    }

    private void LoadInstallButtonEvent()
    {
        btnRemove.clicked += () =>
        {
            PackageFolderHandler.RemovePackage(selectedPck);
            SetButtonsDisplay(selectedPck);
            //  Debug.Log(Path.Combine(Application.streamingAssetsPath, selectedPck.FolderName));
        };
    }

    private void LoadDownloadButtonEvent()
    {
        btnDownload.clicked += () =>
        {
            PackageFolderHandler.AssetFolderExist();
            downloading.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            info.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            packagesElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            PackageDownloadManager.DownloadFile(EditorPrefs.GetString("packageManagerPassword"), selectedPck.Id.ToString(), selectedPck.FolderName,
                selectedPck.Name, "application*x-gzip", () =>
                {
                    // selectedPck.IsDownloaded = true;
                    PackageButtonClicked(selectedPck);
                    downloading.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
                    info.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                    packagesElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                    if (selectedPck.FolderName.Contains(".unitypackage"))
                    {
                        PackageFolderHandler.InstallPackage(selectedPck, true);
                    }
                    else
                    {
                        PackageFolderHandler.ExtractPackage(selectedPck);
                    }

                    AssetDatabase.Refresh();
                    // Debug.Log(selectedPck.FolderName);
                });
        };
    }

    private void CheckSavedPassword()
    {
        if (EditorPrefs.HasKey("packageManagerPassword"))
            if (PackageDownloadManager.Login(EditorPrefs.GetString("packageManagerPassword")))
            {
                loginWindow.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
                info.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                packagesElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                LoadPackages(EditorPrefs.GetString("packageManagerPassword"));
            }
            else
                UnityEditor.EditorUtility.DisplayDialog("Password Check", "Wrong Password", "Ok");
    }

    private void LoadLoginButtonEvent()
    {
        btnLogin.clicked += () =>
        {
            if (PackageDownloadManager.Login(passwordField.value))
            {
                EditorPrefs.SetString("packageManagerPassword", passwordField.value);
                loginWindow.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
                info.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                packagesElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                LoadPackages(passwordField.value);
            }
            else
                UnityEditor.EditorUtility.DisplayDialog("Password Check", "Wrong Password", "Ok");
        };
    }

    private void LoadPackages(string pw)
    {
        packages = PackageDownloadManager.GetPackagesJson(pw);
        foreach (var package in packages)
        {
            PackageButton button = new PackageButton();
            button.text = package.Name;
            button.package = package;
            button.clicked += () => PackageButtonClicked(button.package);
            scrlViewPackageNames.Add(button);
        }
    }

    private void SetUIElements(VisualElement root)
    {
        searchField = root.Query<ToolbarSearchField>("searchField");
        packageInfo = root.Query<VisualElement>("packageInfo");
        loginWindow = root.Query<VisualElement>("loginWindow");
        packagesElement = root.Query<VisualElement>("packages");
        downloading = root.Query<VisualElement>("downloading");
        info = root.Query<VisualElement>("info");
        scrlViewPackageNames = root.Query<ScrollView>("scrlViewPackageNames");
        buttonsParent = root.Query<VisualElement>("buttons");
        btnRemove = root.Query<Button>("btnRemove");
        btnDownload = root.Query<Button>("btnDownload");
        btnDocumentation = root.Query<Button>("btnDocumentation");
        btnLogin = root.Query<Button>("loginBtn");
        packageName = root.Query<Label>("packageName");
        packageDef = root.Query<Label>("packageDef");
        versionInfo = root.Query<Label>("versionInfo");
        passwordField = root.Query<TextField>("passwordField");
    }

    private void PackageButtonClicked(Package package)
    {
        // AssetDownloadCheck(package);
        SetButtonsDisplay(package);
        packageName.text = package.Name;
        versionInfo.text = package.Version;
        packageDef.text = package.Description;
        selectedPck = package;
    }

    private void SetButtonsDisplay(Package package)
    {
        buttonsParent.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
        packageInfo.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
        if (PackageFolderHandler.AssetDownloadCheck(package))
        {
            Debug.Log("Package exist");
            btnDownload.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            btnRemove.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
        }
        else
        {
            btnDownload.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            btnRemove.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        }
        /*
        if (PackageFolderHandler.AssetDownloadCheck(package) && package.FolderName.Contains(".unitypackage"))
        {
            btnDownload.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            btnRemove.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
        }
        else if (PackageFolderHandler.AssetDownloadCheck(package) && !package.FolderName.Contains(".unitypackage"))
        {
            btnRemove.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            btnDownload.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        }
        else
        {
            btnDownload.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            btnRemove.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        }*/
    }


    private void LoadSearchEvent()
    {
        searchField.RegisterValueChangedCallback(x =>
        {
            {
                if (x.newValue == String.Empty)
                {
                    for (int i = 0; i < packages.Count; i++)
                    {
                        scrlViewPackageNames[i].style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                    }
                }
                else
                {
                    for (var i = 0; i < packages.Count; i++)
                    {
                        if (packages[i].Name.ToLower().Contains(x.newValue.ToLower()))
                        {
                            scrlViewPackageNames[i].style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                        }
                        else
                            scrlViewPackageNames[i].style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
                    }
                }
            }
        });
    }
}
#endif