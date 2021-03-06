﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Plus.HabboHotel.Roleplay
{
    /// <summary>
    /// Class ConfigurationData.
    /// </summary>
    internal static class RoleplayData
    {
        /// <summary>
        /// The data
        /// </summary>
        internal static Dictionary<string, string> Data = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationData"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="mayNotExist">if set to <c>true</c> [may not exist].</param>
        /// <exception cref="System.ArgumentException">
        /// </exception>
        internal static void Load(string filePath, bool mayNotExist = false, bool Unload = false)
        {
            if(Unload)
            {
                Data.Clear();
            }

            if (!File.Exists(filePath))
            {
                if (!mayNotExist) throw new ArgumentException(string.Format("The file was not found in '{0}'.", filePath));
            }
            else
            {
                try
                {
                    using (var streamReader = new StreamReader(filePath))
                    {
                        string text;
                        while ((text = streamReader.ReadLine()) != null)
                        {
                            if (text.Length < 1 || text.StartsWith("#")) continue;
                            var num = text.IndexOf('=');
                            if (num == -1) continue;
                            var key = text.Substring(0, num);
                            var value = text.Substring((num + 1));

                            Data.Add(key, value);
                        }
                        streamReader.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(string.Format("Could not process configuration file: {0}", ex.Message));
                }
            }
        }
    }
}