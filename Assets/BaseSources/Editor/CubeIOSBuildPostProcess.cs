using System.IO;
using UnityEngine;

#if UNITY_EDITOR_OSX
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;
#endif

#if UNITY_EDITOR_OSX
public class CubeIOSBuildPostProcess
{

    [PostProcessBuildAttribute(1)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
        if (target == BuildTarget.iOS)
        {
            ///PLISTS
            /// Read plist
            var plistPath = Path.Combine(path, "Info.plist");
            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            /// Update value

            PlistElementDict root = plist.root;
            root.SetString("NSUserTrackingUsageDescription", "Your data will only be used to deliver personalized ads to you");

            /// Write plist
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }

}
#endif