using Common.Dto;
using System;

namespace Common.Exceptions
{
    public class LanguageException : Exception
    {
        public LanguageError LanguageError { get; }

        public LanguageException(LanguageError languageError) : base(languageError.Message) => LanguageError = languageError;

        public LanguageException(int line, int startColumn, int endColumn, string message, string offendingSymbol) : base(message) =>
            LanguageError = new LanguageError { Line = line, StartColumn = startColumn, EndColumn = endColumn, Message = message, OffendingSymbol = offendingSymbol };

        public override string ToString() =>
            $"Linha: {LanguageError.Line}, Coluna Inicial: {LanguageError.StartColumn}, Coluna Final: {LanguageError.EndColumn}, Mensagem: {LanguageError.Message}, Texto: '{LanguageError.OffendingSymbol}'";
    }
}
