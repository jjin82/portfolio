using System.Text;
using System.Runtime.InteropServices;

public class IniFile
{
    [DllImport("kernel32")]
    public static extern long WritePrivateprofileString(string section, string key, string val, string filePath);
    [DllImport("kernel32")]
    public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
    [DllImport("kernel32")]
    public static extern int GetPrivateProfileString(int section, string key, string def, [MarshalAs(UnmanagedType.LPArray)] byte[] Result, int size, string filePath);

    public IniFile(string path)
    {
        _filePath = path;
    }

    public void WriteIniFile(string section, string key, string value)
    {
        WritePrivateprofileString(section, key, value, _filePath);
    }

    public string ReadIniFile(string section, string key)
    {
        StringBuilder sb = new StringBuilder(255);
        GetPrivateProfileString(section, key, "", sb, sb.Capacity, _filePath);

        return sb.ToString();
    }

    public string[] GetSectionNames()
    {
        int maxsize = 500;
        for (int i = 0; i < 10; i++, maxsize *= 2)
        {
            byte[] bytes = new byte[maxsize];
            int size = GetPrivateProfileString(0, "", "", bytes, maxsize, _filePath);

            if(size < maxsize - 2)
            {
                string selectd = Encoding.ASCII.GetString(bytes, 0, size - (size > 0 ? 1 : 0));
                return selectd.Split(new char[] { '\0' });
            }
        }

        return null;
    }

    public bool GetSection(string section, string key)
    {
        StringBuilder sb = new StringBuilder(255);
        int size = GetPrivateProfileString(section, key, "", sb, sb.Capacity, _filePath);

        if (size <= 0)
            return false;

        return true;
    }

    private string _filePath = null;
}
