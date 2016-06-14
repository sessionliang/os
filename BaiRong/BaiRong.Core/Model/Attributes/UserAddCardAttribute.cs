using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace BaiRong.Model
{
   public class UserAddCardAttribute
    {
       public UserAddCardAttribute()
       { }

       public static string CardID = "CardID";
       public static string CardCount = "CardCount";
       public static string BuyTime = "BuyTime";
       public static string IP = "IP";
       public static string SettingsXML = "SettingsXML";
       public static string UserName = "UserName";

       private static ArrayList userAddCardAttributes;
       public static ArrayList UserAddCardAttributes
       {
           get
           {
               if (userAddCardAttributes == null)
               {
                   userAddCardAttributes = new ArrayList();
                   userAddCardAttributes.Add(CardID.ToLower());
                   userAddCardAttributes.Add(CardCount.ToLower());
                   userAddCardAttributes.Add(BuyTime.ToLower());
                   userAddCardAttributes.Add(IP.ToLower());
                   userAddCardAttributes.Add(SettingsXML.ToLower());
                   userAddCardAttributes.Add(UserName.ToLower());
               }

               return userAddCardAttributes;
           }
       }
    }
}
