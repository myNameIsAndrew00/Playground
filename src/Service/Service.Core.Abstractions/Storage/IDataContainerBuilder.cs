using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Storage
{
    /// <summary>
    /// Implements methods which can be used to generate data containers from input data
    /// </summary>
    public interface IDataContainerBuilder
    {
        /// <summary>
        /// Creates a data container with the given type and value. If enum type provided, a generic data container will be created.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        IDataContainer CreateDataContainer(ulong type, byte[] bytes, Type enumType);

        /// <summary>
        /// Creates a collection of data containers with the given types and values. If enum type provided, a generic collection of data containers will be created.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        IList CreateDataContainerCollection(IEnumerable<(ulong type, byte[] bytes)> values, Type enumType);
    }
}
