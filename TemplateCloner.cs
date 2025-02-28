using Fz.Tool.Cli.Abstract;
using LibGit2Sharp;

namespace Fz.Tool.Cli
{
	public class TemplateCloner : ITemplateCloner
	{
		public async ValueTask CloneTemplateAsync(string repoUrl, string projectName)
		{
			string tempDirectory = Path.Combine("projects", "temp");
			string projectDirectory = Path.Combine("projects", projectName);
			// 调用 Git 命令行工具或 LibGit2Sharp 进行克隆
			await Task.Run(() => Repository.Clone(repoUrl, tempDirectory));

			// 将克隆的文件夹名称更改为正确的项目名称
			if (Directory.Exists(tempDirectory))
			{
				// 删除旧的文件夹（如果有）并重命名为正确的项目名称
				if (Directory.Exists(projectDirectory))
				{
					Directory.Delete(projectDirectory, true);
				}
				Directory.Move(tempDirectory, projectDirectory);
			}

			/// 删除 .git 文件夹
			string gitDirectory = Path.Combine(projectDirectory, ".git");
			if (Directory.Exists(gitDirectory))
			{
				// 删除目录时强制设置文件属性为正常，并删除
				DeleteDirectoryWithRetry(gitDirectory);
			}
		}

		private void DeleteDirectoryWithRetry(string path)
		{
			// 强制删除目录
			if (Directory.Exists(path))
			{
				try
				{
					// 移除只读属性
					SetDirectoryAttributesNormal(path);

					// 尝试删除文件夹
					Directory.Delete(path, true);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Failed to delete directory {path}: {ex.Message}");
				}
			}
		}

		private void SetDirectoryAttributesNormal(string path)
		{
			DirectoryInfo dirInfo = new DirectoryInfo(path);
			foreach (var file in dirInfo.GetFiles("*", SearchOption.AllDirectories))
			{
				file.Attributes = FileAttributes.Normal;
			}

			foreach (var dir in dirInfo.GetDirectories("*", SearchOption.AllDirectories))
			{
				dir.Attributes = FileAttributes.Normal;
			}
		}
	}
}
