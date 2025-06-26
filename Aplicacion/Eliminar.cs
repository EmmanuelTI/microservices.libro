using MediatR;
using Uttt.Micro.Libro.Persistencia;

public class Eliminar
{
    public class Ejecutas : IRequest<Unit>  // 👈 Aquí también
    {
        public Guid Id { get; set; }
    }

    public class Manejador : IRequestHandler<Ejecutas, Unit>
    {
        private readonly ContextoLibreria _context;
        public Manejador(ContextoLibreria context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(Ejecutas request, CancellationToken cancellationToken)
        {
            var libro = await _context.LibreriasMateriales.FindAsync(request.Id);
            if (libro == null)
                throw new Exception("No se encontró el libro");

            _context.LibreriasMateriales.Remove(libro);
            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}