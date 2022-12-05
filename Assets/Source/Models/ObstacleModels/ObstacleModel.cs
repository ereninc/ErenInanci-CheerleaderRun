using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleModel : ObjectModel
{
    public ObstacleDataModel GetDataModel()
    {
        ObstacleDataModel dataModel = new ObstacleDataModel();
        dataModel.Position = transform.position;

        return dataModel;
    }
}
