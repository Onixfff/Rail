using System.Windows.Forms;

namespace rail.Models
{
    public class GrouBoxS
    {
        private string _name = default;
        private int _adress = default;

        public GrouBoxS(string name, int adress)
        {
            _name = name;
            _adress = adress;
        }

        public int GetTextInt()
        {
            if (_name != null)
            {
                bool isCompliteParce = int.TryParse(_name, out int result);
                if (isCompliteParce)
                {
                    return result;
                }
                return default;
            }
            return default;
        }

        public void SetText(string text, int adress)
        {
            if (adress == _adress)
            {
                _name = text;
                return;
            }
        }

        public int GetAdress()
        {
            return _adress;
        }
    }
}
