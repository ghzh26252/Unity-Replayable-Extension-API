using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class DataPair<T1, T2>
{
    [SerializeField]
    List<T1> list1 = new List<T1>();
    [SerializeField]
    List<T2> list2 = new List<T2>();

    public List<T1> List1
    {
        get
        {
            return new List<T1>(list1);
        }
    }
    public List<T2> List2
    {
        get
        {
            return new List<T2>(list2);
        }
    }
    int Count => list1.Count;
    public T1 Get(T2 t2)
    {
        if (Contains(t2))
        {
            int index = list2.IndexOf(t2);
            return list1[index];
        }
        else
            return default;
    }
    public T2 Get(T1 t1)
    {
        if (Contains(t1))
        {
            int index = list1.IndexOf(t1);
            return list2[index];
        }
        else
            return default;
    }
    public void Add(T1 t1, T2 t2)
    {
        if (!Contains(t1) || Contains(t2))
        {
            list1.Add(t1);
            list2.Add(t2);
        }
        else
            Debug.LogError("添加失败，数据重复");
    }
    public void AddRange(DataPair<T1, T2> pairs)
    {
        foreach (var item in pairs)
        {
            Add(item.Key, item.Value);
        }
    }
    public void Remove(T1 t1)
    {
        int i = list1.IndexOf(t1);
        list1.RemoveAt(i);
        list2.RemoveAt(i);
    }
    public void Remove(T2 t2)
    {
        int i = list2.IndexOf(t2);
        list1.RemoveAt(i);
        list2.RemoveAt(i);
    }
    public void Clear()
    {
        list1.Clear();
        list2.Clear();
    }
    public bool Contains(T1 t1)
    {
        return list1.Contains(t1);
    }
    public bool Contains(T2 t2)
    {
        return list2.Contains(t2);
    }
    public void Optimize(T1[] t1)
    {
        Optimize(new List<T1>(t1));
    }
    public void Optimize(T2[] t2)
    {
        Optimize(new List<T2>(t2));
    }
    public void Optimize(List<T1> t1)
    {
        for (int i = 0; i < list1.Count; i++)
        {
            if (!t1.Contains(list1[i]))
            {
                list1.RemoveAt(i);
                list2.RemoveAt(i);
            }
        }
    }
    public void Optimize(List<T2> t2)
    {
        for (int i = 0; i < list2.Count; i++)
        {
            if (!t2.Contains(list2[i]))
            {
                list1.RemoveAt(i);
                list2.RemoveAt(i);
            }
        }
    }

    public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
        {
            yield return new KeyValuePair<T1, T2>(list1[i], list2[i]);
        }
    }
}
