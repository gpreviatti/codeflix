using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Common;
public static class StorageFileName
{
    public static string Create(Guid id, string propertyName, string extension)
        => $"{id}-{propertyName.ToLower()}.{extension.Replace(".", "")}";
}
