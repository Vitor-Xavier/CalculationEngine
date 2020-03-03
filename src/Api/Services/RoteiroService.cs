using Api.Dto;
using System.Threading.Tasks;

namespace Api.Services
{
    public class RoteiroService
    {
        public async Task<Roteiro> GetRoteiro()
        {
            Roteiro roteiro = new Roteiro
            {
                RoteiroId = 1,
                Nome = "IPTU",
                SetorOrigem = SetorOrigem.Imobiliario
            };

            Evento fatorG = new Evento
            {
                Id = 1,
                Nome = "FatorG",
                Formula = @"
                    var valor = 0.0;
                    var area = _COALESCE(@FisicoAreas[0].Area, @FisicoOutros[0].Percentual, @FacesdaQuadra.LarguraRua, 9.0);
                    var percentual = @FisicoOutros[0].Percentual;
                    se (@Fisico.AreaEdificada > 0.0) {
                        valor = @Fisico.AreaEdificada * 1.05;
                    } senao {
                        valor = @Fisico.AreaEdificada * @Fisico.Testada;
                    }
                    retorno valor * area;"
            };

            Evento fatorK = new Evento
            {
                Id = 2,
                Nome = "FatorK",
                Formula = @"
                    var valor = 0.0;
                    se (@Roteiro.FatorG > 5725.90) {
                        valor = 5725.90;
                    } senao {
                        valor = 3775.90;
                    }
                    retorno valor;"
            };

            Evento vvt = new Evento
            {
                Id = 3,
                Nome = "vvt",
                Formula = @"retorno @Roteiro.FatorG * @Roteiro.FatorK + @Fisico.AreaTerreno;"
            };

            Evento vvp = new Evento
            {
                Id = 4,
                Nome = "vvp",
                Formula = @"retorno @Fisico.AreaTerreno * 100.0;"
            };

            Evento iptu = new Evento
            {
                Id = 5,
                Nome = "IPTU",
                Formula = @"retorno @Roteiro.vvp + @Roteiro.vvt;"
            };

            roteiro.Eventos.Add(fatorG);
            roteiro.Eventos.Add(fatorK);
            roteiro.Eventos.Add(vvt);
            roteiro.Eventos.Add(vvp);
            roteiro.Eventos.Add(iptu);

            await Task.Delay(100);
            return roteiro;
        }
    }
}
