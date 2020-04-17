using Antlr4.Runtime;
using Common.Dto;
using Common.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Implementation
{
    public class LanguageErrorListener : BaseErrorListener
    {
        public ICollection<LanguageError> SyntaxErrors { get; } = new HashSet<LanguageError>();

        /// <summary>
        /// Identifica erros de sintaxe na análise do código, e os adiciona a lista de erros no formato padrão.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="recognizer"></param>
        /// <param name="offendingSymbol">Entrada qie causou o erro</param>
        /// <param name="line">Linha</param>
        /// <param name="charPositionInLine">Coluna</param>
        /// <param name="msg">Mensagem do Antlr4</param>
        /// <param name="e">Erro de reconhecimento</param>
        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            string message;
            if (RegexHelper.MissingChar.Match(msg) is Match matchMissing && matchMissing.Success)
            {
                message = $"Faltando {(matchMissing.Groups["char"].Success ? matchMissing.Groups["char"].Value : "identificador ou valor")}";
            }
            else if (RegexHelper.ExtraneousInput.Match(msg) is Match matchExtraneous && matchExtraneous.Success)
            {
                message = matchExtraneous.Groups["char"].Success ? $"Caractere '{matchExtraneous.Groups["char"].Value}' não esperado" : "Entidade ou valor não indentificado";
            }
            else
            {
                message = GetExceptionMessage(e, offendingSymbol.Text);
            }

            SyntaxErrors.Add(new LanguageError
            {
                Line = line,
                StartColumn = charPositionInLine,
                EndColumn = offendingSymbol.StopIndex - offendingSymbol.StartIndex + charPositionInLine,
                Message = message ?? msg,
                OffendingSymbol = offendingSymbol.Text
            });
        }

        /// <summary>
        /// Trata o erro para fornecer mensagem em português conforme exceção lançada.
        /// </summary>
        /// <param name="recognitionException">Exceção</param>
        /// <param name="offendingSymbol">Símbolo que ocasionou o erro</param>
        /// <returns>Mensagem tratada</returns>
        private string GetExceptionMessage(RecognitionException recognitionException, string offendingSymbol) => recognitionException switch
        {
            NoViableAltException _ => $"Expressão inválida '{offendingSymbol}'",
            InputMismatchException _ => $"Entrada '{offendingSymbol}' não esperada",
            FailedPredicateException _ => $"Semántica '{offendingSymbol}' inválida",
            _ => "Erro indefinido"
        };
    }
}
