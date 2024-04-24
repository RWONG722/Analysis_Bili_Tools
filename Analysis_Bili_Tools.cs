/**
Script Name: Analysis_Bili_Tools
Version: 1.2
Description: B站解析工具
modified: 2024/04/25
Author: Raymond_OuO
https://github.com/RWONG722/Analysis_Bili_Tools
**/

using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class Analysis_Bili_Tools : UdonSharpBehaviour
{

    [Header("解析链接组成：解析地址 + B站URL + BV号 + 后缀")]

    [Header("解析地址")]
    [SerializeField] private string AnalysisApi;

    [Header("B站URL")]
    [SerializeField] private string Prefix;

    [Header("后缀")]
    [SerializeField] private string Suffix;

    [Header("輸入框")]
    public InputField Input;

    void Start()
    {
        if (AnalysisApi == "")
        {
            AnalysisApi = "https://api.rwit.net/jx/?url=";
        }
        if (Prefix == "")
        {
            Prefix = "https://www.bilibili.com/video/";
        }
    }
                
    public void OnInput()
    {
        if (Input.text.Contains("BV") && Input.text.Length > 11)
        {
            string BV = Input.text.Substring(Input.text.IndexOf("BV"), 12);
            Input.text = AnalysisApi + Prefix + BV + Suffix;
        }
        else if (Input.text.Contains("music.163.com") || Input.text.Contains("live.bilibili.com"))
        {   
            Input.text = AnalysisApi + Input.text;
        }
        else
        {
            OnClear();
        }
    }

    public void OnClear()
    {   
        Input.text = "";
    }
}
