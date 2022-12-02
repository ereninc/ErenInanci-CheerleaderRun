using System; 

[Serializable] 
public class TutorialDataModel : DataModel 
{ 
    public static TutorialDataModel Data;
    public int index = 0;
    public TutorialDataModel Load() 
    {
        if (Data == null)
        {
            Data = this;
            object data = LoadData();
            if (data != null)
            {
                Data = (TutorialDataModel)data;
            }
        }
        return Data;
    }
    public void Save()
    {
        Save(Data);
    }
}