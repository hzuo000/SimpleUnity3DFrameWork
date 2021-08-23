using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;



public class RecordManager : GameInterface
{
    public SystemRecord systemRecord;
    private string saveFilePath; //存档路径
    public override void StartUp()
    {
        SetPath(); //设置存档路径
        LoadRecord();

        base.StartUp();
    }
    private void SetPath() //设置存档路径
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "record.dat");
    }
    public void DeleteRecord() //删档
    {
        File.Delete(saveFilePath);
        if (File.Exists(saveFilePath))
        {
            Debug.LogWarning("删除存档失败！");
        }
        else
        {
            Debug.Log("删除存档成功");
        }
    }
    /// <summary>
    /// 读档
    /// </summary>
    private void LoadRecord()//读档
    {
        if (File.Exists(saveFilePath))//有存档
        {
            FileStream stream = File.Open(saveFilePath, FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                systemRecord = (SystemRecord)formatter.Deserialize(stream);
                Debug.Log("加载存档成功");
            }
            catch (SerializationException e)
            {
                Debug.LogError("加载存档失败！");
                throw e;
            }
            finally
            {
                stream.Close();
            }
            CheckRecord(ref systemRecord);
        }
        else//没有存档，新建存档
        {
            Debug.Log("新建存档……");
            systemRecord = new SystemRecord();
            SaveRecord();
        }
    }
    /// <summary>
    /// 写存档
    /// </summary>
    public void SaveRecord()
    {
        FileStream stream = File.Create(saveFilePath);
        BinaryFormatter formatter = new BinaryFormatter();
        try
        {
            formatter.Serialize(stream, systemRecord);
            Debug.Log("写存档成功");
        }
        catch (SerializationException e)
        {
            Debug.LogError("写存档失败！");
            throw e;
        }
        finally
        {
            stream.Close();
        }
    }
    /// <summary>
    /// 存档校验【主要处理之前存档没有，现在新增的引用类型字段】
    /// </summary>
    /// <param name="record"></param>
    public void CheckRecord(ref SystemRecord record)
    {
 
    }

}
