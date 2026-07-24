#define CHECK_CHECKSUM

#region Using Direction
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;

#if UNITY_2019_2_OR_NEWER
using UnityEngine;
#endif
#endregion

#if SERVER
using System.Windows.Forms;
#endif

namespace GameData
{
    /// <summary>
    /// Excel의 하나의 Row를 의미하는 인터페이스
    /// </summary>
    public class IRow { }

    /// <summary>
    /// Excel 데이터에 접근하는 전역 인터페이스이자 관리자.
    /// </summary>
    public static class Excel
    {
        /// <summary>
        /// Excel Importer의 버전. 상하위 버전 호환 처리에 사용
        /// </summary>
        public const byte ExcelImporterVersion = 1;
        public static string Hash = "";
        public static DateTime LastModifiedTime = default;

        public struct ExcelData
        {
            public bool isLoad;
            public Dictionary<int, List<IRow>> sheetData;
            public FieldInfo stringTableInfo;
            public void init(SheetName sheetname)
            {
                isLoad = false;
                sheetData = new Dictionary<int, List<IRow>>();

                var rowType = Excel.GetType($"{sheetname.ToString()}");
                if (null == rowType)
                {
#if SERVER
                    MessageBox.Show($"Failed to Load Data.. sheetname( {sheetname} )", "★ ★ ★   CRITICAL   ★ ★ ★", MessageBoxButtons.OK, MessageBoxIcon.Information);
#endif
                }

                stringTableInfo = rowType.GetField("stringTable", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            }
        }

        /// <summary>
        /// 실제 Excel 데이터가 저장되는 Repository
        /// ConcurrentDictionary 이용
        /// </summary>
        private static readonly ConcurrentDictionary<int, ExcelData> _table = new ConcurrentDictionary<int, ExcelData>();


        /// <summary>
        /// Data Table별 ID를 Return
        /// </summary>   

        static Excel()
        {

        }

        /// <summary>
        /// 특정 ID의 Row 하나를 가져옴. Type Cast가 필요.
        /// SheetName으로 작성된 클래스를 사용하도록 권장.(Type Cast 불필요)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 

        public static IRow GetRow(SheetName sheetName, int id)
        {
            if (_table.ContainsKey((int)sheetName) == true)
            {
                if (_table[(int)sheetName].isLoad == true && _table[(int)sheetName].sheetData.ContainsKey(id) == true)
                {
                    return _table[(int)sheetName].sheetData[id][0];
                }
                else
                {
#if UNITY_2019_2_OR_NEWER
                    Debug.Log("[Excel] not find (sheetName,id) : " + sheetName.ToString() + "," + id.ToString());
#endif
                    return null;
                }
            }
#if UNITY_2019_2_OR_NEWER
            Debug.LogWarning("[Excel] not find sheetName : " + sheetName.ToString());
#endif
            return null;
        }

        public static List<T> GetRows<T>(SheetName sheetName, int id) where T : IRow
        {
            if (_table.ContainsKey((int)sheetName) == true)
            {
                if (_table[(int)sheetName].isLoad == true && _table[(int)sheetName].sheetData.ContainsKey(id) == true)
                {
                    List<T> list = new List<T>();
                    foreach (var row in _table[(int)sheetName].sheetData[id])
                    {
                        list.Add((T)row);
                    }
                    return list;
                }
                else
                {
#if UNITY_2019_2_OR_NEWER
                    Debug.Log("[Excel] not find (sheetName,id) : " + sheetName.ToString() + "," + id.ToString());
#endif
                    return null;
                }
            }
#if UNITY_2019_2_OR_NEWER
            Debug.LogWarning("[Excel] not find sheetName : " + sheetName.ToString());
#endif
            return null;
        }

        public static List<T> GetList<T>(SheetName sheetName) where T : IRow
        {
            if (_table[(int)sheetName].isLoad == false)
            {
                throw new Exception("[Excel] not load sheet : " + sheetName.ToString());
            }

            if (_table.ContainsKey((int)sheetName) == true)
            {
                List<T> listAll = new List<T>();
                foreach (var rows in _table[(int)sheetName].sheetData.Values)
                {
                    foreach (var row in rows)
                    {
                        listAll.Add((T)row);
                    }
                }
                return listAll;
            }
            throw new Exception("[Excel] not find sheet : " + sheetName.ToString());
        }

        public static List<int> GetKeyList(SheetName sheetName)
        {
            if (_table[(int)sheetName].isLoad == false)
            {
                throw new Exception("[Excel] not load sheet : " + sheetName.ToString());
            }

            if (_table.ContainsKey((int)sheetName) == true)
            {
                return new List<int>(_table[(int)sheetName].sheetData.Keys);
            }
            throw new Exception("[Excel] not find sheet : " + sheetName.ToString());
        }       

        public static T GetRandom<T>(SheetName sheetName) where T : IRow
        {
            List<T> list = GetList<T>(sheetName);
            if( list.Count == 0)
            {
                throw new Exception("[Excel] list count zero, sheet : " + sheetName.ToString());
            }

            return list[new System.Random().Next(list.Count)];
        }

        public static void LoadAllGameData(string dataPath)
        {
            var files = Directory.EnumerateFiles(dataPath);
            if (0 == files.Count())
            {
                throw new Exception($"[Excel] game data load failure...");
            }

            foreach (var sheetName in Enum.GetNames(typeof(SheetName)))
            {
                bool exist = false;
                foreach(var file in files)
                {
                    if (false == file.Contains(sheetName))
                        continue;

                    exist = true;
                    break;
                }

                if (false == exist)
                {
                    throw new Exception($"[Excel] game data load failure... seet name( {sheetName} )");
                }
            }

            List<byte[]> dataList = new List<byte[]>();

            foreach (string currentFile in files)
            {
                byte[] data = File.ReadAllBytes(currentFile);
                string name = Path.GetFileNameWithoutExtension(currentFile.Replace("_Server", ""));

                try
                {
                    if (false == LoadGameData(data, (SheetName)Enum.Parse(typeof(SheetName), name)))
                    {
                        throw new Exception($"[Excel] game data load failure... seet name( {name} )");
                    }
                }
                catch(Exception e)
                {
                    throw new Exception($"[Excel] game data load failure... seet name( {name} )", e);
                }

                // 해시값 등록.
                dataList.Add(MD5.Create().ComputeHash(data));

                if (File.GetLastWriteTime(currentFile) > LastModifiedTime)
                {
                    LastModifiedTime = File.GetLastWriteTime(currentFile);
                }
            }            

            // 전체 해시값 확보.
            var hashBufs = dataList.SelectMany(x => x).ToArray();
            var hash = MD5.Create().ComputeHash(hashBufs);
            Hash = BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        public static bool LoadGameData(byte[] bytes, SheetName sheetName)
        {
            try
            {
                var (intTable, floatTable, int64Table, stringTable, result) = ParseData(ref bytes);
                if (result == false)
                {
                    return false;
                }

                ExcelData excelData = new ExcelData();
                excelData.init(sheetName);

                excelData.stringTableInfo.SetValue(null, stringTable);

                int i = 0;
                foreach (var intRow in intTable)
                {                
                    var row = Activator.CreateInstance(GetType($"{sheetName}"), new object[] { intTable[i], floatTable[i], int64Table[i], });                    
                    int id = intRow[0];

                    if (!excelData.sheetData.ContainsKey(id))
                    {
                        List<IRow> list = new List<IRow>();
                        list.Add(row as IRow);
                        excelData.sheetData.Add(id, list);
                    }
                    else
                    {
                        excelData.sheetData[id].Add(row as IRow);
                    }

                    i++;
                }
                excelData.isLoad = true;
                _table.AddOrUpdate((int)sheetName, excelData, (key, oldValue) => oldValue = excelData);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception($"[Excel] game data load failure... seet name( {sheetName} )", e);
            }
        }

        // 테이블 명시적으로 로드
        public static void ClearGameData()
        {
            foreach (var value in (SheetName[])Enum.GetValues(typeof(SheetName)))
            {
                Unload(value);
            }
        }

        /// <summary>
        /// 특정 Sheet를 메모리에서 내린다.
        /// </summary>
        /// <param name="eSheetName"></param>
        public static void Unload(SheetName eSheetName)
        {
            if (_table.TryRemove((int)eSheetName, out ExcelData outData))
            {
#if UNITY_2019_2_OR_NEWER
                Debug.Log("Unload :" + (SheetName)eSheetName);
#endif
            }
        }

        public static bool IsLoaded(SheetName eSheetName)
        {
            return _table.ContainsKey((int)eSheetName);
        }

        #region Load Data        
        private static (int[][] intTable, float[][] floatTable, Int64[][] int64Table , string[] stringTable, bool result) ParseData(ref byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes);
            BinaryReader br = new BinaryReader(ms);

            //var dateTime = br.ReadBytes(8);

            // Row 개수
            var rowNum = br.ReadInt32();
            var intTable = new int[rowNum][];
            var floatTable = new float[rowNum][];
            var int64Table = new Int64[rowNum][];

            // int table
            var intColNum = br.ReadInt32();
            for (int i = 0; i < rowNum; i++)
            {
                intTable[i] = new int[intColNum];
                for (int j = 0; j < intColNum; j++)
                    intTable[i][j] = br.ReadInt32();
            }

            // float table
            var floatColNum = br.ReadInt32();
            for (int i = 0; i < rowNum; i++)
            {
                floatTable[i] = new float[floatColNum];
                for (int j = 0; j < floatColNum; j++)
                    floatTable[i][j] = (float)br.ReadDouble();
            }

            // int64 table
            var int64ColNum = br.ReadInt32();
            for (int i = 0; i < rowNum; i++)
            {
                int64Table[i] = new Int64[int64ColNum];
                for (int j = 0; j < int64ColNum; j++)
                    int64Table[i][j] = br.ReadInt64();
            }
            
            // stringTable
            var stringTable = new string[br.ReadInt32()];
            for (int i = 0; i < stringTable.Length; i++)
                stringTable[i] = br.ReadString();

            byte[] hashkey = br.ReadBytes(16);
            byte[] checkHashkey;
            using (MD5 md5 = MD5.Create())
                checkHashkey = md5.ComputeHash(bytes, 0, bytes.Length - 16);            

            br.Close();
            ms.Close();

            return (intTable, floatTable, int64Table, stringTable, hashkey.SequenceEqual(checkHashkey));
        }
        #endregion

        #region Utility
        /// <summary>
        /// Editor에서 사용 가능한 확장 GetType 함수
        /// </summary>
        /// <param name="TypeName"></param>
        /// <returns></returns>
        public static Type GetType(string TypeName)
        {
            // Try Type.GetType() first. This will work with types defined
            // by the Mono runtime, in the same assembly as the caller, etc.
            var type = Type.GetType(TypeName);

            // If it worked, then we're done here
            if (type != null)
                return type;
#if !UNITY_EDITOR
            return null;
#else
            // If the TypeName is a full name, then we can try loading the defining assembly directly
            if (TypeName.Contains("."))
            {
                // Get the name of the assembly (Assumption is that we are using
                // fully-qualified type names)
                var assemblyName = TypeName.Substring(0, TypeName.IndexOf('.'));

                // Attempt to load the indicated Assembly
                var assembly = Assembly.Load(assemblyName);
                if (assembly == null)
                    return null;

                // Ask that assembly to return the proper Type
                type = assembly.GetType(TypeName);
                if (type != null)
                    return type;
            }

            // If we still haven't found the proper type, we can enumerate all of the
            // loaded assemblies and see if any of them define the type
            var currentAssembly = Assembly.GetExecutingAssembly();
            var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
            foreach (var assemblyName in referencedAssemblies)
            {
                // Load the referenced assembly
                var assembly = Assembly.Load(assemblyName);
                if (assembly != null)
                {
                    // See if that assembly defines the named type
                    type = assembly.GetType(TypeName);
                    if (type != null)
                        return type;
                }
            }

            // The type just couldn't be found...
            return null;
#endif
        }
        #endregion
    }
}
