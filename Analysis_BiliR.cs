/**
Script Name: Analysis_BiliR
Version: 2.0.3 Update on 2025/01/01
Description: B站视频解析工具
Author: Raymond_OuO(github.com/RWONG722)
https://github.com/RWONG722/Analysis_Bili_Tools
**/

using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

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

    private const string defaultAPI = "https://api.rwit.net/?url=";

    void Start()
    {
        InitializeFields();
    }

    private void InitializeFields()
    {
        APIUrlInput.text = !string.IsNullOrEmpty(server1) ? server1 : !string.IsNullOrEmpty(server2) ? server2 : defaultAPI;
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
                VideoUrlInput.text += $"&part={partNumber}";
            }
        }
        else
        {
            OnClear();
        }
    }

    private string ExtractBVNumber(string input)
    {
        int startIndex = input.IndexOf("BV");
        return startIndex >= 0 ? input.Substring(startIndex, 12) : string.Empty;
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
    }

    public void OnSelect_Clear()
    {
        SuffixInput.text = "";
        PrefixInput.text = "";
        APIUrlInput.text = "";
    }
}