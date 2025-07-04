using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOProjectBasedLeaning
{
    public interface Place
    {

    }

    public class NullPlace : Place
    {

        private static readonly Place instance = new NullPlace();

        private NullPlace()
        {
            // プライベートコンストラクタでインスタンス化を防ぐ
        }

        public static Place Instance => instance;

    }
}
