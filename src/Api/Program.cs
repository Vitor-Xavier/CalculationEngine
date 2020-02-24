using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Tree;
using Api.Dto;

namespace Api {
    public class Program {

        static decimal GerarDecimal () {
            Random rnd = new Random ();
            byte scale = (byte) rnd.Next (29);
            bool sign = rnd.Next (2) == 1;
            var teste = new decimal (rnd.Next (10), rnd.Next (10), rnd.Next (10), sign, scale);
            return teste;
        }

        static void Main (string[] args) {

            Roteiro roteiro = new Roteiro () {
                Id = 1,
                NomeRoteiro = "IPTU",
                Eventos = new List<Evento> ()
            };

            Evento fatorG = new Evento () {
                Id = 1,
                    Nome = "FatorG",
                    Formula = @"(@AreaTerreno * @Testada)/30;"
            };

            Evento fatorK = new Evento () {
                Id = 2,
                    Nome = "FatorK",
                    Formula = @"retorno 5725.90;"
            };

            Evento vvt = new Evento () {
                Id = 3,
                    Nome = "VVT",
                    Formula = @"@FatorG * @FatorK * @Pedologia * @NumeroFrentes * @FatorPosicaoQuadra * @CaracteristicaEspecial * @FracaoIdeal;"
            };

            Evento vvp = new Evento () {
                Id = 4,
                    Nome = "VVP",
                    Formula = @"@AreaEdificada + @ValorM + (@Pontos/100) * @Conservacao * @LocalPropriedadeLote;"
            };

            Evento iptu = new Evento () {
                Id = 5,
                    Nome = "IPTU",
                    Formula = @"@vvp + @vvt;"
            };

            vvt.Eventos = new List<Evento> ();
            vvt.Eventos.Add (fatorK);

            iptu.Eventos = new List<Evento> ();
            iptu.Eventos.Add (vvp);
            iptu.Eventos.Add (vvt);

            roteiro.Eventos.Add (fatorG);
            roteiro.Eventos.Add (fatorK);
            roteiro.Eventos.Add (vvt);
            roteiro.Eventos.Add (vvp);
            roteiro.Eventos.Add (iptu);

            List<ExemploFisico> listaFisico = new List<ExemploFisico> ();

            for (int i = 0; i < 1; i++) {
                var novoFisico = new ExemploFisico () {
                    Id = 0,
                    AreaEdificada = GerarDecimal (),
                    AreaTerreno = GerarDecimal (),
                    Caracteristica = GerarDecimal (),
                    CaracteristicaEspecial = GerarDecimal (),
                    Conservacao = GerarDecimal (),
                    FatorPosicaoQuadra = GerarDecimal (),
                    FracaoIdeal = GerarDecimal (),
                    LocalPropriedadeLote = GerarDecimal (),
                    NumeroFrentes = GerarDecimal (),
                    Pedologia = GerarDecimal (),
                    Pontos = GerarDecimal (),
                    Testada = GerarDecimal (),
                    Topografia = GerarDecimal (),
                    ValorM = GerarDecimal (),

                    Teste01 = GerarDecimal (),
                    Teste02 = GerarDecimal (),
                    Teste03 = GerarDecimal (),
                    Teste04 = GerarDecimal (),
                    Teste05 = GerarDecimal (),
                    Teste06 = GerarDecimal (),
                    Teste07 = GerarDecimal (),

                };
                listaFisico.Add (novoFisico);
            }

            Dictionary<string, GenericValueLanguage> dic = new Dictionary<string, GenericValueLanguage> ();
            // for (int i = 0; i < teste3.Count (); i++) {

            //var eita = teste.GetType ().GetProperties ();
            //  var teste3 = novoFisico.GetType ().GetProperties ();
            //  var teste8 = novoFisico.GetType().GetProperty(teste3[i].Name).GetValue(novoFisico, null);
            // GenericValueLanguage outroteste = new GenericValueLanguage (teste8, false);

            // dic.Add(string.Concat ("@", teste3[i].Name), outroteste);
            // }

            //foreach (var item in roteiro.Eventos) {
            //            ExecuteLanguage execute = new ExecuteLanguage ("var a = 10 * 18 + @Topografia;");

            ExecuteLanguage execute = new ExecuteLanguage ();

            var teste222 = execute.DefaultParserTree ("var aya = 10 * 18; var a = 10 * 18 + 10;@teste");


            // ParseTreeWalker walker = new ParseTreeWalker ();
            // List<string> teste100 = new List < string ();
            // walker.walk(teste100, teste222);

            var value = execute.Execute (dic);

            //};

        }

    }
}