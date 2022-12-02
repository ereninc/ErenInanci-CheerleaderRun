using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;

public class MaterialEditorWindow : EditorWindow
{
    static MaterialEditorWindow Window;
    VisualElement activeTab;
    List<VisualElement> tabs;
    List<Button> buttons;
    MaterialCreator materialCreator;
    ModelController modelController;

    [MenuItem("Cube Games/Material Editor")]
    static void show()
    {
        Window = GetWindow<MaterialEditorWindow>();
        Window.Show();
       
    }

    private void OnEnable()
    {
        var materialWindowUXML = Resources.Load<VisualTreeAsset>("MaterialEditorWindow");
        rootVisualElement.styleSheets.Add(Resources.Load<StyleSheet>("DefaultStyle"));

        materialWindowUXML.CloneTree(rootVisualElement);
        initalizeWindow();
    }

    private void OnSelectionChange()
    {
        if (materialCreator != null)
            materialCreator.OnSelectionChange();

        if (modelController != null)
            modelController.OnSelectionChange();
    }


    private void initalizeWindow()
    {
        tabs = new List<VisualElement>();
        buttons = new List<Button>();

        tabs.Add(rootVisualElement.Q<VisualElement>("MaterialCreateView"));
        tabs.Add(rootVisualElement.Q<VisualElement>("MaterialSetView"));
        activeTab = tabs[0];

        buttons = rootVisualElement.Query<Button>().ToList();
        for (int i = 0; i < buttons.Count; i++)
        {
            switch (buttons[i].name)
            {
                case "btnMaterialCreateView":
                    buttons[i].clicked += () => ChangeTab("MaterialCreateView");
                    break;
                case "btnMaterialSetView":
                    buttons[i].clicked += () => ChangeTab("MaterialSetView");
                    break;
                default:
                    break;
            }
        }

        materialCreator = new MaterialCreator();
        materialCreator.Initialize(rootVisualElement);

        modelController = new ModelController();
        modelController.Initialize(rootVisualElement);

        ChangeTab("MaterialCreateView");
    }

    public void ChangeTab(string id)
    {
        buttons.Find(x => x.name == "btn" + activeTab.name).SetEnabled(true);
        hideElement(activeTab);
        activeTab = tabs.Find(x => x.name == id);
        showElement(activeTab);
        buttons.Find(x => x.name == "btn" + activeTab.name).SetEnabled(false);
        OnSelectionChange();
    }

    private void showElement(VisualElement element)
    {
        element.style.display = DisplayStyle.Flex;
        element.style.visibility = Visibility.Visible;
    }

    private void hideElement(VisualElement element)
    {
        element.style.display = DisplayStyle.None;
        element.style.visibility = Visibility.Hidden;
    }

    public class MaterialCreator
    {
        public List<TextureFieldModel> Textures;
        ObjectField allMaterialShader;
        TextField txtPath;
        VisualElement textureSample;
        Label lblEmpty;
        Button btnSetAllMaterials, btnCreateMaterials;
        ListView listView;
        VisualElement root;

        public void Initialize(VisualElement root)
        {
            this.root = root;
            allMaterialShader = root.Query<ObjectField>("allShaderField");
            txtPath = root.Query<TextField>("txtMatSavePath");
            btnSetAllMaterials = root.Query<Button>("btnSetAllShader");
            btnCreateMaterials = root.Query<Button>("btnCreateMaterials");
            lblEmpty = root.Query<Label>("lblEmptyChilds");
            textureSample = root.Query<VisualElement>("TextureSample");
            listView = root.Query<ListView>("texturesList");
       
            textureSample.style.visibility = Visibility.Hidden;
            textureSample.style.display = DisplayStyle.None;

            allMaterialShader.objectType = typeof(Shader);
            allMaterialShader.value = Shader.Find("Universal Render Pipeline/Lit");

            btnCreateMaterials.clicked += () => CreateMaterials();
            btnSetAllMaterials.clicked += () => UpdateAllAMaterialShader((Shader)allMaterialShader.value);

            OnSelectionChange();
        }

        public void OnSelectionChange()
        {
            if (Textures == null)
                Textures = new List<TextureFieldModel>();

            ClearTextures();


            if (Selection.assetGUIDs.Length > 0)
            {
                for (int i = 0; i < Selection.objects.Length; i++)
                {
                    Object obj = Selection.objects[i];
                    if (obj.GetType() == typeof(Texture2D))
                    {
                        AddTexture(Path.GetFileName(Path.GetDirectoryName(AssetDatabase.GetAssetPath(obj))), (Texture2D)obj);
                    }
                }
            }

            if (Textures.Count > 0)
            {
                lblEmpty.style.display = DisplayStyle.None;
                lblEmpty.style.visibility = Visibility.Hidden;
            }
            else
            {
                lblEmpty.style.display = DisplayStyle.Flex;
                lblEmpty.style.visibility = Visibility.Visible;
            }
        }

        public void AddTexture(string path, Texture2D tex)
        {
            VisualElement texSample = new VisualElement();
            texSample.AddToClassList("textureSample");
       
            ObjectField texField = new ObjectField(tex.name);
            texField.objectType = typeof(Texture2D);
            texField.AddToClassList("textureSample-textureField");

            ObjectField shaderField = new ObjectField("Shader");
            shaderField.objectType = typeof(Shader);
            shaderField.AddToClassList("textureSample-shaderField");

            texSample.Add(texField);
            texSample.Add(shaderField);

            TextureFieldModel fieldModel = new TextureFieldModel(path, texSample, texField, shaderField, tex);
            Textures.Add(fieldModel);
            listView.hierarchy.Add(texSample);
        }

        public void ClearTextures()
        {
            for (int i = 0; i < Textures.Count; i++)
            {
                listView.hierarchy.Remove(Textures[i].Element);
            }

            Textures.Clear();
        }

        public void UpdateAllAMaterialShader(Shader shader)
        {
            foreach (var item in Textures)
            {
                item.ShaderField.value = shader;
            }
        }

        public void CreateMaterials()
        {
            string folderPath = Application.dataPath + "/" + txtPath.text;
            if (Directory.Exists(folderPath) == false)
            {
                Directory.CreateDirectory(folderPath);
            }

            foreach (var item in Textures)
            {
                if (Directory.Exists(folderPath + "/" + item.TexturePath) == false)
                {
                    Directory.CreateDirectory(folderPath + "/" + item.TexturePath);
                }

                Material material = new Material((Shader)item.ShaderField.value);
                material.mainTexture = (Texture2D)item.TextureField.value;
                if (material.shader == Shader.Find("Universal Render Pipeline/Lit"))
                {
                    material.SetFloat("_Smoothness", 0);
                    material.SetFloat("_SpecularHighlights", 0);
                    material.SetFloat("_EnvironmentReflections", 0);
                }

                AssetDatabase.CreateAsset(material, "Assets/" + txtPath.text + "/" + item.TexturePath + "/" + item.Name + ".mat");
            }

            AssetDatabase.Refresh();
        }

    }

    public class ModelController
    {
        public List<SharedMaterialModel> SharedMaterials;
        VisualElement root;
        ListView rendererList;
        Button btnUpdateMaterials, btnFindMaterials;
        Label lblEmptyRenderers;

        public void Initialize(VisualElement root)
        {
            this.root = root;
            SharedMaterials = new List<SharedMaterialModel>();
            rendererList = root.Query<ListView>("rendererList");
            lblEmptyRenderers = root.Query<Label>("lblEmptyRenderers");
            btnUpdateMaterials = root.Query<Button>("btnUpdateModel");
            btnFindMaterials = root.Query<Button>("btnFindMaterials");
            btnUpdateMaterials.clicked += updateMaterials;
            btnFindMaterials.clicked += findMaterials;
        }

        public void OnSelectionChange()
        {
            Clear();
            if (Selection.activeObject == null)
            {
                lblEmptyRenderers.style.visibility = Visibility.Visible;
                lblEmptyRenderers.style.display = DisplayStyle.Flex;
            }
            else
            {
                for (int i = 0; i < Selection.gameObjects.Length; i++)
                {
                    checkChilds(Selection.gameObjects[i].transform);
                }

                if (SharedMaterials.Count == 0)
                {
                    lblEmptyRenderers.style.visibility = Visibility.Visible;
                    lblEmptyRenderers.style.display = DisplayStyle.Flex;
                }
                else
                {
                    lblEmptyRenderers.style.visibility = Visibility.Hidden;
                    lblEmptyRenderers.style.display = DisplayStyle.None;

                    btnFindMaterials.style.visibility = Visibility.Visible;
                    btnFindMaterials.style.display = DisplayStyle.Flex;

                    btnUpdateMaterials.style.visibility = Visibility.Visible;
                    btnUpdateMaterials.style.display = DisplayStyle.Flex;
                }
            }
        }

        public void Clear()
        {
            if (SharedMaterials == null)
                SharedMaterials = new List<SharedMaterialModel>();

            for (int i = 0; i < SharedMaterials.Count; i++)
            {
                rendererList.hierarchy.Remove(SharedMaterials[i].Parent);
            }

            SharedMaterials.Clear();
        }

        private void findMaterials()
        {
            string[] paths = AssetDatabase.FindAssets("t: Material");

            for (int i = 0; i < SharedMaterials.Count; i++)
            {
                Material mat = ((Material)SharedMaterials[i].MaterialField.value);
                if (mat.mainTexture != null)
                {
                    if (paths.Length > 0)
                    {
                        string path = "";

                        for (int k = 0; k < paths.Length; k++)
                        {
                            string value = AssetDatabase.GUIDToAssetPath(paths[k]);
                            if (value.Contains(mat.mainTexture.name))
                            {
                                path = value;
                                break;
                            }
                        }

                        if (path != "")
                        {
                            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
                            if (material != null)
                            {
                                SharedMaterials[i].MaterialField.value = material;
                            }
                        }
                    }
                }
            }
        }

        private void updateMaterials()
        {
            foreach (var item in SharedMaterials)
            {
                item.UpdateMaterials();
            }
        }

        private void checkChilds(Transform parent)
        {
            if (parent.GetComponent<Renderer>() != null)
            {
                addRenderer(parent.GetComponent<Renderer>());
            }

            for (int i = 0; i < parent.childCount; i++)
            {
                if (parent.GetChild(i).GetComponent<Renderer>() != null)
                {
                    addRenderer(parent.GetChild(i).GetComponent<Renderer>());
                }

                checkChilds(parent.GetChild(i));
            }
        }

        private void addRenderer(Renderer renderer)
        {
            for (int i = 0; i < renderer.sharedMaterials.Length; i++)
            {
                if (renderer.sharedMaterials[i] != null)
                {
                    SharedMaterialModel model = SharedMaterials.Find(x => x.MaterialField.value.name == renderer.sharedMaterials[i].name);
                    if (model != null)
                    {
                        model.AddRenderer(renderer, i);
                    }
                    else
                    {
                        model = new SharedMaterialModel(renderer.sharedMaterials[i], rendererList);
                        model.AddRenderer(renderer, i);
                        SharedMaterials.Add(model);
                    }
                }
            }
        }
    }

    public class TextureFieldModel
    {
        public VisualElement Element;
        public ObjectField TextureField;
        public ObjectField ShaderField;
        public string TexturePath;
        public string Name;

        public TextureFieldModel(string path, VisualElement element, ObjectField texField, ObjectField shaderField, Texture2D tex)
        {
            TexturePath = path;
            Name = tex.name;
            Element = element;
            TextureField = texField;
            ShaderField = shaderField;
            TextureField.value = tex;
            ShaderField.value = Shader.Find("Universal Render Pipeline/Lit");

        }
    }

    public class SharedMaterialModel
    {
        public List<RendererViewModel> RendererViews;
        public VisualElement Parent;
        public Foldout RenderersView;
        public ScrollView RenderersParent;
        public ObjectField MaterialField;


        public SharedMaterialModel(Material mat, VisualElement root)
        {
            Parent = new VisualElement();
            MaterialField = new ObjectField("Shared Material");
            MaterialField.objectType = typeof(Material);
            MaterialField.value = mat;
            RenderersView = new Foldout();
            RenderersView.text = "Renderers";
            RenderersView.value = false;
            RenderersView.style.minHeight = 100;
            Parent.AddToClassList("materialSample");
            RenderersParent = new ScrollView();
            RenderersParent.horizontalPageSize = 100;
            RenderersView.Add(RenderersParent);

            Parent.Add(MaterialField);
            Parent.Add(RenderersView);


            root.hierarchy.Add(Parent);
        }

        public void AddRenderer(Renderer renderer, int index)
        {
            if (RendererViews == null)
                RendererViews = new List<RendererViewModel>();

            if (RendererViews.Find(x => x.Renderer == renderer) == null)
            {
                RendererViewModel rendererView = new RendererViewModel(renderer, RenderersParent.contentContainer, index);
                RendererViews.Add(rendererView);
            }
        }

        public void UpdateMaterials()
        {
            if (RendererViews != null)
            {
                foreach (var item in RendererViews)
                {
                    item.UpdateMaterial((Material)MaterialField.value);
                }
            }
        }
    }

    public class RendererViewModel
    {
        public Renderer Renderer;
        public VisualElement Parent;
        public ObjectField RenderField;
        public Button btnSelect;
        public int Index;
        VisualElement root;

        public RendererViewModel(Renderer renderer, VisualElement root, int index)
        {
            this.root = root;
            Index = index;
            Renderer = renderer;
            Parent = new VisualElement();
            Parent.style.flexDirection = FlexDirection.Row;
            RenderField = new ObjectField(renderer.gameObject.name);
            RenderField.objectType = typeof(Renderer);
            RenderField.value = renderer;
            Parent.Add(RenderField);
            btnSelect = new Button(SelectObject);
            btnSelect.text = "Select";
            Parent.Add(btnSelect);

            root.hierarchy.Add(Parent);
        }

        public void SelectObject()
        {
            EditorGUIUtility.PingObject(Renderer.gameObject);
        }

        public void UpdateMaterial(Material mat)
        {
            Material[] materials = Renderer.sharedMaterials;
            materials[Index] = mat;

            Renderer.sharedMaterials = materials;
        }
    }
}
