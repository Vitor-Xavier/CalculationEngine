using Infrastructure.Extensions;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class FisicoRepository : Repository<Fisico, CalculationContext>
    {
        public FisicoRepository(CalculationContext context) : base(context) { }

        public Task<List<Fisico>> GetFisicos(string[] columns, int selectionId, CancellationToken cancellationToken = default) =>
            _context.Fisicos.Where(f => f.RoteiroSelecaoItens.Any(s => s.SelecaoId == selectionId)).SelectMembers(columns).OrderBy(x => x.Id).ToListAsync(cancellationToken);
    }
}
