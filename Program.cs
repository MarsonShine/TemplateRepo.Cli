// See https://aka.ms/new-console-template for more information
using Fz.Tool.Cli;
using System.CommandLine;

// 定义根命令
var rootCommand = new RootCommand("mstool - 项目管理工具");
// 定义子命令 "new"
var newCommand = new Command("new", "创建新项目");
rootCommand.Add(newCommand);

// 定义选项和参数
var repoOption = new Option<string>(
	["--repo", "-r"], // 支持长选项 --repo 和短选项 -r
	() => "https://github.com/MarsonShine/Template.git", // 默认值
	"Git 仓库模板的 URL"
);
var projectNameArg = new Argument<string>(
	"projectName",
	"项目名称（例如 Fz.Web）"
);

// 将选项和参数添加到子命令
newCommand.Add(repoOption);
newCommand.Add(projectNameArg);

// 定义命令的执行行为
newCommand.SetHandler<string, string>(async (repo, projectName) =>
{
	var templateCloner = new TemplateCloner();
	var namespaceReplacer = new NamespaceReplacer();
	var createProjectHandler = new CreateProjectCommandHandler(templateCloner, namespaceReplacer);

	// 使用提供的 repo URL 或默认 URL
	await createProjectHandler.ExecuteAsync(repo, projectName);
}, repoOption, projectNameArg);


await rootCommand.InvokeAsync(args);
