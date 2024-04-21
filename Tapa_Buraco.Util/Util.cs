using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tapa_Buraco.Util
{
    public static class Util
    {
        private static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source, Func<TSource, TSource> nextItem, Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }

        private static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source, Func<TSource, TSource> nextItem) where TSource : class
        {
            return FromHierarchy(source, nextItem, s => s != null);
        }

        public static string GetAllExceptionsMessages(this Exception exception)
        {
            bool suc = false;
            var err = "";
            try
            {
                var messages = exception.FromHierarchy(ex => ex.InnerException)
                    .Select(ex => ex.Message);
                suc = true;

                string a = String.Join(Environment.NewLine, messages);
                return (a);
            }
            catch (Exception e)
            {
                if (exception != null)
                {
                    err = string.IsNullOrEmpty(exception.Message) ? "" : (exception.Message);
                }
                else
                {
                    err = string.IsNullOrEmpty(e.Message) ? "" : (e.Message);
                }
                suc = false;
            }

            return err;
        }
    }
}
