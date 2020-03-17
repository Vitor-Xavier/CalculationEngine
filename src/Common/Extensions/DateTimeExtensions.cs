using System;

namespace Common.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Calcula a diferença entre duas datas em meses.
        /// </summary>
        /// <param name="left">Data inicial</param>
        /// <param name="right">Data final</param>
        /// <returns>Valor absoluto</returns>
        public static int MonthDifference(this DateTime left, DateTime right) =>
            Math.Abs((right.Year * 12 + right.Month) - (left.Year * 12 + left.Month));
    }
}
