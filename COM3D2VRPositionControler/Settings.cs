﻿using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.CompilerServices;

namespace COM3D2VRPositionControler
{

    class Settings
    {

        // デフォルトでの設定値 設定ファイルが存在しない時、読み込めない時にはにはこの値がセットされる
        public static bool IsReverseMode = true;
        public static bool IsAbsoluteMoveMode = false;
        public static float MoveSpeed = 0.025f;
        public static float UpSpeed = 0.025f;
        public static float SpinSpeed = 1;

        private static string dll_path;
        private static string full_path;
        private static string file_path = @"\Config\VRYotogiPositionControlerSettings.xml";
        public static bool IsFolderExist()
        {
            dll_path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            full_path = System.IO.Directory.GetParent(dll_path).ToString() + file_path;

            
            // 設定ファイルの存在を確認
            if (System.IO.File.Exists(full_path))
            // 設定ファイルが存在していたら設定値を読み込む
            {
                try
                {
                    FileStream file = new FileStream(full_path, FileMode.Open);
                    XmlSerializer serializer = new XmlSerializer(typeof(xml_settings.SettingsInfo));
                    xml_settings.SettingsInfo settings = (xml_settings.SettingsInfo)serializer.Deserialize(file);

                    IsReverseMode = System.Convert.ToBoolean(settings.IsReverseMode);
                    IsAbsoluteMoveMode = System.Convert.ToBoolean(settings.IsAbsoluteMoveMode);
                    MoveSpeed = int.Parse(settings.MoveSpeed) * 0.0005f;
                    UpSpeed = int.Parse(settings.UpSpeed) * 0.0005f;
                    SpinSpeed = int.Parse(settings.SpinSpeed) * 0.02f;

                    UnityEngine.Debug.Log("VRYPC: 設定ファイルを読み込みました。");
                     return true;
                }
                catch(Exception e)
                {
                    UnityEngine.Debug.Log(e.ToString());
                    UnityEngine.Debug.Log("VRYPC: 設定ファイルを読み込めませんでした。ファイルが破損している可能性があります。デフォルトの設定を読み込みます。");
                    return false;
                }              

            }
            else
            {
                FileStream file = new FileStream(full_path, FileMode.Create);
                file.Close();
                XmlSerializer serializer = new XmlSerializer(typeof(xml_settings.SettingsInfo));
                var settings = new xml_settings.SettingsInfo { IsReverseMode = "true", IsAbsoluteMoveMode = "false", MoveSpeed = "50", UpSpeed = "50", SpinSpeed = "50" };
                // var streamwriter = new StreamWriter(full_path, false, Encoding.UTF8);
                StreamWriter writer = new StreamWriter(full_path, false, new UTF8Encoding(false));
                serializer.Serialize(writer, settings);
                UnityEngine.Debug.Log("VRYPC: 設定ファイルが存在しません。デフォルトの設定ファイルを作成します。");
                return false; 
            }        
        }
    }
}

namespace xml_settings
{
    [XmlRoot("settings")]
    public class SettingsInfo
    {
        [XmlElement("IsReverseMode")]
        public string IsReverseMode { get; set; }

        [XmlElement("IsAbsoluteMoveMode")]
        public string IsAbsoluteMoveMode { get; set; }

        [XmlElement("MoveSpeed")]
        public string MoveSpeed { get; set; }

        [XmlElement("UpSpeed")]
        public string UpSpeed { get; set; }

        [XmlElement("SpinSpeed")]
        public string SpinSpeed { get; set; }

    }
}

