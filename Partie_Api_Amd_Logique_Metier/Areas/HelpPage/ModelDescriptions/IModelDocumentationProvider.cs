using System;
using System.Reflection;

namespace Partie_Api_Amd_Logique_Metier.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}