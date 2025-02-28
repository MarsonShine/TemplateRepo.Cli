namespace Fz.Tool.Cli.Common
{
	public class FileHelper
	{
		public static void Rename(string sourceFilePath,string targetFilePath)
		{
			try
			{
				// 检查源文件是否存在
				if (File.Exists(sourceFilePath))
				{
					// 重命名文件（本质是移动并修改文件名）
					File.Move(sourceFilePath, targetFilePath);
					Console.WriteLine("文件重命名成功！");
				}
				else
				{
					Console.WriteLine("源文件不存在！");
				}
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine($"错误: {ex.Message}");
				throw;
			}
		}
	}
}
