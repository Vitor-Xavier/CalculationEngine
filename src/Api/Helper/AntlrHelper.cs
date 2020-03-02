using Antlr4.Runtime;
using System.Linq;

namespace Api.Helper
{
    public static class AntlrHelper
    {
        public static string ExtractTextToken(int valueTokenTableCaracteristica, IToken[] rangeTokenBuscaCaracteristica, int ordem)
        {
            var tokenCaracteristica = rangeTokenBuscaCaracteristica.Where(x => x.Type == valueTokenTableCaracteristica).ToArray();
            return tokenCaracteristica.Length > ordem ? tokenCaracteristica[ordem].Text : string.Empty;
        }
    }
}
