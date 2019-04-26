using Fixi3d.Utilities;
using System.IO;
using UnityEngine;

public class EditorTester : Singleton<EditorTester>
{
    public bool loadProject;
    public string projectToLoad;

    public bool saveProject;

    private void Update()
    {
        if(loadProject)
        {
            loadProject = false;
            var text = UnityEngine.Resources.Load(projectToLoad) as TextAsset;
            var json = text.text;
            WallCreator.Instance.LoadProject(json);
        }

        if(saveProject)
        {
            saveProject = false;
            var json = WallCreator.Instance.ProjectToJson();
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "Project.json"), json);
        }
    }
}
