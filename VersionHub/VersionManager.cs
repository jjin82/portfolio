using ClosedXML.Excel;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.VisualBasic.Devices;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


internal static class VersionManager
{
    public static bool LoadExcel()
    {
        var versions = new Dictionary<string, VersionInfo>();

        // 엑셀 파일 경로
        string filePath = @"Excel/DataVersion.xlsx";

        // 엑셀 파일을 열고 워크북을 로드
        using (var workbook = new XLWorkbook(filePath))
        {
            // 첫 번째 워크시트 선택
            var worksheet = workbook.Worksheet(1);

            // 테이블 범위를 사용하여 읽기 (헤더 포함)
            var range = worksheet.RangeUsed();

            // 각 행을 순회하며 데이터 읽기
            foreach (var row in range.RowsUsed())
            {
                // 첫 번째 행은 헤더이므로 제외
                if (row.RowNumber() <= 3)
                    continue;

                // 각 열의 값을 가져와 출력
                var _ = row.Cell(1).GetValue<int>();
                
                // 버전
                var version = row.Cell(2).GetValue<string>();

                int cell = 0;
                switch (ServerConfig.GetServiceMode())
                {
                    case SERVICE_MODE.LIVE: cell = 2; break;
                    case SERVICE_MODE.DEV:  cell = 4; break;
                    case SERVICE_MODE.QA:   cell = 6; break;
                }

                // LIVE, DEV, QA 서비스별 정보.
                var host = row.Cell(++cell).GetValue<string>();
                var port = row.Cell(++cell).GetValue<int>();

                var info = new VersionInfo
                {
                    version = version,
                    host    = host,
                    port    = port,
                    live    = true
                };
                versions.TryAdd(version, info);

                Logger.INFO_PRINT($"Service( {ServerConfig.GetServiceMode()} ), Version: {version}, Host: {host}, Port: {port}");
            }

            if (0 == versions.Count)
                return false;

            _versions = versions;

            Program.UpdateVersionInfo(_versions);
        }

        return true;
    }

    public static VersionInfo GetVersion(string version)
    {
        if (false == _versions.TryGetValue(version, out var info))
        {
            return new VersionInfo();
        }

        return info;
    }

    static Dictionary<string, VersionInfo> _versions = new Dictionary<string, VersionInfo>();
}
