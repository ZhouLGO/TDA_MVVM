using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class Asset_Saver_Loader
{
    /// <summary>
    /// 将数据源对象转换为Json字符串并保存到Application.persistentDataPath路径下
    /// </summary>
    /// <param name="dataSourceObj">可被序列化的数据源</param>
    /// <param name="streamingAssetsSavePath">目标保存位置（Application.persistentDataPath的子级文件夹）</param>
    public static void Save_JsonToPrsistentPath(object dataSourceObj, string streamingAssetsSavePath)
    {
        string json = JsonUtility.ToJson(dataSourceObj, true);
        string targetSavePath = Path.Combine(Application.persistentDataPath, streamingAssetsSavePath);
        
        // 创建目录
        string directoryPath = Path.GetDirectoryName(targetSavePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        using (StreamWriter sw = new StreamWriter(targetSavePath))
        {
            sw.WriteLine(json);
            sw.Close();
        }
    }
    
    /// <summary>
    /// 从Application.persistentDataPath路径下读取Json文件并转换为数据源对象
    /// </summary>
    /// <param name="sourceDestination">被加载资源的目的地</param>
    /// <param name="streamingAssetsSavePath">Application.persistentDataPath路径下的文件</param>
    /// <typeparam name="T">反序列化后的目标类型</typeparam>
    /// <returns>加载成功返回true，失败返回false</returns>
    public static bool Load_FromPersistentPath<T>(ref T sourceDestination, string streamingAssetsSavePath)
    {
        string targetReadPath =  Path.Combine(Application.persistentDataPath, streamingAssetsSavePath);
        if (!File.Exists(targetReadPath)) return false;
        
        string json;
        using (StreamReader sr = new StreamReader(targetReadPath))
        {
            json = sr.ReadToEnd();
        }
        sourceDestination = JsonUtility.FromJson<T>(json);
        return sourceDestination != null;
    }

    /// <summary>
    /// 从Resources文件夹下加载资源
    /// </summary>
    /// <param name="destinationObj">被加载资源的目的地</param>
    /// <param name="path">Resources路径下的文件</param>
    /// <typeparam name="T">反序列化后的目标类型</typeparam>
    /// <returns>加载成功返回true,失败返回false</returns>
    public static bool Load_FromResourcesPath<T>(ref T destinationObj,string path) where T : UnityEngine.Object
    {
        if (destinationObj == null)
            destinationObj = Resources.Load<T>(path);
        return destinationObj!= null;
    }

    //因为AB包寄了所以就注释掉不用了（想用来加载配置SO的）
    //public static async Task<T> AS_Load_Async<T>(string AddressableSourceName) where T : class
    //{
    //    AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(AddressableSourceName);
    //    try
    //    {
    //        return await handle.Task;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
}
