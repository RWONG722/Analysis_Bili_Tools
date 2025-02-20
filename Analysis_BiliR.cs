/**
Script Name: Analysis_BiliR
Version: 2.1.0 Update on 2025/01/06
Description: B站视频解析工具
Author: Raymond_OuO(github.com/RWONG722)
https://github.com/RWONG722/Analysis_Bili_Tools
**/

using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Persistence;
using System;
using System.Text.RegularExpressions;

public class Analysis_BiliR : UdonSharpBehaviour
{
    [Header("解析链接组成：解析地址 + B站URL + BV号 + 后缀")]
    [Header("解析地址")]
    [SerializeField] private string server1;
    [SerializeField] private string server2;

    [Header("B站URL前缀")]
    [SerializeField] private string prefix1 = "bilibili.com/";
    [SerializeField] private string prefix2 = "bilibili.com/";

    [Header("URL后缀")]
    [SerializeField] private string suffix1;
    [SerializeField] private string suffix2;

    [Header("输入框")]
    public GameObject SettingUi;
    public InputField APIUrlInput;
    public InputField PrefixInput;
    public InputField SuffixInput;
    public InputField VideoUrlInput;
    public InputField VideoPartInput;

    private const string defaultAPI = "https://api.rwit.net/jx/?url=";

    void Start()
    {

    }

    void OnPlayerRestored(VRCPlayerApi player)
    {
        APIUrlInput.text = PlayerData.GetString(Networking.LocalPlayer, "AnalysisServer");
        PrefixInput.text = PlayerData.GetString(Networking.LocalPlayer, "AnalysisPrefix");
        SuffixInput.text = PlayerData.GetString(Networking.LocalPlayer, "AnalysisSuffix");
        if (APIUrlInput.text.Length == 0)
        {
            Debug.Log("First Setup");
            InitializeFields();
        }
    }

    private void InitializeFields()
    {
        if (APIUrlInput.text.Length == 0)
        {
            if (server1.Length > 0)
            {
                APIUrlInput.text = server1;
                PrefixInput.text = prefix1;
                SuffixInput.text = suffix1;
            }
            else if (server2.Length > 0)
            {
                APIUrlInput.text = server2;
                PrefixInput.text = prefix2;
                SuffixInput.text = suffix2;
            }
            else
            {
                APIUrlInput.text = defaultAPI;
                PrefixInput.text = "";
                SuffixInput.text = "";

            }
            OnChangeSettings();
        }
    }

    public void OnChangeSettings() // 保存设置
    {
        Debug.Log("Settings Saved");
        PlayerData.SetString("AnalysisServer", APIUrlInput.text);
        PlayerData.SetString("AnalysisPrefix", PrefixInput.text);
        PlayerData.SetString("AnalysisSuffix", SuffixInput.text);
    }

    public void OnInput() // 输入框内容变化时
    {
        if (string.IsNullOrEmpty(APIUrlInput.text))
        {
            VideoUrlInput.text = "没有填入解析服务器！";
            return;
        }

        if (VideoUrlInput.text.Contains("BV") || VideoUrlInput.text.Length > 11)
        {
            string bvNumber = ExtractBVNumber(VideoUrlInput.text);
            string jxurl = $"{APIUrlInput.text}{PrefixInput.text}{bvNumber}{SuffixInput.text}";
            VideoUrlInput.text = jxurl;

            // part is number and need > 1
            if (int.TryParse(VideoPartInput.text, out int partNumber) && partNumber > 1)
            {
                VideoUrlInput.text += $"&p={partNumber}";
            }
        }else if(VideoUrlInput.text.Contains("live.bilibili.com")){
            VideoUrlInput = jxurl + VideoUrlInput.text;
        }
        else
        {
            OnClear();
        }
    }

    private string ExtractBVNumber(string input)
    {
        string bvPattern = @"BV[a-zA-Z0-9]{10}";
        Match bvMatch = Regex.Match(input, bvPattern);
        
        string partPattern = @"[?&]p=(\d+)";
        Match partMatch = Regex.Match(input, partPattern);

        if (bvMatch.Success)
        {
            if (partMatch.Success && partMatch.Groups.Count > 1)
            {
                VideoPartInput.text = partMatch.Groups[1].Value;
            }
            else
            {
                VideoPartInput.text = "1";
            }
            
            return bvMatch.Value;
        }
        
        return null;
    }

    public void OnClear() // 清空输入框
    {
        VideoUrlInput.text = "";
        VideoUrlInput.placeholder.GetComponent<Text>().text = "请输入B站链接或BV号!";
    }

    public void Click_Setting()
    {
        SettingUi.SetActive(!SettingUi.activeSelf);
    }

    public void OnSelect_Server1()
    {
        UpdateServerSettings(suffix1, prefix1, server1);
    }

    public void OnSelect_Server2()
    {
        UpdateServerSettings(suffix2, prefix2, server2);
    }

    private void UpdateServerSettings(string suffix, string prefix, string server)
    {
        SuffixInput.text = suffix;
        PrefixInput.text = prefix;
        APIUrlInput.text = server;
        OnChangeSettings();
    }

    public void OnSelect_Clear()
    {
        SuffixInput.text = "";
        PrefixInput.text = "";
        APIUrlInput.text = "";
        OnChangeSettings();
    }
}