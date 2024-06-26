﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ScreenSound.Modelos;
using ScreenSound.Shared.Dados.Modelos;
using ScreenSound.Shared.Modelos.Modelos;

namespace ScreenSound.Banco;
public class ScreenSoundContext: IdentityDbContext<PessoaComAcesso, PerfilDeAcesso, int>
{
    public DbSet<Artista> Artistas { get; set; }
    public DbSet<Musica> Musicas { get; set; }
    public DbSet<Genero> Generos { get; set; }
    public DbSet<AvaliacaoArtista> Avaliacao { get; set; }

    private string connectionString = "Server=tcp:screensoundserverx.database.windows.net,1433;Initial Catalog=ScreenSoundV0;Persist Security Info=False;User ID=joao;Password=@Quiziri1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

    public ScreenSoundContext()
    {

    }

    public ScreenSoundContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }
        optionsBuilder.UseSqlServer(connectionString).UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Musica>().HasMany(c => c.Generos).WithMany(c => c.Musicas);

        modelBuilder.Entity<AvaliacaoArtista>().HasKey(a => new { a.ArtistaId, a.PessoaId });
    }
}
