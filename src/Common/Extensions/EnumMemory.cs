using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Common.Extensions
{
  public enum EnumMemory
  {
    MemoryLocalList,
    MemoryLocal,
    MemoryGlobal,
    Memory,
    MemoryOrMemoryGlobal,
    MemoryLocalOrMemoryLocalList
  }
}
