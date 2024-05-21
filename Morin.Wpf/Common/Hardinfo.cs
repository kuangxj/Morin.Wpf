using System.IO;


namespace Morin.Wpf.Common;

public class Hardinfo
{
    public static long GetHardDiskSpace(string hardDiskName)
    {
        if (string.IsNullOrWhiteSpace(hardDiskName)) return 0;
        var diskName = $"{hardDiskName}:\\";
        var drive = DriveInfo.GetDrives()
            .Where(x => x.Name.Equals(diskName, StringComparison.OrdinalIgnoreCase))
            .FirstOrDefault();

        return drive != null ? drive.TotalSize / (1024 * 1024 * 1024) : 0;
    }
    public static long GetHardDiskFreeSpace(string hardDiskName)
    {
        if (string.IsNullOrWhiteSpace(hardDiskName)) return 0;
        var diskName = $"{hardDiskName}:\\";
        var drive = DriveInfo.GetDrives()
            .Where(x => x.Name.Equals(diskName, StringComparison.OrdinalIgnoreCase))
            .FirstOrDefault();

        return drive != null ? drive.TotalFreeSpace / (1024 * 1024 * 1024) : 0;
    }
}
