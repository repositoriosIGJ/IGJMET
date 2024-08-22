using System;
using System.Collections.Concurrent;

namespace Domain.Common
{
    public class UserCache
    {
        private static ConcurrentDictionary<string ,Usuario> _list;
        
        static UserCache()
        {
            _list = new ConcurrentDictionary<string, Usuario>();
        }

        public static void Add(Usuario usuario)
        {
            _list.TryAdd(usuario.Alias, usuario);
        }

        public static Usuario Get(string alias)
        {
            Usuario usuario;
            _list.TryGetValue(alias, out usuario);
            return usuario;
        }
        
    }
}
