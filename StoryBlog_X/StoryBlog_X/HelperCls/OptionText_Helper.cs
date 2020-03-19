using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using StoryBlog_X.Models;

namespace StoryBlog_X.HelperCls
{
    public class OptionText_Helper
    {
        public static readonly string fileName
               = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "UserInfo.txt");

        public static string ReadAllText(string para)
        {

            bool doesExist = File.Exists(fileName);

            if (doesExist)
            {
                string text = "[" + File.ReadAllText(fileName) + "]";

                var list = JsonConvert.DeserializeObject<List<UserInfo>>(text);


                switch (para)
                {
                    case "Account":
                        return list[0].Account;
                    case "Guid":
                        return list[0].Guid.ToString();
                    case "PassWord":
                        return list[0].PassWord;
                    case "NickName":
                        return list[0].NickName;
                    case "Picture":
                        return list[0].Picture;
                    case "Phone":
                        return list[0].Phone;
                    case "Gender":
                        return list[0].Gender;
                    case "Age":
                        return list[0].Age.ToString();
                    case "Birthday":
                        return list[0].Birthday.ToString();
                    case "CreateTime":
                        return list[0].CreateTime.ToString();
                    case "Introduce":
                        return list[0].Introduce;
                    case "Describe":
                        return list[0].Describe;
                    case "BGPicture":
                        return list[0].BGPicture;
                    default:
                        return "";
                }
            }

            return "";

        }

        public static bool WriteAllText(UserInfo ui)
        {

            try
            {
                string text = JsonConvert.SerializeObject(ui);
                File.WriteAllText(fileName, text);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public static bool WriteText(string option, string para)
        {

            try
            {

                string jsonstr = File.ReadAllText(fileName);

                UserInfo ui = JsonConvert.DeserializeObject<UserInfo>(jsonstr);

                switch (option)
                {
                    case "Gender":
                        ui.Gender = para;
                        break;
                    case "PassWord":
                        ui.PassWord = para;
                        break;
                    case "Picture":
                        ui.Picture = para;
                        break;
                    case "BGPicture":
                        ui.BGPicture = para;
                        break;
                    case "NickName":
                        ui.NickName = para;
                        break;
                    case "Phone":
                        ui.Phone = para;
                        break;
                    case "Introduce":
                        ui.Introduce = para;
                        break;
                    case "Describe":
                        ui.Describe = para;
                        break;
                    case "Birthday":
                        ui.Birthday = Convert.ToDateTime(para);
                        ui.Age= DateTime.Now.Year - Convert.ToDateTime(para).Year;
                        break;
                }

                string text = JsonConvert.SerializeObject(ui);

                File.WriteAllText(fileName, text);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }

        public static bool DeleteAllText()
        {

            try
            {
                File.Delete(fileName);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }

}
