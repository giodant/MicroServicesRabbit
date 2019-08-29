using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MicroServicesRabbit.Domain.Core.Extensions
{
    public static class CoreExtensions
    {
        private readonly static HashAlgorithm s_defaultAlgorithm = SHA256.Create();
        /// <summary>
        /// Get Encryption SHA256 for any object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetSHA256(this object obj)
        {
            return BitConverter.ToString(s_defaultAlgorithm.ComputeHash(obj.ToByteArray()));
        }

        // Convert an object to a byte array
        public static byte[] ToByteArray(this object obj)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }

        public static string ToErrorString(this ValidationResult validationRule)
        {
            string result = string.Empty;
            if (validationRule.Errors.Any())
            {
                StringBuilder sb = new StringBuilder();
                validationRule.Errors.ForEach(e =>
                {
                    sb.AppendLine(e.ErrorMessage);
                });
                result = sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// Gt Custom T Attribute  from TEnum
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns>Custom Attribute if exists</returns>
        public static T GetEnumAttribute<TEnum, T>(TEnum item)
        {
            Type type = item.GetType();

            var attribute = type.GetField(item.ToString()).GetCustomAttributes(typeof(T), false).Cast<T>().FirstOrDefault();
            return attribute;
        }

        public static string ToFullDateString(this DateTime date)
        {
            return $"{date.ToShortDateString()} {date.ToShortTimeString()}";
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }

        public static void ForEach<T>(this IQueryable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }


        public static void ForEach<T>(this IEnumerable<T> source, Func<T, bool> action)
        {
            foreach (var item in source)
                if (!action(item))
                    break;
        }

        public static void ForEach<T>(this IQueryable<T> source, Func<T, bool> action)
        {
            foreach (var item in source)
                if (!action(item))
                    break;
        }

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, int, bool> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }

        /// <summary>
        /// Get root directory for current Path. Assume that ".init" file is present in root directory
        /// </summary>
        /// <param name="currentPath"></param>
        /// <returns></returns>
        public static DirectoryInfo TryGetStartUpDirectoryInfo(string currentPath = null)
        {
            var directory = new DirectoryInfo(
                currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.init").Any())
            {
                directory = directory.Parent;
            }
            return directory;
        }

        /// <summary>
        /// Check if a class has an attribute of type attribType
        /// </summary>
        /// <param name="clsType"></param>
        /// <param name="attribType"></param>
        /// <returns></returns>
        public static object HasClassAttribute(Type clsType, Type attribType)
        {
            if (clsType == null)
                throw new ArgumentNullException("clsType");

            return clsType.GetCustomAttributes(attribType, false).FirstOrDefault();
        }

        public static string ToHex(this long number)
        {
            return ToHex(number.ToString());
        }

        public static string ToHex(this double number)
        {
            return ToHex(number.ToString());
        }

        public static string ToHex(this string str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.Unicode.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }



        /// <summary>
        /// Json Serialization of an Object into a stream
        /// </summary>
        /// <param name="value">Object to serailize</param>
        /// <param name="stream">Stream for serialization</param>
        public static void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jtw, value);
                jtw.Flush();
            }
        }


        /// <summary>
        /// Get Value from FNC File for FNC Import File
        /// </summary>
        /// <param name="fncItem">Row of FNC File</param>
        /// <param name="code">Code to Find</param>
        /// <returns></returns>
        public static string GetFNCValue(string fncItem, string code)
        {
            string matchPattern = @"(?<displayName>[A-Z_]{1,7})\:(?<value>[\w\\.\/\w]+)|(?<displayName>[A-Z_]{1,7})(?<value>[-\d.]+)";
            MatchCollection collection = Regex.Matches(fncItem, matchPattern);
            var match = collection.FirstOrDefault(x => x.Groups["displayName"].Value == code);
            if (match != null)
            {
                return match.Groups["value"].Value;
            }
            return string.Empty;
        }

        public static long ParseFromDisplayName<TEnum>(string displayName)
        {
            long result = -1;
            var values = Enum.GetValues(typeof(TEnum));
            foreach (TEnum value in values)
            {
                try
                {
                    string itemToCheck = GetEnumAttribute<TEnum, DisplayNameAttribute>(value).DisplayName;
                    if (itemToCheck.ToUpper() == displayName.ToUpper())
                    {
                        result = Convert.ToInt32(value);
                        break;
                    }
                }
                catch (Exception)
                {
                }
            }
            return result;
        }

        public static Task<List<TSource>> ToListAsyncSafe<TSource>(
                this IQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            return !(source is IAsyncEnumerable<TSource>) ? Task.FromResult(source.ToList()) : source.ToListAsync();
        }
    }
}
