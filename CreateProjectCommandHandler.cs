using Fz.Tool.Cli.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fz.Tool.Cli
{
	public class CreateProjectCommandHandler(ITemplateCloner templateCloner, INamespaceReplacer namespaceReplacer) : ICommandHandler
	{
		private readonly ITemplateCloner _templateCloner = templateCloner;
		private readonly INamespaceReplacer _namespaceReplacer = namespaceReplacer;

		public async ValueTask ExecuteAsync(string repoUrl, string projectName)
		{
			await _templateCloner.CloneTemplateAsync(repoUrl, projectName);
			// 解析repoUrl的名称，替换项目名称
			string originProjectName = repoUrl.Split('/').Last().Replace(".git", "");
			await _namespaceReplacer.ReplaceNamespaceAsync(originProjectName, projectName);
		}
	}
}
