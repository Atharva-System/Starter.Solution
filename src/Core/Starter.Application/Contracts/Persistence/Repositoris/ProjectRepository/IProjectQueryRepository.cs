using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Starter.Application.Contracts.Persistence.Repositoris.Base;
using Starter.Application.Contracts.Responses;
using Starter.Application.Features.Projects.Dto;
using Starter.Domain.Entities;

namespace Starter.Application.Contracts.Persistence.Repositoris.ProjectRepository;
public interface IProjectQueryRepository : IQueryRepository<Project>
{
}
