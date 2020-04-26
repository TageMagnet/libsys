﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public interface IGenericRepository<T>
    {
        // CRUD Create REad
        Task Create(T t);
        Task<T> Read(int id);
        Task Update(T t);
        Task Delete(int id);
        //... extra
        Task<List<T>> ReadAll();
    }
}
