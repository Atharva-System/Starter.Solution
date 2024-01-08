using System.Text;
using RazorEngineCore;
using Starter.Application.Contracts.Mailing;

namespace Starter.InfraStructure.Mailing;
public class EmailTemplateService : IEmailTemplateService
{
    public string GenerateDefaultEmailTemplate<T>(T mailTemplateModel)
    {
        string template = GetTemplate("index");

        IRazorEngine razorEngine = new RazorEngine();
        IRazorEngineCompiledTemplate modifiedTemplate = razorEngine.Compile(template);

        return modifiedTemplate.Run(mailTemplateModel);
    }

    public static string GetTemplate(string templateName)
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string tmplFolder = Path.Combine(baseDirectory, "EmailTemplates");
        string filePath = Path.Combine(tmplFolder, $"{templateName}.cshtml");

        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var sr = new StreamReader(fs, Encoding.Default);
        string mailText = sr.ReadToEnd();
        sr.Close();

        return mailText;
    }
}
