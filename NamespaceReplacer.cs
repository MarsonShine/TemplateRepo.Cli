using Fz.Tool.Cli.Abstract;
using Fz.Tool.Cli.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fz.Tool.Cli
{
	public class NamespaceReplacer : INamespaceReplacer
	{
		public async ValueTask ReplaceNamespaceAsync(string originProjectName, string projectName)
		{
			// 替换 .cs 文件中的命名空间
			await ReplaceInFilesAsync("*.cs", originProjectName, projectName);

			// 替换 .csproj 文件中的命名空间
			await ReplaceInFilesAsync("*.csproj", originProjectName, projectName);

			// 替换 .sln 文件中的命名空间
			await ReplaceInFilesAsync("*.sln", originProjectName, projectName);

			// 遍历并修改子目录的名称
			RenameSubdirectories("projects", originProjectName, projectName);
		}

		private static async ValueTask ReplaceInFilesAsync(string filePattern, string originProjectName, string projectName)
		{
			var files = Directory.GetFiles("projects", filePattern, SearchOption.AllDirectories);
			foreach (var file in files)
			{
				var content = await File.ReadAllTextAsync(file);

				// 使用正则表达式进行命名空间替换，替换项目中的命名空间
				var newContent = Regex.Replace(content, originProjectName, projectName);

				// 替换文件内容
				await File.WriteAllTextAsync(file, newContent);
			}
		}

		private static void RenameProjectFiles(string originProjectName, string projectName)
		{
			// 重命名 .csproj 文件
			var csprojFiles = Directory.GetFiles("projects", $"{originProjectName}*.csproj", SearchOption.AllDirectories);
			foreach (var file in csprojFiles)
			{
				string targetFileName = file.Replace(originProjectName, projectName);
				FileHelper.Rename(file, targetFileName);
			}

			// 重命名 .sln 文件
			var slnFiles = Directory.GetFiles("projects", $"{originProjectName}*.sln", SearchOption.AllDirectories);
			foreach (var file in slnFiles)
			{
				string targetFileName = file.Replace(originProjectName, projectName);
				FileHelper.Rename(file, targetFileName);
			}
		}

		private static void RenameSubdirectories(string rootDirectory, string originProjectName, string projectName)
		{
			// 获取所有子目录
			var subDirectories = Directory.GetDirectories(rootDirectory, "*", SearchOption.AllDirectories);
			foreach (var directory in subDirectories)
			{
				string newDirectoryName = directory.Replace(originProjectName, projectName);

				if (newDirectoryName != directory)
				{
					// 重命名子目录
					try
					{
						// 判断 directory 是否存在
						if (!Directory.Exists(directory))
						{
							continue;
						}
						Directory.Move(directory, newDirectoryName);
					}
					catch
					{
						Console.WriteLine($"Failed to rename directory {directory} to {newDirectoryName}");
					}
				}
			}

			// 重命名 .csproj 和 .sln 文件
			RenameProjectFiles(originProjectName, projectName);
		}
	}
}
