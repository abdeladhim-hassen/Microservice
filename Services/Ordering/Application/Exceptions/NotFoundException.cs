using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class NotFoundException(string entityName, object entityKey) 
        : ApplicationException(
            $"Entity \"{entityName}\" (Key: {entityKey}) was not found."
            ){}
}
