using System.Windows.Forms;

namespace rail.Models
{
    public class GrouBoxS
    {
        private GroupBox _name = default;
        private int _adress = default;

        public GrouBoxS(GroupBox name, int adress)
        {
            _name = name;
            _adress = adress;
        }

        public int GetTextInt()
        {
            if (_name != null)
            {
                bool isCompliteParce = int.TryParse(_name.Text, out int result);
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
                _name.Text = text;
                return;
            }
        }

        public int GetAdress()
        {
            return _adress;
        }
    }
}
