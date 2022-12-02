#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DocumentationView : EditorWindow
{
    private static DocumentationView documentation;
    private VisualTreeAsset moduleContTemplate;
    private VisualTreeAsset moduleLongDescTemplate;
    private VisualElement body;
    private VisualElement modulesContainer;
    private VisualElement modulesHolder;
    private VisualElement moduleLongDesc;
    private List<ModuleShortDescContainer> moduleShortDescriptions;
    ModuleDescriptionDatas moduleDescriptionDatas;
    private List<ModuleDescription> moduleDescriptions;


    [MenuItem("Cube Games/Documentation")]
    static void ShowWindow()
    {
        documentation = GetWindow<DocumentationView>("Cube Games Template Documentation");
        documentation.minSize = new Vector2(800, 750);
        documentation.Show();
    }

    private void OnEnable()
    {
        var documentationUXML = (VisualTreeAsset) AssetDatabase.LoadAssetAtPath(
            "Assets/CubeGames/Editor/DocumentationPackage/Documentation/Resources/DocumentationView.uxml", typeof(VisualTreeAsset));
        documentationUXML.CloneTree(rootVisualElement);
        moduleContTemplate = (VisualTreeAsset) AssetDatabase.LoadAssetAtPath(
            "Assets/CubeGames/Editor/DocumentationPackage/Documentation/Resources/moduleContainer.uxml", typeof(VisualTreeAsset));
        moduleLongDescTemplate = (VisualTreeAsset) AssetDatabase.LoadAssetAtPath(
            "Assets/CubeGames/Editor/DocumentationPackage/Documentation/Resources/moduleDescContainer.uxml", typeof(VisualTreeAsset));
        ;
        Initialize(rootVisualElement);
    }

    void Initialize(VisualElement root)
    {
        moduleShortDescriptions = new List<ModuleShortDescContainer>();
        moduleDescriptionDatas = new ModuleDescriptionDatas();
        moduleDescriptions = moduleDescriptionDatas.GetDescriptions();
        body = root.Query<VisualElement>("body");
        modulesHolder = root.Query<VisualElement>("modulesHolder");
        modulesContainer = root.Query<VisualElement>("modulesContainer");

        for (int i = 0; i < moduleDescriptions.Count; i++)
        {
            VisualElement module = new VisualElement();
            moduleContTemplate.CloneTree(module);
            ModuleShortDescContainer moduleShortDescContainer = new ModuleShortDescContainer()
            {
                id = i,
                readMoreBtn = module.Query<Button>("btnReadMore"),
                moduleNameLbl = module.Query<Label>("txtModuleName"),
                shortDescLbl = module.Query<Label>("txtModuleShortDesc")
            };


            moduleShortDescContainer.readMoreBtn.clicked += () => ReadMoreButton(moduleShortDescContainer.id);
            moduleShortDescContainer.moduleNameLbl.text = moduleDescriptions[i].moduleName;
            moduleShortDescContainer.shortDescLbl.text = moduleDescriptions[i].shortDescription;
            moduleShortDescriptions.Add(moduleShortDescContainer);
            modulesHolder.Add(module);
        }
    }

    void CreateText(ModuleDescription module)
    {
        Label moduleDesc = moduleLongDesc.Query<Label>("txtLongDesc");
        ScrollView scroll = moduleLongDesc.Query<ScrollView>("scrollLongDesc");
        List<string> strs = ModuleDescriptionHelper.SplitTextBySeperator(module.fullDescription, '{');
        if (strs.Count == 1)
        {
            moduleDesc.text = module.fullDescription;
        }
        else
        {
            //moduleLongDesc.Remove(moduleDesc);

            List<int> imageIncides = ModuleDescriptionHelper.GetImageIndices(strs);
            List<VisualElement> images = new List<VisualElement>();
            for (int i = 0; i < imageIncides.Count; i++)
            {
                VisualElement visualElement = new VisualElement();
                visualElement.style.backgroundImage =
                    UnityEngine.UIElements.Background.FromTexture2D(module.images[imageIncides[i]]);
                visualElement.style.width = Length.Percent(100);
                visualElement.style.minHeight = 250;
                visualElement.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                images.Add(visualElement);
            }

            for (int i = 0; i < strs.Count; i++)
            {
                Label lbl = new Label();
                lbl.name = moduleDesc.name + i.ToString();
                lbl.text = strs[i];
                lbl.style.whiteSpace = WhiteSpace.Normal;
                scroll.Add(lbl);
                if (i < imageIncides.Count)
                {
                    scroll.Add(images[i]);
                }
            }
        }
    }


    void ReadMoreButton(int id)
    {
        moduleLongDesc = new VisualElement();
        moduleLongDesc.style.width = Length.Percent(100);
        moduleLongDesc.style.height = Length.Percent(100);
        moduleLongDescTemplate.CloneTree(moduleLongDesc);
        ScrollView scroll = moduleLongDesc.Query<ScrollView>("scrollLongDesc");
        /*Button backBtn = moduleLongDesc.Query<Button>("btnBack");
        backBtn.clicked += BackBtn_clicked;*/
        while (moduleLongDesc.childCount > 2)
        {
            moduleLongDesc.RemoveAt(2);
        }

        Label moduleName = moduleLongDesc.Query<Label>("txtModuleName");
        // Label moduleDesc = moduleLongDesc.Query<Label>("txtLongDesc");
        moduleName.text = moduleDescriptions[id].moduleName;
        CreateText(moduleDescriptions[id]);
        //moduleDesc.text = moduleDescriptions[id].fullDescription;

        if (moduleDescriptions[id].subPages.Count > 0)
        {
            for (int i = 0; i < moduleDescriptions[id].subPages.Count; i++)
            {
                Button subPageBtn = new Button();
                subPageBtn.text = moduleDescriptions[id].subPages[i].moduleName;
                // subPageBtn.style.maxWidth = 300;
                subPageBtn.style.marginLeft = 10;
                subPageBtn.style.marginRight = 10;
                var subPage = moduleDescriptions[id].subPages[i];
                subPageBtn.clicked += () => ReadMoreBtnForSubpage(subPage, id);
                scroll.Add(subPageBtn);
            }
        }

        Button backBtn = new Button();
        backBtn.text = "Back";
        backBtn.style.marginLeft = 10;
        backBtn.style.marginRight = 10;
        backBtn.clicked += BackBtn_clicked;
        scroll.Add(backBtn);
        //moduleLongDesc.style.display = DisplayStyle.Flex;
        modulesContainer.style.display = DisplayStyle.None;
        body.Add(moduleLongDesc);
    }

    void ReadMoreBtnForSubpage(ModuleDescription subPage, int mainID)
    {
        body.Remove(moduleLongDesc);
        moduleLongDesc = new VisualElement();
        moduleLongDesc.style.width = Length.Percent(100);
        moduleLongDesc.style.height = Length.Percent(100);
        moduleLongDescTemplate.CloneTree(moduleLongDesc);
        ScrollView scroll = moduleLongDesc.Query<ScrollView>("scrollLongDesc");
        Label moduleName = moduleLongDesc.Query<Label>("txtModuleName");
        moduleName.text = subPage.moduleName;
        CreateText(subPage);
        Button backBtn = new Button();
        backBtn.text = "Back";
        backBtn.style.marginLeft = 10;
        backBtn.style.marginRight = 10;
        backBtn.clicked += () =>
        {
            body.Remove(moduleLongDesc);
            ReadMoreButton(mainID);
        };
        scroll.Add(backBtn);
        body.Add(moduleLongDesc);
        while (moduleLongDesc.childCount > 2)
            moduleLongDesc.RemoveAt(2);
    }


    private void BackBtn_clicked()
    {
        body.Remove(moduleLongDesc);
        moduleLongDesc = null;
        modulesContainer.style.display = DisplayStyle.Flex;
    }
}


struct ModuleShortDescContainer
{
    public int id;
    public Button readMoreBtn;
    public Label moduleNameLbl;
    public Label shortDescLbl;
}

#endif