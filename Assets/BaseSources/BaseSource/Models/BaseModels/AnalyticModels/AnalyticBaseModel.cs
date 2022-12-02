using System;
using UnityEngine;

namespace CubeGames.Analytic
{
    public class AnalyticBaseModel : ObjectModel
    {
        public virtual void OnLevelStarted(int level)
        {

        }

        public virtual void OnLevelFailed(int level)
        {

        }

        public virtual void OnLevelCompleted(int level)
        {

        }

        public virtual void SendCustomEvent(string key, object value)
        {

        }

        public virtual string DefinationSymbol()
        {
            return "";
        }

        public virtual void OnAddAnalytic()
        {

        }

        public virtual void RemoveAnalytic()
        {
            GetComponent<AnalyticController>().RemoveAnalytic(this);
        }

        public virtual void E_Editor()
        {
          
        }
    }
}