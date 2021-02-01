namespace Common.Dto
{
    public class LanguageError
    {
        public string Source { get; set; }

        public int Line { get; set; }

        public int StartColumn { get; set; }

        public int EndColumn { get; set; }

        public string Message { get; set; }

        public string OffendingSymbol { get; set; }

        public override string ToString() => $"Fórmula: {Source}, Linha: {Line}, Entre as colunas: {StartColumn} e {EndColumn}, Caractere: {OffendingSymbol}, {Message}";
    }
}
