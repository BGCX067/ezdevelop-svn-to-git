#region  修订历史
/*
 * 创建时间：2010-4-6
 * 创建人：董永刚
 * 描述：存放与操作系统有关的操作和功能
 */
#endregion
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace EZDev
{
    /// <summary>
    /// 存放与操作系统有关的操作和功能
    /// </summary>
    public class SysUtils
    {
        /// <summary>
        /// 注册表中设置操作系统运行时启动程序的路径
        /// </summary>
        private const string softwareRunPathInRegistry = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        /// <summary>
        /// 返回一个空的时间
        /// </summary>
        public static DateTime EmptyDateTime
        {
            get
            {
                return new DateTime(1949, 10, 1, 1, 1, 1, 1);
            }
        }

        /// <summary>
        /// 清除当操作系统运行时自动启动的某个程序
        /// </summary>
        /// <param name="applicationName">标识该应用程序的名称</param>
        public static void CancelApplicationAutoRun(string applicationName)
        {
            try
            {
                RegistryKey localMachine = Registry.LocalMachine;
                if (localMachine != null)
                {
                    localMachine = localMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                    if (localMachine != null)
                    {
                        localMachine.DeleteValue(applicationName.Trim(), false);
                        localMachine.Close();
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 延时指定时间（毫秒）的函数
        /// </summary>
        /// <param name="milliseconds">毫秒</param>
        public static void Delay(int milliseconds)
        {
            DateTime now = DateTime.Now;
            now.AddMilliseconds((double)milliseconds);
            while (DateTime.Now < now)
            {
            }
        }

        /// <summary>
        /// 得到当操作系统运行时自动启动指定的程序的对应的可执行文件(含路径及文件名称)
        /// </summary>
        /// <param name="applicationName">标识该应用程序的名称</param>
        /// <returns>返回自动启动的内容，为""表示没有内容</returns>
        public static string GetAutoRunExecutableFile(string applicationName)
        {
            try
            {
                RegistryKey localMachine = Registry.LocalMachine;
                if (localMachine == null)
                {
                    return "";
                }
                localMachine = localMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                if (localMachine == null)
                {
                    return "";
                }
                object obj2 = localMachine.GetValue(applicationName.Trim());
                if (obj2 == null)
                {
                    localMachine.Close();
                    return "";
                }
                localMachine.Close();
                return ((string)obj2).Trim();
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 返回当前计算机名称
        /// </summary>
        /// <returns>为""表示返回失败，其它为计算机名称</returns>
        public static string GetComputerName()
        {
            StringBuilder computerName = new StringBuilder(0xff);
            int size = 0xff;
            if (GetComputerName(computerName, ref size) != 0)
            {
                return computerName.ToString(0, size).Trim();
            }
            return "";
        }

        /// <summary>
        /// 返回计算机名称
        /// </summary>
        /// <param name="computerName">计算机名称</param>
        /// <param name="size">名称长度</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern int GetComputerName(StringBuilder computerName, ref int size);

        /// <summary>
        /// 得到磁盘信息,
        /// 磁盘上剩余空间(字节) = 簇的扇区数 * 扇区的字节数 * 剩余簇数
        /// 磁盘上总空间(字节) = 簇的扇区数 * 扇区的字节数 * 总簇数
        /// </summary>
        /// <param name="driver">要判断可用空间的驱动器名</param>
        /// <param name="sectors">存放每簇扇区数的变量</param>
        /// <param name="bytes">存放每扇区字节数的变量</param>
        /// <param name="free">存放剩余簇数的变量</param>
        /// <param name="total">存放总簇数的变量</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern long GetDiskFreeSpace(string driver, out long sectors, out long bytes, out long free,
                                                    out long total);

        /// <summary>
        /// 得到磁盘空间大小信息
        /// </summary>
        /// <param name="driver">要判断可用空间的驱动器名</param>
        /// <param name="freeBytes">存放剩余字节数的变量</param>
        /// <param name="totalBytes">存放总字节数的变量</param>
        public static void GetDiskSpace(string driver, out long freeBytes, out long totalBytes)
        {
            long num;
            long num2;
            long num3;
            long num4;
            GetDiskFreeSpace(driver, out num, out num2, out num3, out num4);
            freeBytes = (num2 * num) * num3;
            totalBytes = (num2 * num) * num4;
        }

        /// <summary>
        /// 判断当操作系统运行时自动启动指定的程序是否将自动运行
        /// </summary>
        /// <param name="applicationName">标识该应用程序的名称</param>
        /// <returns>true-设置为自动运行;false-没有设置为自动运行</returns>
        public static bool IsApplicationAutoRun(string applicationName)
        {
            return (GetAutoRunExecutableFile(applicationName.Trim()) != "");
        }

        /// <summary>
        /// 判断是否指定时间是否为空的时间
        /// </summary>
        /// <param name="dateTime">要判断的时间</param>
        /// <returns>true-是空时间/false-不是空时间</returns>
        public static bool IsEmptyDateTime(DateTime dateTime)
        {
            return (dateTime.Date == EmptyDateTime.Date);
        }

        /// <summary>
        /// 设置当操作系统运行时自动启动指定的程序
        /// </summary>
        /// <param name="applicationName">标识该应用程序的名称</param>
        /// <param name="executableFile">对应该应用程序的exe文件(含路径和文件全名称)</param>
        /// <returns>是否设置成功</returns>
        public static bool SetApplicationAutoRun(string applicationName, string executableFile)
        {
            try
            {
                RegistryKey localMachine = Registry.LocalMachine;
                if (localMachine == null)
                {
                    return false;
                }
                localMachine = localMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (localMachine == null)
                {
                    return false;
                }
                localMachine.SetValue(applicationName.Trim(), executableFile.Trim());
                localMachine.Flush();
                object obj2 = localMachine.GetValue(applicationName.Trim());
                if (obj2 == null)
                {
                    localMachine.Close();
                    return false;
                }
                string strA = (string)obj2;
                if (string.Compare(strA, executableFile.Trim(), true) == 0)
                {
                    localMachine.Close();
                    return false;
                }
                localMachine.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 设置系统机器时钟
        /// </summary>
        /// <param name="systemTime">要设置的时间</param>
        /// <returns>非零表示成功，零表示失败。会设置GetLastError</returns>
        [DllImport("kernel32.dll")]
        private static extern int SetLocalTime(ref APISystemTime systemTime);

        /// <summary>
        /// 设置系统机器时钟
        /// </summary>
        /// <param name="systemTime">要设置的时间</param>
        /// <returns>true-成功；false-不成功</returns>
        public static bool SetSystemTime(DateTime systemTime)
        {
            APISystemTime time;
            time.Yaer = (short)systemTime.Year;
            time.Month = (short)systemTime.Month;
            time.DayOfWeek = (short)systemTime.DayOfWeek;
            time.Day = (short)systemTime.Day;
            time.Hour = (short)systemTime.Hour;
            time.Minute = (short)systemTime.Minute;
            time.Second = (short)systemTime.Second;
            time.Milliseconds = (short)systemTime.Millisecond;
            return (SetLocalTime(ref time) != 0);
        }

        /// <summary>
        /// 得到指定程序集中嵌入的资源
        /// </summary>
        /// <param name="assembly">指定的程序集</param>
        /// <param name="resourceName">资源全名称</param>
        /// <returns>返回的资源流,如果找不到则返回null</returns>
        public static Stream GetInnerResourceStream(Assembly assembly, string resourceName)
        {
            return assembly.GetManifestResourceStream(resourceName);
        }

        /// <summary>
        /// 得到指定程序集中嵌入的图片
        /// </summary>
        /// <param name="assembly">指定程序集</param>
        /// <param name="imageName">图片全名称</param>
        /// <returns>返回的图像,如果找不到则返回null</returns>
        public static Image GetInnerImage(Assembly assembly, string imageName)
        {
            Stream stream = assembly.GetManifestResourceStream(imageName);
            return stream == null ? null : Image.FromStream(stream);
        }

        /// <summary>
        /// 打开加密的文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        public static bool OpenEncryptFile(string fileName, out string fileContent)
        {
            if (File.Exists(fileName))
            {
                FileStream fs = null; ;
                try
                {
                    fs = new FileStream(fileName, FileMode.Open);
                    byte[] readBytes = new byte[fs.Length];
                    fs.Read(readBytes, 0, (Int32)fs.Length);
                    fileContent = DataConvert.BytesToString(DataConvert.ToNormalData(readBytes));
                    return true;
                }
                catch (Exception ex )
                {
                    fileContent = "";
                    return false;
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
            }
            fileContent = "";
            return false;
        }

        /// <summary>
        /// 保存为密码的文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="fileContent">文件内容</param>
        /// <returns>是否保存成功</returns>
        public static bool SaveEncryptFile(string fileName, string fileContent)
        {
            FileStream fs = null;
            try
            {
                byte[] fileBytes = DataConvert.ToEncryptData(DataConvert.StringToBytes(fileContent));
                fs = new FileStream(fileName, FileMode.OpenOrCreate);
                fs.SetLength(0);
                fs.Write(fileBytes, 0, fileBytes.Length);
                fs.Flush();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        #region Nested type: APISystemTime

        /// <summary>
        /// 系统时钟结构
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct APISystemTime
        {
            public short Day;

            public short DayOfWeek;

            public short Hour;

            public short Milliseconds;

            public short Minute;

            public short Month;

            public short Second;

            public short Yaer;
        }

        #endregion
    }
}