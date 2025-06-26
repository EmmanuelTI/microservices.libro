using MediatR;
using Uttt.Micro.Libro.Persistencia;

public class Editar
{
    public class Ejecutar : IRequest<Unit>  // 👈 Aquí el cambio importante
    {
        public Guid LibroId { get; set; }
        public string Titulo { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public Guid? AutorLibro { get; set; }
    }

    public class Manejador : IRequestHandler<Ejecutar, Unit>
    {
        private readonly ContextoLibreria _context;
        public Manejador(ContextoLibreria context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(Ejecutar request, CancellationToken cancellationToken)
        {
            var libro = await _context.LibreriasMateriales.FindAsync(request.LibroId);
            if (libro == null)
                throw new Exception("No se encontró el libro");

            libro.Titulo = request.Titulo ?? libro.Titulo;
            libro.FechaPublicacion = request.FechaPublicacion ?? libro.FechaPublicacion;
            libro.AutorLibro = request.AutorLibro ?? libro.AutorLibro;

            await _context.SaveChangesAsync();
            return Unit.Value;
        }
    }
}