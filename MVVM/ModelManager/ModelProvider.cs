using System;
using System.Collections;
using System.Collections.Generic;
using UI.MVVM.Models;
using UnityEngine;


namespace MVVM
{
    public static class ModelProvider
    {
        private static Dictionary<Type, Func<IModel>> modelCreators;

        static ModelProvider()
        {
            Init();
        }

        private static void Init()
        {
            modelCreators = new Dictionary<Type, Func<IModel>>();
            // 在此处初始化所有实现了IModel接口的model类型的数据
            // 例如：
            modelCreators.Add(typeof(Weapon_Model), ()=> new Weapon_Model());
            modelCreators.Add(typeof(Fitting_Model), ()=> new Fitting_Model());
            modelCreators.Add(typeof(Charactor_Model), () => new Charactor_Model());
        }

        public static T GetUniqueModel<T>() where T : class, IModel
        {
            if (modelCreators.TryGetValue(typeof(T), out var creator))
            {
                return creator() as T;
            }
            Debug.LogErrorFormat("Model of type {0} not found!", typeof(T).Name);
            return null;
        }
    }
}
