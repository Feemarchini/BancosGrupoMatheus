﻿using AutoMapper;
using BancoGrupoMatheusAPI.Models;
using BancoGrupoMatheusAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BancoGrupoMatheusAPI.Profiles
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<RegistrarNovaConta, Contas>();
            CreateMap<AtualizarContas, Contas>();
            CreateMap<Contas, GetContasModel>();
            CreateMap<RequestTransacaoDto, Transacoes>();
        }
    }
}
