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
    public class FaceQuadraRepository
    {
        private readonly CalculationContext _context;

        public FaceQuadraRepository(CalculationContext context) => _context = context;

        public Task<List<FaceQuadra>> GetFacesQuadra(string[] columns, int selectionId, CancellationToken cancellationToken = default) =>
            _context.FacesQuadra.Where(fq => fq.Fisicos.Any(f => f.RoteiroSelecaoItens.Any(s => s.SelecaoId == selectionId))).SelectMembers(columns).OrderBy(x => x.Id).ToListAsync(cancellationToken);
    }
}
