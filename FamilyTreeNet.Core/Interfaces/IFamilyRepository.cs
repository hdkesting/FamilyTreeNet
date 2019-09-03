﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeNet.Core.Interfaces
{
    public interface IFamilyRepository
    {
        Task<int> Count(bool includeDeleted);

        Task DeleteAll();

    }
}
