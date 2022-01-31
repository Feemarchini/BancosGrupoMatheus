using Microsoft.EntityFrameworkCore;
using BancoGrupoMatheusAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BancoGrupoMatheusAPI.DAL
{
    public class DBContextBancoGrupoMatheus : DbContext
    {
        public DBContextBancoGrupoMatheus(DbContextOptions<DBContextBancoGrupoMatheus> options) : base(options)
        {

        }
        public DbSet<Response> Response { get; set; }
        public DbSet<Contas> Contas { get; set; }
        public DbSet<Transacoes> Transacoes { get; set; }
        public DbSet<RegistrarNovaConta> RegistrarNovaConta { get; set; }
        public DbSet<AtualizarContas> AtualizarContas { get; set; }

    }
}
