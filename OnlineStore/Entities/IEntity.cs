﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Entities
{
    public interface IEntity
    {
    }
    public interface IEntity<TIdentifier> : IEntity
    {
        TIdentifier Id { get; }
    }
}