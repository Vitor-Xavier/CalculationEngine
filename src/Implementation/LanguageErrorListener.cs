using Antlr4.Runtime;
using Common.Dto;
using Common.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Implementation
{
    public class LanguageErrorListener : BaseErrorListener
    {
        public string SourceName { get; }

        public ICollection<LanguageError> SyntaxErrors { get; } = new HashSet<LanguageError>();

        public LanguageErrorListener() => SourceName = "Unknown";

        public LanguageErrorListener(string sourceName) => SourceName = sourceName;

        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            Console.WriteLine($"\n\n# {SyntaxErrors.Count}");
            Console.WriteLine(output);
            Console.WriteLine(recognizer);
            Console.WriteLine(offendingSymbol);
            Console.WriteLine(msg);
            Console.WriteLine(e?.GetType());

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

            //base.SyntaxError(output, recognizer, offendingSymbol, line, charPositionInLine, msg, e);
        }


        private string GetExceptionMessage(RecognitionException recognitionException, string offendingSymbol)
        {
            switch (recognitionException)
            {
                case NoViableAltException _: return $"Expressão inválida '{offendingSymbol}'";
                case InputMismatchException _: return $"Entrada '{offendingSymbol}' não esperada";
                case FailedPredicateException _: return $"Semántica '{offendingSymbol}' inválida";
                default: Console.WriteLine(recognitionException); return null;
            };
        }
    }
}
