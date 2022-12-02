using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;

[Serializable]
public class Package
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string FolderName { get; set; }
    public string Version { get; set; }
    public string Description { get; set; }
  //  public bool IsDownloaded { get; set; }
}   


public class PackageButton : Button
{
    public Package package;
   // public string downloadLink;
}