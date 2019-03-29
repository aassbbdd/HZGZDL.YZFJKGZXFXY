using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Common
{
    public class FileHelper
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger("FileHelper");
        public static string Xml_Path = ConfigurationManager.ConnectionStrings["xml_path"].ConnectionString.ToString();

        private static void PrintExcetpion(string errors, Exception ex)
        {
            _logger.ErrorFormat(errors + "    {0} \r\n {1}", ex.Message, ex.StackTrace);
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <returns></returns>
        public static string GetOpenFilePath()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + Xml_Path;

            OpenFileDialog pOpenFileDialog = new OpenFileDialog();
            pOpenFileDialog.Filter = "所有文件|*.*";//若打开指定类型的文件只需修改Filter，如打开txt文件，改为*.txt即可
            pOpenFileDialog.Multiselect = false;
            pOpenFileDialog.Title = "打开文件";
            if (!FileHelper.IsFileExist(path))//验证文件是否存在
            {
                pOpenFileDialog.InitialDirectory = path;
            }

            if (pOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                path = pOpenFileDialog.FileName;
            }
            else
            {
                path = "";
            }
            return path;
        }
        /// <summary>
        /// 获取当前运行程序文件夹路径
        /// </summary>
        public static string Local_Path_Get()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;

            return path;
        }

        public static string GetTempFileName(string fileName)
        {
            return Path.Combine(Path.GetTempPath(), string.IsNullOrEmpty(fileName) ? "~temp.tmp" : fileName);
        }

        ///////////////////////////////////
        // 文件基本操作
        public static void ClearOrCreatePath(string dstPath)
        {
            if (Directory.Exists(dstPath))
                ClearPath(dstPath);
            else
                CreateDirectoy(dstPath);
        }

        public static void ClearPath(string dstPath)
        {
            try
            {
                if (!new System.IO.DirectoryInfo(dstPath).Exists)
                    return;

                foreach (string d in System.IO.Directory.GetFileSystemEntries(dstPath))
                {
                    if (System.IO.File.Exists(d))
                    {
                        System.IO.FileInfo fi = new System.IO.FileInfo(d);
                        DeleteFile(fi); // 直接删除其中的文件
                    }
                    else
                    {
                        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(d);
                        DeletePath(di.FullName); // 删除子文件夹   
                    }
                }
            }
            catch (Exception ex)
            {
                PrintExcetpion(string.Format("清除文件夹失败-{0}", dstPath), ex);
            }
        }

        /// <summary>
        /// 删除目录下的所有文件（只删除文件）
        /// </summary>
        /// <param name="dstPath"></param>
        public static void DeleteFiles(string dstPath)
        {
            try
            {
                if (!new System.IO.DirectoryInfo(dstPath).Exists)
                    return;

                foreach (string d in System.IO.Directory.GetFiles(dstPath))
                {
                    if (System.IO.File.Exists(d))
                    {
                        System.IO.FileInfo fi = new System.IO.FileInfo(d);
                        DeleteFile(fi); // 直接删除其中的文件
                    }
                }
            }
            catch (Exception ex)
            {
                PrintExcetpion(string.Format("删除目录下的所有文件失败-{0}", dstPath), ex);
            }
        }

        public static void DeletePath(string dstPath)
        {
            try
            {
                if (!Directory.Exists(dstPath))
                    return;

                foreach (string d in System.IO.Directory.GetFileSystemEntries(dstPath))
                {
                    if (System.IO.File.Exists(d))
                    {
                        System.IO.FileInfo fi = new System.IO.FileInfo(d);
                        DeleteFile(fi); // 直接删除其中的文件
                    }
                    else
                        DeletePath(d); // 递归删除子文件夹   
                }

                System.IO.Directory.Delete(dstPath); // 删除已空文件夹
            }
            catch (Exception ex)
            {
                PrintExcetpion(string.Format("删除文件夹失败-{0}", dstPath), ex);
            }
        }
        /// <summary>
        /// 复制单个文件到指定文件夹
        /// </summary>
        /// <param name="src">指定文件路径</param>
        /// <param name="dst">保存路径</param>
        public static void CopyFile(string src, string dst)
        {
            try
            {
                if (src.ToLower() == dst.ToLower())
                {
                    return;
                }

                if (!File.Exists(src))
                {
                    return;
                }

                FileHelper.DeleteFile(dst);
                FileHelper.CreateDirectoy(Path.GetDirectoryName(dst));
                File.Copy(src, dst);
            }
            catch (Exception ex)
            {
                PrintExcetpion(string.Format("拷贝文件失败-{0}:{1}", src, dst), ex);
            }
        }
        /// <summary>
        /// 复制单个文件到指定文件夹
        /// </summary>
        /// <param name="src">指定文件路径</param>
        /// <param name="dst">保存路径</param>
        /// <param name="filename">文件名字</param>
        public static string CopyFile(string src, string dst, string filename)
        {
            try
            {
                if (src.ToLower() == dst.ToLower())
                {
                    return "复制路径和保存路径一样！";
                }

                if (!File.Exists(src))
                {
                    return "未找到需复制文件！";
                }

                FileHelper.DeleteFile(dst + filename);

                ///删除后文件还存在
                if (File.Exists(dst + filename))
                {
                    return "指定目录下已有该文件！";
                }
                FileHelper.CreateDirectoy(Path.GetDirectoryName(dst));
                File.Copy(src, dst + filename);
                return "";
            }
            catch (Exception ex)
            {
                PrintExcetpion(string.Format("拷贝文件失败-{0}:{1}", src, dst), ex);
                throw ex;
            }
        }
        /// <summary>
        /// //验证文件是否存在
        /// </summary>
        /// <param name="file">文件路径</param>
        /// <returns></returns>
        public static bool IsFileExist(string file)
        {
            return !string.IsNullOrEmpty(file) && File.Exists(file);
        }

        public static bool IsDirExist(string dir)
        {
            return !string.IsNullOrEmpty(dir) && Directory.Exists(dir);
        }

        public static void MoveFile(string src, string dst)
        {
            try
            {
                if (string.Equals(src, dst, StringComparison.CurrentCultureIgnoreCase))
                    return;

                if (!File.Exists(src))
                    return;

                FileHelper.DeleteFile(dst);
                FileHelper.CreateDirectoy(Path.GetDirectoryName(dst));

                System.IO.File.Move(src, dst);
            }
            catch (Exception ex)
            {
                PrintExcetpion($"移动文件失败-{src}:{dst}", ex);
            }
        }

        public static void MovePath(string srcPath, string dstPath)
        {
            try
            {
                if (srcPath.ToLower() == dstPath.ToLower())
                    return;

                if (!Directory.Exists(srcPath))
                    return;

                ClearOrCreatePath(dstPath);

                System.IO.Directory.Move(srcPath, dstPath);
            }
            catch (Exception ex)
            {
                PrintExcetpion(string.Format("拷贝文件失败-{0}:{1}", srcPath, dstPath), ex);
            }
        }

        public static string DiskTypeInfo = "Disk";

        public static void MovePath_copy_delete(string srcPath, string dstPath)
        {
            try
            {
                if (srcPath.ToLower() == dstPath.ToLower())
                    return;

                if (!Directory.Exists(srcPath))
                    return;

                ClearOrCreatePath(dstPath);

                MoveDirectory(srcPath, dstPath);
                if (DiskTypeInfo == "Disk")
                {
                    DeletePath(srcPath);
                }

            }
            catch (Exception ex)
            {
                PrintExcetpion($"拷贝删除文件失败-{srcPath}:{dstPath}", ex);
            }
        }

        /// <summary>
        /// 移动整个文件夹下文件
        /// </summary>
        /// <param name="srcDir"></param>
        /// <param name="tgtDir"></param>
        public static void MoveDirectory(string srcDir, string tgtDir)
        {
            if (string.Equals(srcDir, tgtDir, StringComparison.CurrentCultureIgnoreCase))
                return;

            if (!Directory.Exists(srcDir))
                return;

            DirectoryInfo source = new DirectoryInfo(srcDir);
            DirectoryInfo target = new DirectoryInfo(tgtDir);

            if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("父目录不能拷贝到子目录！");
            }

            if (!source.Exists)
            {
                return;
            }

            if (!target.Exists)
            {
                target.Create();
            }

            FileInfo[] files = source.GetFiles();

            foreach (var file in files)
            {
                if (DiskTypeInfo == "Disk")
                {
                    File.Move(file.FullName, target.FullName + @"\" + file.Name);
                }
                else
                    File.Copy(file.FullName, target.FullName + @"\" + file.Name);
            }

            DirectoryInfo[] dirs = source.GetDirectories();

            foreach (var dir in dirs)
            {
                MoveDirectory(dir.FullName, target.FullName + @"\" + dir.Name);
            }
        }


        /// <summary>
        /// 复制整个文件夹下文件
        /// </summary>
        /// <param name="srcDir"></param>
        /// <param name="tgtDir"></param>
        public static void CopyDirectory(string srcDir, string tgtDir)
        {
            if (srcDir.ToLower() == tgtDir.ToLower())
                return;

            if (!Directory.Exists(srcDir))
                return;

            DirectoryInfo source = new DirectoryInfo(srcDir);
            DirectoryInfo target = new DirectoryInfo(tgtDir);

            if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("父目录不能拷贝到子目录！");
            }

            if (!source.Exists)
            {
                return;
            }

            if (!target.Exists)
            {
                target.Create();
            }

            FileInfo[] files = source.GetFiles();

            foreach (var file in files)
            {
                File.Copy(file.FullName, target.FullName + @"\" + file.Name, true);
            }

            DirectoryInfo[] dirs = source.GetDirectories();

            foreach (var dir in dirs)
            {
                CopyDirectory(dir.FullName, target.FullName + @"\" + dir.Name);
            }
        }


        /// 拷贝oldlab的文件到newlab下面
        /// </summary>
        /// <param name="sourcePath">lab文件所在目录(@"~\labs\oldlab")</param>
        /// <param name="savePath">保存的目标目录(@"~\labs\newlab")</param>
        /// <returns>返回:true-拷贝成功;false:拷贝失败</returns>
        public static bool CopyOldLabFilesToNewLab(string sourcePath, string savePath)
        {
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            #region //拷贝labs文件夹到savePath下
            try
            {
                string[] labDirs = Directory.GetDirectories(sourcePath);//目录
                string[] labFiles = Directory.GetFiles(sourcePath);//文件
                if (labFiles.Length > 0)
                {
                    for (int i = 0; i < labFiles.Length; i++)
                    {

                        File.Copy(sourcePath + "\\" + System.IO.Path.GetFileName(labFiles[i]), savePath + "\\"
                            + System.IO.Path.GetFileName(labFiles[i]), true);

                    }
                }
                if (labDirs.Length > 0)
                {
                    for (int j = 0; j < labDirs.Length; j++)
                    {
                        Directory.GetDirectories(sourcePath + "\\" + System.IO.Path.GetFileName(labDirs[j]));

                        //递归调用
                        CopyOldLabFilesToNewLab(sourcePath + "\\" + System.IO.Path.GetFileName(labDirs[j]), savePath + "\\"
                            + System.IO.Path.GetFileName(labDirs[j]));
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            #endregion
            return true;
        }

        /// <summary>  
        /// 复制文件夹中的所有文件夹与文件到另一个文件夹
        /// </summary>
        /// <param name="sourcePath">源文件夹</param>
        /// <param name="destPath">目标文件夹</param>
        public static void CopyFolder(string sourcePath, string destPath)
        {
            if (Directory.Exists(sourcePath))
            {
                if (!Directory.Exists(destPath))
                {
                    //目标目录不存在则创建
                    try
                    {
                        Directory.CreateDirectory(destPath);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("创建目标目录失败：" + ex.Message);
                    }
                }
                //获得源文件下所有文件
                List<string> files = new List<string>(Directory.GetFiles(sourcePath));
                files.ForEach(c =>
                {
                    string destFile = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                    File.Copy(c, destFile, true);//覆盖模式
                });
                //获得源文件下所有目录文件
                List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));
                folders.ForEach(c =>
                {
                    string destDir = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                    //采用递归的方法实现
                    CopyFolder(c, destDir);
                });
            }
            else
            {
                throw new DirectoryNotFoundException("源目录不存在！");
            }
        }

        public static void DeleteFile(string fileName)
        {
            try
            {
                FileInfo fi = new System.IO.FileInfo(fileName);
                DeleteFile(fi);
            }
            catch (Exception ex)
            {
                PrintExcetpion($"删除文件失败-{fileName}", ex);
            }
        }

        public static void DeleteFile(FileInfo fi)
        {
            try
            {
                if (fi.Exists)
                {
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = System.IO.FileAttributes.Normal;
                    System.IO.File.Delete(fi.FullName);

                    fi.Delete();
                }
            }
            catch (Exception ex)
            {
                PrintExcetpion($"删除文件失败-{fi.FullName}", ex);
            }
        }

        public static void CreateDirectoyFromFileName(string fileName)
        {
            try
            {
                string path = Path.GetDirectoryName(fileName);
                if (!new System.IO.DirectoryInfo(path).Exists)
                    System.IO.Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                PrintExcetpion($"创建文件夹失败-{fileName}", ex);
            }
        }

        public static void CreateDirectoyByFileName(string fileName)
        {
            try
            {
                //string dir = Path.GetDirectoryName(fileName);
                CreateDirectoy(fileName);
            }
            catch (Exception ex)
            {
                PrintExcetpion($"由文件创建文件夹失败-{fileName}", ex);
            }
        }

        public static void CreateDirectoy(string dir)
        {
            try
            {
                if (!new System.IO.DirectoryInfo(dir).Exists)
                    System.IO.Directory.CreateDirectory(dir);
            }
            catch (Exception ex)
            {
                PrintExcetpion($"创建文件夹失败-{dir}", ex);
            }
        }

        public static string[] GetAllSubDirs(string root, SearchOption searchOption)
        {
            string[] all = null;
            try
            {
                all = Directory.GetDirectories(root, "*", searchOption);
            }
            catch
            {
                // ignored
            }
            return all;
        }

        public static List<string> GetAllPureSubDirs(string root)
        {
            List<string> list = new List<string>();
            try
            {
                string[] all = Directory.GetDirectories(root, "*", SearchOption.TopDirectoryOnly);
                foreach (string path in all)
                {
                    string s = System.IO.Path.GetFileName(path);
                    list.Add(s);
                }
            }
            catch
            {
                // ignored
            }

            return list;
        }

        public static long GetFilesSize(string[] fileNames)
        {
            return fileNames.Sum(fileName => GetFileSize(fileName));
        }

        public static long GetFileSize(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
                return new FileInfo(fileName).Length;

            return 0;
        }

        /// <summary>
        /// Files the content.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static byte[] FileContent(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            try
            {
                byte[] buffur = new byte[fs.Length];
                fs.Read(buffur, 0, (int)fs.Length);

                return buffur;
            }
            catch (Exception ex)
            {
                Console.WriteLine("读取文件字节流失败: {0}", ex.Message);
                return null;
            }
            finally
            {
                if (fs != null)
                {
                    //关闭资源
                    fs.Close();
                }
            }
        }

        public static bool SaveString2File(string content, string fileName)
        {
            StreamWriter sw = null;

            try
            {
                CreateDirectoyFromFileName(fileName);

                sw = new StreamWriter(fileName);
                sw.WriteLine(content);
            }
            catch (Exception ex)
            {
                PrintExcetpion($"保存内容错误-{fileName}", ex);
                return false;
            }

            if (sw != null)
                sw.Close();

            return true;
        }

        public static string LoadStringFromFile(string fileName)
        {
            string content = string.Empty;

            StreamReader sr = null;
            try
            {
                sr = new StreamReader(fileName, System.Text.Encoding.UTF8);
                content = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                PrintExcetpion($"读取内容错误-{fileName}", ex);
            }

            if (sr != null)
                sr.Close();

            return content;
        }

        public static string GetLocalFileName(string lastPath, string displayName, string[] typeNames)
        {
            OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();

            // ofd.InitialDirectory = lastPath;
            ofd.RestoreDirectory = true;

            // "动画(.flc;.fli;.gif;.swf)|*.flc;*.fli;*.gif;*.swf|Animation(.flc,.fli)|*.flc;*.fli|Gif89a(.gif)|*.gif|SWF(*.swf)|*.swf|"
            // displayName + "(*.epub|All files (*.*)|*.*";
            string filter = displayName + "(";

            filter = typeNames.Aggregate(filter, (current, ext) => current + ("." + ext + ";"));

            filter += ")|";

            for (int i = 0; i < typeNames.Length; i++)
            {
                filter += "*." + typeNames[i];
                if (i < typeNames.Length - 1)
                    filter += ";";
            }

            // filter += "|全部文件 (*.*)|*.*";
            ofd.Filter = filter;

            return ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK ? string.Empty : ofd.FileName;
        }

        /// <summary>
        /// 获取差异的文件列表
        /// </summary>
        /// <param name="srcDir"></param>
        /// <param name="destDir"></param>
        /// <returns></returns>
        //public static List<string> GetNotSameFiles(string srcDir, string destDir)
        //{
        //    var oldData = new List<string>();
        //    var newData = new List<string>();
        //    new DirectoryInfo(srcDir).GetFiles().ToList().ForEach((f) =>
        //    {
        //        newData.Add(f.Name);
        //    });

        //    new DirectoryInfo(destDir).GetFiles().ToList().ForEach((f) =>
        //    {
        //        oldData.Add(f.Name);
        //    });

        //    //求交集
        //    var dupComparer = new InlineComparer<string>((i1, i2) => i1.Equals(i2), i => i.GetHashCode());
        //    var _IntersectData = oldData.Intersect(newData, dupComparer);

        //    if (_IntersectData == null || _IntersectData.Count() == 0)
        //        return null;

        //    // 处理交集文件里可能有差异的内容。
        //    var _files = new List<string>();

        //    // 通过md5先过滤一部分数据 然后找到有差异的文件列表
        //    _IntersectData.ToList().ForEach((s) =>
        //    {
        //        var o = Path.Combine(destDir, s);
        //        var n = Path.Combine(srcDir, s);

        //        //var omd5 = Md5FileHelper.MD5File(o);
        //        //var nmd5 = Md5FileHelper.MD5File(n);

        //        if (!omd5.Equals(nmd5))
        //        {
        //            _files.Add(s);
        //        }
        //    });

        //    return _files;
        //}

        /// <summary>
        /// 获取root文件夹下所有的文件,包含子文件夹中的文件
        /// </summary>
        /// <param name="root">文件夹路径</param>
        /// <param name="extension">文件扩展名,默认为所有文件</param>
        /// <returns>root文件夹下所有的文件的列表</returns>
        public static List<string> GetAllFileList(string root, string extension = "")
        {
            if (Directory.Exists(root))
            {
                var dirList = GetAllSubDirs(root, SearchOption.AllDirectories).ToList();
                dirList.Add(root);

                var list = new List<string>();

                foreach (var dir in dirList)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(dir);
                    dirInfo.GetFiles().ToList().ForEach(f =>
                    {
                        if (!string.IsNullOrWhiteSpace(extension))
                        {
                            if (f.Extension == (extension.Contains(".") ? extension : "." + extension))
                            {
                                list.Add(f.FullName);
                            }
                        }
                        else
                        {
                            list.Add(f.FullName);
                        }
                    });
                }

                return list;
            }
            else
            {
                return null;
            }
        }

        public class InlineComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> getEquals;
            private readonly Func<T, int> getHashCode;

            public InlineComparer(Func<T, T, bool> equals, Func<T, int> hashCode)
            {
                this.getEquals = equals;
                this.getHashCode = hashCode;
            }

            public bool Equals(T x, T y)
            {
                return this.getEquals(x, y);
            }

            public int GetHashCode(T obj)
            {
                return this.getHashCode(obj);
            }
        }
    }
}
