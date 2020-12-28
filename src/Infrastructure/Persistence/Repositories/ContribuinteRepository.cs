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
    public class ContribuinteRepository
    {
        private readonly CalculationContext _context;

        public ContribuinteRepository(CalculationContext context) => _context = context;

        public Task<List<Contribuinte>> GetContribuintes(string[] columns, int selectionId, CancellationToken cancellationToken = default) =>
            _context.Contribuintes.Where(c => c.RoteiroSelecaoItens.Any(s => s.SelecaoId == selectionId)).SelectMembers(columns).OrderBy(x => x.Id).ToListAsync(cancellationToken);
    }
}
