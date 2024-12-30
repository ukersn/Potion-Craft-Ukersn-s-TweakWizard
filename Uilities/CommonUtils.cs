using System;
using System.Reflection;
namespace PotionCraftAutoGarden.Utilities
{
    internal class CommonUtils
    {
        // 通用反射方法来获取属性值
        public static T GetPropertyValue<T>(object obj, string propertyName, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty)
        {
            if (obj == null)
            {
                LoggerWrapper.LogError(string.Format("Object is null when trying to get property: {0}", propertyName));
                return default(T);
            }

            Type type = obj.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName, bindingFlags);

            if (propertyInfo != null)
            {
                try
                {
                    return (T)propertyInfo.GetValue(obj);
                }
                catch (Exception e)
                {
                    LoggerWrapper.LogError(string.Format("Error getting property {0}: {1}", propertyName, e.Message));
                }
            }
            else
            {
                LoggerWrapper.LogError(string.Format("Property not found: {0}", propertyName));
            }

            return default(T);
        }

        // 通用反射方法来获取属性值
        public static T GetPropertyValueS<T>(object obj, string propertyName, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty)
        {
            if (obj == null)
            {
                LoggerWrapper.LogInfo(string.Format("Object is null when trying to get property: {0}", propertyName));
                return default(T);
            }

            Type type = obj.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName, bindingFlags);

            if (propertyInfo != null)
            {
                try
                {
                    return (T)propertyInfo.GetValue(obj);
                }
                catch (Exception e)
                {
                    LoggerWrapper.LogInfo(string.Format("Error getting property {0}: {1}", propertyName, e.Message));
                }
            }
            else
            {
                LoggerWrapper.LogInfo(string.Format("Property not found: {0}", propertyName));
            }

            return default(T);
        }
        public static bool SetPropertyValueS(object obj, string propertyName, object value, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty)
        {
            if (obj == null)
            {
                LoggerWrapper.LogInfo($"Object is null when trying to set property: {propertyName}");
                return false;
            }

            Type type = obj.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName, bindingFlags);

            if (propertyInfo != null)
            {
                try
                {
                    propertyInfo.SetValue(obj, value, null);
                    return true;
                }
                catch (Exception e)
                {
                    LoggerWrapper.LogInfo($"Error setting property {propertyName}: {e.Message}");
                }
            }
            else
            {
                LoggerWrapper.LogInfo($"Property not found: {propertyName}");
            }

            return false;
        }

    }
}
