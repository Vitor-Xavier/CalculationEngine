﻿namespace Common.Dto
{
    public class LanguageError
    {
        public string Source { get; set; }

        public int Line { get; set; }

        public int StartColumn { get; set; }

        public int EndColumn { get; set; }

        public string Message { get; set; }

        public string OffendingSymbol { get; set; }
    }
}
