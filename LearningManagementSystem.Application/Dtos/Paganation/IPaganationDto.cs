using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Dtos.Paganation
{
    public interface IPaganationDto
    {
        int CurrentPage { get; }
        bool HasNext { get; }
        bool HasPrev { get; }

    }
}
