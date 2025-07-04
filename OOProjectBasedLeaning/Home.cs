using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOProjectBasedLeaning
{
    public interface Home : Model, Place
    {

    }

    public class HomeModel : ModelEntity, Home
    {

        public HomeModel() : this(string.Empty)
        {

        }

        public HomeModel(string name)
        {

            this.Name = name;

        }

    }

    public class NullHome : ModelEntity, Home, NullObject
    {

        private static readonly Home instance = new NullHome();

        private NullHome()
        {

        }

        public override string Name
        {

            get { return string.Empty; }
            set { /* Do nothing */ }

        }

        public static Home Instance
        {
            get { return instance; }
        }

    }
}
