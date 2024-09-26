/**
Script Name: Analysis_BiliR
Version: 2.0.2
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
    [SerializeField] private string prefix;

    [Header("URL后缀")]
    [SerializeField] private string suffix;

    [Header("输入框")]
    public GameObject ui;
    public InputField SJX_Url;
    public InputField Sprefix;
    public InputField Ssuffix;
    public InputField Input;
    public InputField part;

    void Start()
    {   
	//default Api Server
		string defaultAPI = "https://api.rwit.net/?url=";
        part.text = "";
		
        if (server1.Length > 0)
        {	
            SJX_Url.text = server1;
        }
        else if (server2.Length > 0)
        {   
            SJX_Url.text = server2;
        }
        else
        {
            SJX_Url.text = defaultAPI;
        }

        if (prefix.Length > 0)
        {
            Sprefix.text = prefix;
        }
        else
        {
            Sprefix.text = "bilibili.com/";
        }

        if (server1.Length == 0)
        {
            server1 = defaultAPI;
        }
        if (server2.Length == 0)
        {
            server2 = defaultAPI;
        }
        

    }
    public void OnInput()//输入框内容变化时
    {
		if (SJX_Url.text.Length > 0)
		{
			Input.text = "没有填入解析服务器！";
            return;
		}
		
        if (Input.text.Contains("BV") || Input.text.Length > 11)
        {
            string text = Input.text;
            Input.text = Input.text.Substring(Input.text.IndexOf("BV"), 12);
            string jxurl = SJX_Url.text + Sprefix.text + Input.text + Ssuffix.text;
            Input.text = jxurl;
            

            //part is number and need > 1
            if (part.text.Length > 0 && int.Parse(part.text) > 1)
            {
                Input.text += "&part=" + part.text;
            }

        }
        else
        {
            OnClear();
        }

    }

    public void OnClear()//清空输入框
    {
        Input.text = "";
        Input.placeholder.GetComponent<Text>().text = "请输入B站链接或BV号!";

    }

    public void Click_Setting()
    {
        ui.SetActive(!ui.activeSelf);
    }

    public void OnSelect_Server1()
    {
        SJX_Url.text = server1;
    }

    public void OnSelect_Server2()
    {
        SJX_Url.text = server2;
    }

    public void OnSelect_Clear()
    {
        SJX_Url.text = "";
    }
}
