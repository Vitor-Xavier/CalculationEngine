using Antlr4.Runtime.Tree;

namespace Api.Dto
{
    public class Evento
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Formula { get; set; }

        public IParseTree ParseTree { get; set; }
    }
}