using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<object, object> Container = new();

        public static void Add<T>(T value)
        {
            if(!Container.ContainsKey(typeof(T)))
                Container.Add(typeof(T), value);
            else
            {
                Container[typeof(T)] = value;
                Debug.LogWarning("Service already exists in dictionary, replacing with the new value");
            }
        }
        
        public static T Get<T>()
        {
            try
            {
                return (T)Container[typeof(T)];
            }
            catch (Exception ex)
            {
                throw new NotImplementedException($"Service for {typeof(T)} not available.");
            }
        }
    }
}